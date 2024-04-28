package fpr.weatherservice;

import WeatherService.WeatherMessage;
import io.confluent.kafka.streams.serdes.avro.SpecificAvroSerde;
import org.apache.kafka.common.serialization.Serde;
import org.apache.kafka.common.serialization.Serdes;
import org.apache.kafka.streams.KafkaStreams;
import org.apache.kafka.streams.StreamsBuilder;
import org.apache.kafka.streams.StreamsConfig;
import org.apache.kafka.streams.kstream.Consumed;
import org.apache.kafka.streams.kstream.Produced;

import java.util.*;
import java.util.concurrent.CountDownLatch;

public class Main {
    public static void main(String[] args) {
        if (args.length <= 0) {
            System.err.println("Parameters: broker-ip:port,broker-ip:port,... schema-registry-ip:port,schema-registry-ip:port,... topic1 topic2...");
            return;
        }

        String kafkaAddresses = args[0];
        String schemaRegistryAddresses = args[1];

        ArrayList<String> topics = new ArrayList<>();
        for (int i = 2; i < args.length; i++) {
            topics.add(args[i]);
        }

        Properties props = new Properties();
        props.put(StreamsConfig.APPLICATION_ID_CONFIG, "WeatherDataTransformer");
        props.put(StreamsConfig.BOOTSTRAP_SERVERS_CONFIG, kafkaAddresses);

        final var builder = new StreamsBuilder();

        final Map<String, String> serdeConfig = Collections.singletonMap("schema.registry.url", schemaRegistryAddresses);

        final Serde<WeatherMessage> valueSpecificAvroSerde = new SpecificAvroSerde<>();
        valueSpecificAvroSerde.configure(serdeConfig, false);

        for (var topic: topics) {
            try {
                builder.stream(topic + "-raw", Consumed.with(Serdes.String(), valueSpecificAvroSerde))
                        .mapValues(Main::BringToSameUnits)
                        .to(topic, Produced.with(Serdes.String(), valueSpecificAvroSerde));
            } catch (Exception e) {
                System.err.println("Could not transform from topic " + topic);
                System.err.println(e.getMessage());
            }
        }

        final var topology = builder.build();
        final var latch = new CountDownLatch(1);
        try (final var streams = new KafkaStreams(topology, props)) {
            streams.start();
            Runtime.getRuntime().addShutdownHook(new Thread(() -> {
                streams.close();
                latch.countDown();
            }));
            latch.await();
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }

    private static WeatherMessage BringToSameUnits(WeatherMessage weather) {
        if (weather.getTemperatureUnit().toString().equals("f")) {
            var temp = (weather.getTemperature() - 32) / 1.8f;
            weather.setTemperature(Math.round(temp * 100) / 100.0);
            weather.setTemperatureUnit("c");
        }

        System.out.println("Processed message with key " + weather.getTimestamp());
        return weather;
    }
}