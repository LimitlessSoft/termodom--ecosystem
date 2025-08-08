package auth

import (
	"bytes"
	"encoding/json"
	"fmt"
	"net/http"
	"slices"
)

type VaultLoginResponse struct {
	Auth struct {
		ClientToken string `json:"client_token"`
	} `json:"auth"`
}

type VaultAPIKeysResponse struct {
	Data struct {
		Data struct {
			APIKeys []string `json:"API_KEYS"`
		} `json:"data"`
	} `json:"data"`
}

type APIKeyStore struct {
	keys []string
}

func NewAPIKeyStore(keys []string) *APIKeyStore {
	return &APIKeyStore{keys: keys}
}

func (s *APIKeyStore) Contains(key string) bool {
	return slices.Contains(s.keys, key)
}

func VaultLogin(url, username, password string) (string, error) {
	if username == "" || password == "" {
		return "", fmt.Errorf("missing VAULT_USERNAME or VAULT_PASSWORD env variables")
	}
	payload := map[string]string{"password": password}
	jsonData, err := json.Marshal(payload)
	if err != nil {
		return "", err
	}

	resp, err := http.Post(url, "application/json", bytes.NewBuffer(jsonData))
	if err != nil {
		return "", err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return "", fmt.Errorf("vault login failed: status %d", resp.StatusCode)
	}

	var loginResp VaultLoginResponse
	if err := json.NewDecoder(resp.Body).Decode(&loginResp); err != nil {
		return "", err
	}

	return loginResp.Auth.ClientToken, nil
}

func FetchAPIKeys(url, token string) ([]string, error) {
	req, err := http.NewRequest("GET", url, nil)
	if err != nil {
		return nil, err
	}
	req.Header.Set("X-Vault-Token", token)

	resp, err := http.DefaultClient.Do(req)
	if err != nil {
		return nil, err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return nil, fmt.Errorf("failed to fetch keys, status: %d", resp.StatusCode)
	}

	var keysResp VaultAPIKeysResponse
	if err := json.NewDecoder(resp.Body).Decode(&keysResp); err != nil {
		return nil, err
	}

	return keysResp.Data.Data.APIKeys, nil
}
