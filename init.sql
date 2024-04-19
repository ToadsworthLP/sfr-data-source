CREATE TABLE IF NOT EXISTS weather_data (
    id UUID primary key NOT NULL,
    timestamp TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    provider SMALLINT NOT NULL,
    temperature DOUBLE PRECISION NOT NULL,
    temperature_unit SMALLINT NOT NULL,
    pressure DOUBLE PRECISION NOT NULL,
    pressure_unit SMALLINT NOT NULL
);