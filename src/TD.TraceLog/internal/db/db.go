package db

import (
	"database/sql"
	"fmt"
	"github.com/filipcvejic/trace-logs/internal/db/sqlc"
	"github.com/filipcvejic/trace-logs/internal/util"
	"log"
	"time"

	_ "github.com/jackc/pgx/v5/stdlib"
	_ "github.com/joho/godotenv/autoload"
)

type Service interface {
	Close() error
	Queries() *sqlc.Queries
}

type service struct {
	db      *sql.DB
	queries *sqlc.Queries
}

func New() Service {
	if err := ensureDatabaseExists(); err != nil {
		log.Fatalf("failed to ensure database exists: %v", err)
	}

	connStr := fmt.Sprintf(
		"postgres://%s:%s@%s:%s/%s?sslmode=disable&search_path=%s",
		util.GetEnv("DB_USERNAME", "postgres"),
		util.GetEnv("DB_PASSWORD", "password1234"),
		util.GetEnv("DB_HOST", "localhost"),
		util.GetEnv("DB_PORT", "5432"),
		util.GetDBName(),
		util.GetEnv("DB_SCHEMA", "public"),
	)

	db, err := sql.Open("pgx", connStr)
	if err != nil {
		log.Fatal(err)
	}
	if err := db.Ping(); err != nil {
		log.Fatal("cannot connect to database: ", err)
	}

	db.SetMaxOpenConns(50)
	db.SetMaxIdleConns(10)
	db.SetConnMaxLifetime(time.Hour)

	if err := RunMigrations(db); err != nil {
		log.Fatalf("failed to run migrations: %v", err)
	}

	return &service{
		db:      db,
		queries: sqlc.New(db),
	}
}
func (s *service) Close() error {
	return s.db.Close()
}

func (s *service) Queries() *sqlc.Queries {
	return s.queries
}

func ensureDatabaseExists() error {
	adminDSN := fmt.Sprintf(
		"postgres://%s:%s@%s:%s/postgres?sslmode=disable",
		util.GetEnv("DB_USERNAME", "postgres"),
		util.GetEnv("DB_PASSWORD", "password1234"),
		util.GetEnv("DB_HOST", "localhost"),
		util.GetEnv("DB_PORT", "5432"),
	)

	dbName := util.GetDBName()

	db, err := sql.Open("pgx", adminDSN)
	if err != nil {
		return err
	}
	defer db.Close()

	var exists bool
	err = db.QueryRow("SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = $1)", dbName).Scan(&exists)
	if err != nil {
		return fmt.Errorf("failed to check if database exists: %w", err)
	}

	if !exists {
		_, err = db.Exec("CREATE DATABASE " + dbName)
		if err != nil {
			return fmt.Errorf("failed to create database: %w", err)
		}
		log.Printf("Database %s created", dbName)
	}

	return nil
}
