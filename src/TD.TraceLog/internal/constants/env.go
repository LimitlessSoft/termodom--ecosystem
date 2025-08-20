package constants

type env struct {
	App           string // Environment variable for the application environment
	VaultAddress  string // Environment variable for the vault address
	VaultUsername string // Environment variable for the vault username
	VaultPassword string // Environment variable for the vault password
	VaultEngine   string // Environment variable for the vault engine
	VaultPath     string // Environment variable for the vault path
}

var Env = env{
	App:           "APP_ENV",
	VaultAddress:  "VAULT_ADDR",
	VaultUsername: "VAULT_USERNAME",
	VaultPassword: "VAULT_PASSWORD",
	VaultEngine:   "VAULT_ENV",
	VaultPath:     "VAULT_PATH",
}
