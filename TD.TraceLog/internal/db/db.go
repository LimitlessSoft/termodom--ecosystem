package db

import (
	"database/sql"
	"fmt"
	"github.com/filipcvejic/changes-history/internal/db/sqlc"
	"log"
	"os"
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

var (
	database   = os.Getenv("BLUEPRINT_DB_DATABASE")
	password   = os.Getenv("BLUEPRINT_DB_PASSWORD")
	username   = os.Getenv("BLUEPRINT_DB_USERNAME")
	port       = os.Getenv("BLUEPRINT_DB_PORT")
	host       = os.Getenv("BLUEPRINT_DB_HOST")
	schema     = os.Getenv("BLUEPRINT_DB_SCHEMA")
	dbInstance *service
)

func New() Service {
	if dbInstance != nil {
		return dbInstance
	}
	connStr := fmt.Sprintf("postgres://%s:%s@%s:%s/%s?sslmode=disable&search_path=%s", username, password, host, port, database, schema)
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

	dbInstance = &service{
		db:      db,
		queries: sqlc.New(db),
	}
	return dbInstance
}

func (s *service) Health() map[string]string {
	//ctx, cancel := context.WithTimeout(context.Background(), 1*time.Second)
	//defer cancel()

	stats := make(map[string]string)
	if err := s.db.Ping(); err != nil {
		stats["status"] = "down"
		stats["error"] = fmt.Sprintf("db down: %v", err)
		log.Printf("db down: %v", err)
		return stats
	}

	stats["status"] = "up"
	stats["message"] = "It's healthy"
	return stats
}

func (s *service) Close() error {
	log.Printf("Disconnected from database: %s", database)
	s.db.Close()
	return nil
}

func (s *service) Queries() *sqlc.Queries {
	return s.queries
}
