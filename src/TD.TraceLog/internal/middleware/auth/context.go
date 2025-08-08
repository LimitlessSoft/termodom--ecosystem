package auth

import (
	"net/http"
)

type contextKey string

const apiKeyContextKey contextKey = "apiKey"

func GetAPIKey(r *http.Request) (string, bool) {
	apiKey, ok := r.Context().Value(apiKeyContextKey).(string)
	return apiKey, ok
}
