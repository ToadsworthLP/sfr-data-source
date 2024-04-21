package fpr.weatherservice;

import org.apache.kafka.connect.data.Time;

public class Weather {
    public String timestamp;
    public double temperature = 0.0;
    public String temperature_unit = "c";
    public int pressure = 0;
    public String pressure_unit = "mb";
}
