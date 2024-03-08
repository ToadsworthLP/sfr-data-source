# FHTW - SFR

## Setup Kafka

### 1. Run docker-compose
```
docker-compose up -d
````

This will create a Kafka Cluster with 3 brokers with the new KRaft mode therefore not relying on ZooKeeper instances.

### 2. Create topics

Go to http://localhost:8080/ui/clusters/local/all-topics

`actual weather` & `forecast`

Set number of partitions, replica & keep duration+

### 4. Create Schemas

1. Go to http://localhost:8080/ui/clusters/local/schemas
2. Click on `Create Schema`
2. Give the Schema a subject ex: `weather-forecast-day`
3. Create a Schema in AVRO format ex:
```JSON
{
  "type": "record",
  "name": "forecastWeatherDay",
  "doc": "This record contains a weather forecast day",
  "namespace": "fhtw.sfr",
  "fields": [
    {
      "name": "day",
      "type": "string",
      "doc": "Day string in format DD.MM.YYYY"
    },
    {
      "name": "minTemp",
      "type": "float",
      "doc": "Minimum temperature of the day in C"
    },
    {
      "name": "maxTemp",
      "type": "float",
      "doc": "Maximum temperature of the day in C"
    }
  ]
}
```

### 5. Start DataSource command line app

Required arguments are in following format
```
broker-ip:port,broker-ip:port,... ack-all|ack-none|ack-leader max-flush-timeout-seconds retry-interval forecast-topic-name actual-weather-topic-name
```
ex:
```
localhost:9192,localhost:9292,localhost:9392 ack-none 10 10 forecast actual-weather
```

The app will send one message to both topics and shutdown.

## Choice of number of brokers, partitions and replica

We choose to follow the recommendation: https://www.conduktor.io/kafka/kafka-topics-choosing-the-replication-factor-and-partitions-count/ for deciding which setup we want to select.

 - Clusters: 1
 - Brokers per cluster: 3
 - Partitions per topic: 9
 - Replica per topic: 3
 - in.sync.replica: 2 

We decided to choose a cluster of 3 brokers which is probably already overkill for such a small project however 3 brokers make it possible to have a redundancy and some flexibility.
We decided to choose 9 partitions per topic by following the recommendation that for small cluster partition number should be 3x higher than number of brokers. As adding partitions after the fact can be problematic it is preferable to have more partitions than currently needed.
Replica per topic are set to 3 to give redundancy split across all 3 brokers.
We decided to set the in.sync.replica to 2 to keep have the best of both worlds the possibility to have one broker fail but the certainty that the data is always replicated on at least two brokers.