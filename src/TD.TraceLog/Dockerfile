FROM golang:1.24.4-alpine AS build

WORKDIR /src
COPY . .
RUN go mod download
WORKDIR /cmd/api
RUN CGO_ENABLED=0 GOOS=linux go build -o main

FROM alpine:latest
WORKDIR /app
COPY --from=build /src/main .

EXPOSE 8080
CMD ["./main"]