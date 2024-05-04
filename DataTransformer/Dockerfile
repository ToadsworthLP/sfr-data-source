# Build
FROM maven:latest AS build
WORKDIR /app
COPY pom.xml .
COPY src ./src
RUN mvn clean package -DskipTests -Dmaven.artifact.threads=30 -X

# Serve
FROM eclipse-temurin:21
WORKDIR /app
COPY --from=build /app/target/DataTransformer-1.0-SNAPSHOT-jar-with-dependencies.jar DataTransformer-1.0-SNAPSHOT-jar-with-dependencies.jar
CMD ["java", "-jar", "DataTransformer-1.0-SNAPSHOT-jar-with-dependencies.jar", "http://127.0.0.1:9192,http://127.0.0.1:9292,http://127.0.0.1:9392", "http://127.0.0.1:8081", "openmeteo", "weatherapi" ]
