FROM golang:1.24.4-alpine AS build

WORKDIR /src
COPY "src/TD.TraceLog/" ./
RUN CGO_ENABLED=0
RUN GOOS=linux
RUN go build -o main ./cmd/api/main.go

FROM alpine:latest
WORKDIR /app
COPY --from=build /src/main /app/main

EXPOSE 8080
CMD ["./main"]