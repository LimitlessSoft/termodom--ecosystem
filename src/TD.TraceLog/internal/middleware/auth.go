package middleware

import (
	"errors"
	"gin-trace-logs/internal/util"
	"gin-trace-logs/internal/vault"
	"github.com/gin-gonic/gin"
	"net/http"
	"slices"
)

func AuthenticationMiddleware() gin.HandlerFunc {
	return func(c *gin.Context) {
		if !isAuthenticated(c) {
			c.AbortWithError(http.StatusUnauthorized, errors.New("unauthorized")).SetType(gin.ErrorTypePublic)
			return
		}

		c.Next()
	}
}

func isAuthenticated(c *gin.Context) bool {
	authHeader := c.GetHeader("X-API-Key")
	if authHeader == "" {
		return false
	}

	valid := slices.Contains(vault.Secrets.ApiKeys, authHeader)

	if valid {
		c.Set("appId", util.HashString(authHeader))
	}

	return valid
}
