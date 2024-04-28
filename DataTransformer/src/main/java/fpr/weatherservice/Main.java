package fpr.weatherservice;
import fpr.weatherservice.serializer.WeatherMessage;
import fpr.weatherservice.serializer.WeatherSerde;
import io.confluent.kafka.schemaregistry.client.CachedSchemaRegistryClient;
import io.confluent.kafka.serializers.KafkaAvroSerializer;
import org.apache.kafka.common.serialization.Serdes;
import org.apache.kafka.streams.KafkaStreams;
import org.apache.kafka.streams.StreamsBuilder;
import org.apache.kafka.streams.kstream.Consumed;
import org.apache.kafka.streams.kstream.Produced;

import java.util.HashMap;
import java.util.Objects;
import java.util.concurrent.CountDownLatch;

public class Main {
    public static void main(String[] args) {
        if (args.length <= 0) {
            System.err.println("You must specify the Topics as commandline args!");
            return;
        }

        final var props = KafkaHelper.localClusterConfig();
        final var builder = new StreamsBuilder();

        for (var topic: args) {
            try {


                builder.stream(topic + "-raw", Consumed.with(Serdes.String(), WeatherSerde.Serde))
                        .mapValues(Main::BringToSameUnits)
                        .to(topic, Produced.with(Serdes.String(), WeatherSerde.Serde));
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

    private static WeatherMessage BringToSameUnits(Object obj) {
        var weather = (WeatherMessage) obj;
        if (Objects.equals(weather.getTemperatureUnit(), "f")) {
            var temp = (weather.getTemperature() - 32) / 1.8f;
            weather.setTemperature(Math.round(temp * 100) / 100.0);
            weather.setTemperatureUnit("c");
        }
        return weather;
    }
}