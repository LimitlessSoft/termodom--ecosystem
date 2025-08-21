FROM golang:1.24.4-alpine AS build

RUN apk add --no-cache curl

WORKDIR /app

COPY src/TD.TraceLog/go.mod src/TD.TraceLog/go.sum ./
RUN go mod download

COPY . .
RUN CGO_ENABLED=0 GOOS=linux go build -o main ./cmd/api

FROM alpine:latest

WORKDIR /app
COPY --from=build /app/main .

EXPOSE 8080
CMD ["./main"]