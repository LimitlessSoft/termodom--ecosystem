package middleware

import (
	"gin-trace-logs/internal/api/handler"
	"gin-trace-logs/internal/db"

	"github.com/gin-gonic/gin"
)

func UseLogsRoute(r *gin.Engine) {
	logsRoutes := r.Group("/trace-logs")
	logsHandler := handler.NewLogHandler(db.Instance.Queries)
	logsRoutes.GET("", logsHandler.ListLogs)
	logsRoutes.POST("", logsHandler.CreateLog)
}
