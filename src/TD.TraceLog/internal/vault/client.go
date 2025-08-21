package vault

import (
	"context"
	"fmt"
	"gin-trace-logs/internal/constants"
	"os"

	"github.com/hashicorp/vault/api"
	"github.com/hashicorp/vault/api/auth/userpass"
)

func NewClient() (*api.Client, error) {
	config := api.DefaultConfig()
	config.Address = os.Getenv(constants.Env.VaultAddress)

	client, err := api.NewClient(config)
	if err != nil {
		return nil, fmt.Errorf("failed to create Vault client: %w", err)
	}

	username := os.Getenv(constants.Env.VaultUsername)

	authMethod, err := userpass.NewUserpassAuth(username, &userpass.Password{
		FromEnv: constants.Env.VaultPassword,
	})
	if err != nil {
		return nil, err
	}

	secret, err := client.Auth().Login(context.Background(), authMethod)
	if err != nil || secret == nil || secret.Auth == nil {
		return nil, err
	}

	client.SetToken(secret.Auth.ClientToken)
	return client, nil
}
