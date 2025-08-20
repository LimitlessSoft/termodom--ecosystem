package middleware

import (
	"gin-trace-logs/internal/constants"
	"gin-trace-logs/internal/util"
	"gin-trace-logs/internal/vault"
	"net/http"
	"slices"

	"github.com/gin-gonic/gin"
)

func UseAuth(r *gin.Engine) {
	r.Use(func(c *gin.Context) {
		if !isAuthenticated(c) {
			c.AbortWithStatus(http.StatusUnauthorized)
			return
		}

		c.Next()
	})
}

func isAuthenticated(c *gin.Context) bool {
	authHeader := c.GetHeader(constants.Headers.ApiKey)
	if authHeader == "" {
		return false
	}

	valid := slices.Contains(vault.Secrets.ApiKeys, authHeader)

	if valid {
		c.Set(constants.Context.AppId, util.HashString(authHeader))
	}

	return valid
}
