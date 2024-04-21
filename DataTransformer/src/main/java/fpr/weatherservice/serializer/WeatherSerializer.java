package fpr.weatherservice.serializer;

import com.fasterxml.jackson.databind.ObjectMapper;
import fpr.weatherservice.Weather;
import org.apache.kafka.common.errors.SerializationException;
import org.apache.kafka.common.serialization.Serializer;

public class WeatherSerializer implements Serializer<Weather> {
    private final ObjectMapper objectMapper = new ObjectMapper();

    @Override
    public byte[] serialize(String topic, Weather data) {
        try {

            if (data == null) {
                System.err.println("Can not serialize null Weather!");
                return null;
            }
            return objectMapper.writeValueAsBytes(data);
        } catch (Exception e) {
            throw new SerializationException("Error when serializing Weather: " + e.getMessage());
        }
    }

}
