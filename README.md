# FHTW - SFR

test

## Setup Kafka

### 1. Run docker-compose
```
docker-compose up -d
````

This will create a Kafka Cluster with 3 brokers with the new KRaft mode therefore not relying on ZooKeeper instances.

### 2. Create topics

Go to http://localhost:8080/ui/clusters/local/all-topics  
You can create the needed topics here, however when running DataSource the topics will be created automatically with the intended number of partition, replica and min.insync.replica.

### 3. Create Schemas

The schemas in AVRO format will be created automatically by DataSource and used by DataIngest.

### 4. Start DB

To start the DB just use following command
```
docker-compose -f .\docker-compose-db.yml up -d
```

Write and Read DB ip is: `localhost:5432`

There a to replicas of this db acessible on ips `localhost:5433` and `localhost5434`

The replicas are read only copy of the leader DB. This replicas backups the data of the leader DB and prevents data loss if the leader DB fails. Additionally a failing leader DB would not break the feature of the public API as the read only API connects to one or both replicas. We decided that high availability writes where not as important for our use case but it would be possible to modify the config automatically on leader failure to create a new leader


### 5. Start DataSource command line app

Required arguments are in following format
```
DataSource.dll broker-ip:port,broker-ip:port,... schema-registry-ip:port,schema-registry-ip:port,... ack-all|ack-none|ack-leader max-flush-timeout-seconds retry-interval-seconds open-meteo-topic-name weatherapi-topic-name
```
ex:
```
localhost:9192,localhost:9292,localhost:9392 localhost:8081 ack-all 10 10 openmeteo weatherapi
```

The app will send messages containing weather forecast data for each hour of the day from each of the two weather APIs to Kafka.
It is designed to run once per day using an external scheduling mechanism (e.g. Windows Task Scheduler, cron job). Once all messages were sent successfully, the application terminates.

### 6. Start the DataTransformer command line app
Listens to the incoming weather data on a specific topic and transforms Fahrenheit temperature values into Celsius values via Kafka Streams  

Required arguments are in the following format
```
broker-ip:port,broker-ip:port,... schema-registry-ip:port,schema-registry-ip:port... open-meteo-topic-name weatherapi-topic-name
```
ex:
```
http://127.0.0.1:9192,http://127.0.0.1:9292,http://127.0.0.1:9392 http://127.0.0.1:8081 openmeteo weatherapi
```


### 7. Start DataIngest command line app

Required arguments are in following format
```
DataIngest.dll broker-ip:port,broker-ip:port,... schema-registry-ip:port,schema-registry-ip:port,... group-id open-meteo-topic-name weatherapi-topic-name db-connection-string retry-interval
```
ex:
```
localhost:9192,localhost:9292,localhost:9392 localhost:8081 ingest openmeteo weatherapi "Host=localhost:5432; Database=postgres; Username=postgres; Password=password" 10
```

The app will receive the messages sent by DataSource from Kafka and persist their contents into the database.

### 8. Start ApiService
Run the ApiService container. No args needed.

### 9. Start WebClient
Run the webclient. No args needed.

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
