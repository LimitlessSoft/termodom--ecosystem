package middleware

import (
	"gin-trace-logs/internal/constants"

	"github.com/gin-contrib/cors"
	"github.com/gin-gonic/gin"
)

func UseCors(r *gin.Engine) {
	corsConfig := cors.DefaultConfig()
	corsConfig.AllowAllOrigins = true
	corsConfig.AddAllowHeaders("Accept", constants.Headers.ApiKey)
	r.Use(cors.New(corsConfig))
}
