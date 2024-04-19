CREATE TABLE IF NOT EXISTS weather_data (
    id UUID primary key NOT NULL,
    timestamp TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    provider VARCHAR(50) NOT NULL,
    temperature DOUBLE PRECISION NOT NULL,
    temperature_unit VARCHAR(10) NOT NULL,
    pressure DOUBLE PRECISION NOT NULL,
    pressure_unit VARCHAR(10) NOT NULL
);