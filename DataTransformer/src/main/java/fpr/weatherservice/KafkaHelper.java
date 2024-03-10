package fpr.weatherservice;

import fpr.weatherservice.serializer.WeatherDeserializer;
import fpr.weatherservice.serializer.WeatherSerializer;
import org.apache.kafka.common.serialization.Serde;
import org.apache.kafka.common.serialization.Serdes;
import org.apache.kafka.streams.StreamsConfig;

import java.util.Properties;

public class KafkaHelper {
    public static final Serde<Weather> weatherSerde = Serdes.serdeFrom(new WeatherSerializer(), new WeatherDeserializer());

    public static Properties localClusterConfig() {
        Properties props = new Properties();
        props.put(StreamsConfig.APPLICATION_ID_CONFIG, "WeatherDataTransformer");
        props.put(StreamsConfig.BOOTSTRAP_SERVERS_CONFIG, "localhost:9192;localhost:9292;localhost:9392");
        props.put(
                StreamsConfig.DEFAULT_KEY_SERDE_CLASS_CONFIG,
                Serdes.String().getClass().getName());
        props.put(
                StreamsConfig.DEFAULT_VALUE_SERDE_CLASS_CONFIG,
                Serdes.String().getClass().getName());
        return props;
    }
}
