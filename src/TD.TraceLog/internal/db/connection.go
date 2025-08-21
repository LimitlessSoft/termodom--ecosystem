package db

import (
	"context"
	"fmt"
	"gin-trace-logs/internal/constants"
	"gin-trace-logs/internal/db/sqlc"
	"gin-trace-logs/internal/vault"
	"log"
	"os"

	"github.com/jackc/pgx/v5/pgxpool"
	_ "github.com/jackc/pgx/v5/stdlib"
)

type DB struct {
	Queries *sqlc.Queries
	Pool    *pgxpool.Pool
}

var Instance *DB

func Initialize() {
	ctx := context.Background()
	var err error
	Instance, err = New(ctx)
	if err != nil {
		log.Fatal(err)
	}
	log.Println("Database initialized successfully.")
}

func New(ctx context.Context) (*DB, error) {
	connectionString := fmt.Sprintf(
		"postgres://%s:%s@%s:%s/%s?sslmode=disable&search_path=%s",
		vault.Secrets.DbUsername,
		vault.Secrets.DbPassword,
		vault.Secrets.DbHost,
		vault.Secrets.DbPort,
		fmt.Sprintf("%s_%s", os.Getenv(constants.Env.App), vault.Secrets.DbName),
		vault.Secrets.DbSchema,
	)

	connPool, err := pgxpool.New(ctx, connectionString)
	if err != nil {
		return nil, fmt.Errorf(constants.Messages.ConnectionPoolCreationError, err)
	}
	if err := connPool.Ping(ctx); err != nil {
		return nil, fmt.Errorf(constants.Messages.ConnectionPoolPingError, err)
	}
	log.Println("Connection pool created.")

	queries := sqlc.New(connPool)

	return &DB{
		Queries: queries,
		Pool:    connPool,
	}, nil
}
