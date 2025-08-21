all: build

build:
	@echo "Building..."
	@go build -o main cmd/api/main.go

run:
	@go run cmd/api/main.go

install_sqlc:
	go install github.com/sqlc-dev/sqlc/cmd/sqlc@latest

install_goose:
	go install github.com/pressly/goose/v3/cmd/goose@latest
	
goose_up:
	@echo "Running migrations..."
	cd db/migrations && goose postgres postgres
	
sqlc:
	sqlc generate