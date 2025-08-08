package api

import (
	"fmt"
	"github.com/filipcvejic/trace-logs/internal/db"
	"github.com/filipcvejic/trace-logs/internal/middleware/auth"
	"log"
	"net/http"
	"os"
	"strconv"
	"time"

	_ "github.com/joho/godotenv/autoload"
)

type Server struct {
	port     int
	db       db.Service
	keyStore *auth.APIKeyStore
}

func NewServer() *http.Server {
	portStr := os.Getenv("PORT")
	port, err := strconv.Atoi(portStr)
	if err != nil {
		log.Fatalf("Invalid PORT value: %v", err)
	}

	vaultToken, err := auth.VaultLogin(
		fmt.Sprintf("%s/v1/auth/userpass/login/%s", os.Getenv("VAULT_URL"), os.Getenv("VAULT_USERNAME")),
		os.Getenv("VAULT_USERNAME"),
		os.Getenv("VAULT_PASSWORD"),
	)
	if err != nil {
		log.Fatalf("Vault login failed: %v", err)
	}

	apiKeys, err := auth.FetchAPIKeys(
		fmt.Sprintf("%s/v1/%s/data/%s", os.Getenv("VAULT_URL"), os.Getenv("VAULT_ENVIRONMENT"), "trace-log/api"),
		vaultToken,
	)
	if err != nil {
		log.Fatalf("Fetching API keys failed: %v", err)
	}

	NewServer := &Server{
		port:     port,
		db:       db.New(),
		keyStore: auth.NewAPIKeyStore(apiKeys),
	}

	server := &http.Server{
		Addr:         fmt.Sprintf(":%d", NewServer.port),
		Handler:      NewServer.RegisterRoutes(),
		IdleTimeout:  time.Minute,
		ReadTimeout:  10 * time.Second,
		WriteTimeout: 30 * time.Second,
	}

	return server
}
