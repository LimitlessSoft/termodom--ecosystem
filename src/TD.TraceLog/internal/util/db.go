package util

import (
	"fmt"
)

func GetDBName() string {
	env := GetEnv("APP_ENV", "develop")
	base := GetEnv("DB_NAME", "trace_logs")

	return fmt.Sprintf("%s_%s", env, base)
}
