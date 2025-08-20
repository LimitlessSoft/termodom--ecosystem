package vault

import (
	"context"
	"fmt"
	"github.com/hashicorp/vault/api"
	"github.com/hashicorp/vault/api/auth/userpass"
	"os"
)

func NewClientWithUserpass() (*api.Client, error) {
	config := api.DefaultConfig()
	config.Address = os.Getenv("VAULT_ADDR")

	client, err := api.NewClient(config)
	if err != nil {
		return nil, fmt.Errorf("failed to create Vault client: %w", err)
	}

	username := os.Getenv("VAULT_USERNAME")

	authMethod, err := userpass.NewUserpassAuth(username, &userpass.Password{
		FromEnv: "VAULT_PASSWORD",
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
