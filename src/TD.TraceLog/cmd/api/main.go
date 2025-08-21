package main

import (
	"gin-trace-logs/internal/db"
	"gin-trace-logs/internal/middleware"
	"log"

	"github.com/gin-gonic/gin"
	_ "github.com/joho/godotenv/autoload"
)

func main() {
	db.Initialize()
	defer db.Instance.Pool.Close()

	r := gin.Default()
	middleware.UseCors(r)
	middleware.UseError(r)
	middleware.UseAuth(r)
	middleware.UseLogsRoute(r)

	if err := r.Run(":8080"); err != nil {
		log.Fatal("Failed to start server: ", err)
	}
}
