package handler

import (
	"errors"
	"gin-trace-logs/internal/api/model"
	"gin-trace-logs/internal/db/sqlc"
	"github.com/gin-gonic/gin"
	"log"
	"net/http"
)

type LogHandler struct {
	queries *sqlc.Queries
}

func NewLogHandler(q *sqlc.Queries) *LogHandler {
	return &LogHandler{queries: q}
}

func (h *LogHandler) CreateLog(c *gin.Context) {
	appId := c.MustGet("appId").(string)

	var req model.RequestCreateLog
	if err := c.ShouldBindJSON(&req); err != nil {
		c.Error(err).SetType(gin.ErrorTypeBind)
		return
	}

	params := sqlc.CreateLogParams{
		EntityID:   req.EntityId,
		EntityType: req.EntityType,
		ActionType: req.ActionType,
		New:        req.New,
		Old:        req.Old,
		LoggedBy:   req.LoggedBy,
		CreatedBy:  appId,
	}

	logEntry, err := h.queries.CreateLog(c.Request.Context(), params)
	if err != nil {
		log.Println("CreateLog failed: ", err)
		c.Status(http.StatusInternalServerError)
		return
	}

	c.JSON(http.StatusCreated, logEntry)
}

func (h *LogHandler) ListLogs(c *gin.Context) {
	appId := c.MustGet("appId").(string)

	entityType := c.Query("entity_type")
	if entityType == "" {
		c.Error(errors.New("missing entity_type")).SetType(gin.ErrorTypePublic)
		c.Status(http.StatusBadRequest)
		return
	}

	entityId := c.Query("entity_id")
	if entityId == "" {
		c.Error(errors.New("missing entity_id")).SetType(gin.ErrorTypePublic)
		c.Status(http.StatusBadRequest)
		return
	}

	params := sqlc.ListLogsParams{
		EntityID:   entityId,
		EntityType: entityType,
		CreatedBy:  appId,
	}

	logs, err := h.queries.ListLogs(c.Request.Context(), params)
	if err != nil {
		log.Println("ListLogs failed: ", err)
		c.Status(http.StatusInternalServerError)
		return
	}

	c.JSON(http.StatusOK, logs)
}
