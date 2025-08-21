package handler

import (
	"errors"
	"gin-trace-logs/internal/api/model"
	"gin-trace-logs/internal/constants"
	"gin-trace-logs/internal/db/sqlc"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

type LogHandler struct {
	queries *sqlc.Queries
}

func NewLogHandler(q *sqlc.Queries) *LogHandler { return &LogHandler{queries: q} }

func (h *LogHandler) CreateLog(c *gin.Context) {
	appId := c.MustGet(constants.Context.AppId).(string)

	var req model.RequestCreateLog
	if err := c.ShouldBindJSON(&req); err != nil {
		_ = c.Error(err).SetType(gin.ErrorTypeBind)
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
	appId := c.MustGet(constants.Context.AppId).(string)

	entityType := c.Query("entity_type")
	if entityType == "" {
		_ = c.Error(errors.New("missing entity_type")).SetType(gin.ErrorTypeBind)
		return
	}

	entityId := c.Query("entity_id")
	if entityId == "" {
		_ = c.Error(errors.New("missing entity_id")).SetType(gin.ErrorTypeBind)
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
