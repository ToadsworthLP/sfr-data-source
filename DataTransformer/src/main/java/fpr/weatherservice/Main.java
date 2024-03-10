package fpr.weatherservice;
import fpr.weatherservice.serializer.WeatherDeserializer;
import fpr.weatherservice.serializer.WeatherSerializer;
import org.apache.kafka.streams.KafkaStreams;
import org.apache.kafka.streams.StreamsBuilder;

import org.apache.kafka.common.serialization.Serde;
import org.apache.kafka.common.serialization.Serdes;
import org.apache.kafka.streams.kstream.Consumed;
import org.apache.kafka.streams.kstream.Produced;

import java.util.concurrent.CountDownLatch;

public class Main {
    public static void main(String[] args) {
        final var props = KafkaHelper.localClusterConfig();
        final StreamsBuilder builder = new StreamsBuilder();

        builder.stream("actual-weather", Consumed.with(Serdes.String(), KafkaHelper.weatherSerde))
            .mapValues(value -> {
                value.temperature = value.temperature * 1.8f + 32;
                return value;
            })
            .to("actual-weather-fareinheit", Produced.with(Serdes.String(), KafkaHelper.weatherSerde));


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
}