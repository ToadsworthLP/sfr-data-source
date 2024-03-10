package fpr.weatherservice.serializer;

import com.fasterxml.jackson.databind.ObjectMapper;
import fpr.weatherservice.Weather;
import org.apache.kafka.common.errors.SerializationException;
import org.apache.kafka.common.serialization.Deserializer;

import java.nio.charset.StandardCharsets;

public class WeatherDeserializer implements Deserializer<Weather> {
    private final ObjectMapper objectMapper = new ObjectMapper();

    @Override
    public Weather deserialize(String topic, byte[] data) {
        try {
            if (data == null) {
                System.err.println("Cannot deserialize null Weather");
                return null;
            }
            return objectMapper.readValue(new String(data, StandardCharsets.UTF_8), Weather.class);
        } catch (Exception e) {
            throw new SerializationException("Error when deserializing Weather: " + e.getMessage());
        }
    }
}
