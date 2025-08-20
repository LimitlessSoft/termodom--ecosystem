package db

import (
	"context"
	"fmt"
	"gin-trace-logs/internal/db/sqlc"
	"gin-trace-logs/internal/vault"
	"github.com/jackc/pgx/v5/pgxpool"
	_ "github.com/jackc/pgx/v5/stdlib"
	"log"
	"os"
)

type DB struct {
	Queries *sqlc.Queries
	Pool    *pgxpool.Pool
}

func New(ctx context.Context) (*DB, error) {
	appDSN := fmt.Sprintf(
		"postgres://%s:%s@%s:%s/%s?sslmode=disable&search_path=%s",
		vault.Secrets.DbUsername,
		vault.Secrets.DbPassword,
		vault.Secrets.DbHost,
		vault.Secrets.DbPort,
		fmt.Sprintf("%s_%s", os.Getenv("APP_ENV"), vault.Secrets.DbName),
		vault.Secrets.DbSchema,
	)

	//db, err := sql.Open("pgx", appDSN)
	//if err != nil {
	//	return nil, fmt.Errorf("unable to open database for migrations: %w", err)
	//}
	//defer db.Close()
	//
	//if err := RunMigrations(db); err != nil {
	//	return nil, err
	//}
	//
	//log.Println("Database migrations complete.")

	connPool, err := pgxpool.New(ctx, appDSN)
	if err != nil {
		return nil, fmt.Errorf("unable to create connection pool: %w", err)
	}
	if err := connPool.Ping(ctx); err != nil {
		return nil, fmt.Errorf("unable to ping connection pool: %w", err)
	}
	log.Println("Connection pool created.")

	queries := sqlc.New(connPool)

	return &DB{
		Queries: queries,
		Pool:    connPool,
	}, nil
}

//func EnsureDatabase(ctx context.Context) error {
//	conn, err := pgx.Connect(ctx, buildDSN("postgres"))
//	if err != nil {
//		return fmt.Errorf("failed to connect to server: %w", err)
//	}
//	defer conn.Close(ctx)
//
//	var exists bool
//	err = conn.QueryRow(ctx, "SELECT EXISTS(SELECT 1 FROM pg_database WHERE datname = $1)", vault.Secrets.DbName).Scan(&exists)
//	if err != nil {
//		return fmt.Errorf("failed to check DB existence: %w", err)
//	}
//
//	if !exists {
//		_, err = conn.Exec(ctx, "CREATE DATABASE "+vault.Secrets.DbName)
//		if err != nil {
//			return fmt.Errorf("failed to create DB: %w", err)
//		}
//		log.Printf("Database %s created", vault.Secrets.DbName)
//	}
//	return nil
//}
