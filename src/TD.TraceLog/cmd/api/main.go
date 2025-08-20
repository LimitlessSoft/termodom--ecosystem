package main

import (
	"context"
	"gin-trace-logs/internal/api/handler"
	"gin-trace-logs/internal/db"
	"gin-trace-logs/internal/middleware"
	"github.com/gin-contrib/cors"
	"github.com/gin-gonic/gin"
	_ "github.com/joho/godotenv/autoload"
	"log"
)

func main() {
	ctx := context.Background()

	//if err := db.EnsureDatabase(ctx); err != nil {
	//	log.Fatal(err)
	//}

	dbInstance, err := db.New(ctx)
	if err != nil {
		log.Fatal(err)
	}
	defer dbInstance.Pool.Close()

	r := gin.Default()
	corsConfig := cors.DefaultConfig()
	corsConfig.AllowAllOrigins = true
	corsConfig.AddAllowHeaders("Accept", "X-API-Key")

	r.Use(cors.New(corsConfig), middleware.ErrorHandler(), middleware.AuthenticationMiddleware())

	logsHandler := handler.NewLogHandler(dbInstance.Queries)

	logsRoutes := r.Group("/trace-logs")
	logsRoutes.GET("", logsHandler.ListLogs)
	logsRoutes.POST("", logsHandler.CreateLog)

	r.Run(":8080")
}
