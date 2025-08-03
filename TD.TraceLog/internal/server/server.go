package server

import (
	"fmt"
	"github.com/filipcvejic/changes-history/internal/api/auth"
	"github.com/filipcvejic/changes-history/internal/db"
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

var (
	port             = os.Getenv("PORT")
	vaultUrl         = os.Getenv("VAULT_URL")
	vaultUsername    = os.Getenv("VAULT_USERNAME")
	vaultPassword    = os.Getenv("VAULT_PASSWORD")
	vaultEnvironment = os.Getenv("VAULT_ENVIRONMENT")
	vaultEngine      = os.Getenv("VAULT_ENGINE")
)

func NewServer() *http.Server {
	port, _ := strconv.Atoi(port)

	vaultLoginUrl := fmt.Sprintf("%s/v1/auth/userpass/login/%s", vaultUrl, vaultUsername)
	vaultToken, err := auth.VaultLogin(vaultLoginUrl, vaultUsername, vaultPassword)
	if err != nil {
		log.Fatal(err)
	}

	vaultAPIKeysUrl := fmt.Sprintf("%s/v1/%s/data/%s", vaultUrl, vaultEnvironment, vaultEngine)
	apiKeys, err := auth.FetchAPIKeys(vaultAPIKeysUrl, vaultToken)
	if err != nil {
		log.Fatal(err)
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
