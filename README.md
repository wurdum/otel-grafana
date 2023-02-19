# An example of .NET application instrumented with OpenTelemetry and Grafana stack

This repository contains a sample .NET application exports metrics, traces and logs to OpenTelemetry Collector that pushes them further to Grafana stack - Loki, Tempo and Prometheus.

To run the example:

```sh
docker-compose up
curl http://localhost:5001/ # to generate some metrics/traces/logs
```

Open Grafana in browser http://localhost:3000/ and explore the metrics, traces and logs.
