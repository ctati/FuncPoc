# fluentd/Dockerfile
FROM fluent/fluentd:v1.14-debian-1
USER root
RUN ["gem", "install", "fluent-plugin-elasticsearch", "--no-document"]
#RUN ["fluent-gem", "install", "--no-document", "fluent-plugin-elasticsearch"]
#USER fluent
