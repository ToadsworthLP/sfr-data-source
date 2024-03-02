# FHTW - SFR

## Setup Kafka

### 1. Run docker-compose
```
docker-compose up -d
````

This will create a Kafka Cluster with 3 brokers with the new KRaft mode therefore not relying on ZooKeeper instances.

### 2. Create topics

`actual weather` & `forecast`

Set number of partitions, replica & keep duration

### 3. Start DataSource command line app

Required arguments are in following format
```
broker-ip:port,broker-ip:port,... ack-all|ack-none|ack-leader max-flush-timeout-seconds retry-interval forecast-topic-name actual-weather-topic-name
```
ex:
```
localhost:9192,localhost:9292,localhost:9392 ack-none 10 10 forecast actual-weather
```

The app will send one message to both topics and shutdown.