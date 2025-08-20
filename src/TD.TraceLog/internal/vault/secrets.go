package vault

import (
	"context"
	"gin-trace-logs/internal/constants"
	"log"
	"os"

	"github.com/mitchellh/mapstructure"
)

var Secrets struct {
	ApiKeys    []string `mapstructure:"API_KEYS"`
	DbHost     string   `mapstructure:"DB_HOST"`
	DbName     string   `mapstructure:"DB_NAME"`
	DbPassword string   `mapstructure:"DB_PASSWORD"`
	DbPort     string   `mapstructure:"DB_PORT"`
	DbSchema   string   `mapstructure:"DB_SCHEMA"`
	DbUsername string   `mapstructure:"DB_USERNAME"`
}

func init() {
	client, err := NewClient()
	if err != nil {
		log.Fatal(err)
	}

	kv := client.KVv2(os.Getenv(constants.Env.VaultEngine))

	secret, err := kv.Get(context.Background(), os.Getenv(constants.Env.VaultPath))
	if err != nil {
		log.Fatal(err)
	}

	err = mapstructure.Decode(secret.Data, &Secrets)
	if err != nil {
		log.Fatal("Error decoding secrets:", err)
	}
}
