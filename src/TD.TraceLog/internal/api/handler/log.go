package handler

import (
	"encoding/json"
	"github.com/filipcvejic/trace-logs/internal/api/model"
	"github.com/filipcvejic/trace-logs/internal/db"
	"github.com/filipcvejic/trace-logs/internal/db/sqlc"
	"github.com/filipcvejic/trace-logs/internal/middleware/auth"
	"github.com/filipcvejic/trace-logs/internal/util"

	"github.com/go-playground/validator/v10"
	"log"
	"net/http"
)

type LogHandler struct {
	db        db.Service
	validator *validator.Validate
}

func NewLogHandler(db db.Service) *LogHandler {
	return &LogHandler{db: db, validator: validator.New()}
}

func (h *LogHandler) CreateLog(w http.ResponseWriter, r *http.Request) {
	apiKey, ok := auth.GetAPIKey(r)
	if !ok {
		util.WriteError(w, http.StatusUnauthorized, "Unauthorized")
		return
	}

	var req model.RequestCreateLog
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		log.Printf("CreateLog - JSON decode error: %v", err)
		util.WriteError(w, http.StatusBadRequest, "invalid JSON payload")
		return
	}

	if err := h.validator.Struct(req); err != nil {
		util.WriteValidationErrors(w, err)
		return
	}

	params := sqlc.CreateLogParams{
		EntityID:   req.EntityID,
		EntityType: req.EntityType,
		ActionType: req.ActionType,
		New:        req.New,
		Old:        req.Old,
		LoggedBy:   req.LoggedBy,
		CreatedBy:  util.HashString(apiKey),
	}

	logEntry, err := h.db.Queries().CreateLog(r.Context(), params)
	if err != nil {
		log.Printf("CreateLog - DB error: %v", err)
		util.WriteError(w, http.StatusInternalServerError, err.Error())
		return
	}

	util.WriteJSON(w, http.StatusCreated, logEntry)
}

func (h *LogHandler) ListLogs(w http.ResponseWriter, r *http.Request) {
	apiKey, ok := auth.GetAPIKey(r)
	if !ok {
		util.WriteError(w, http.StatusUnauthorized, "Unauthorized")
		return
	}

	entityType := r.URL.Query().Get("entity_type")
	if entityType == "" {
		log.Println("ListLogs - missing entity_type parameter")
		util.WriteError(w, http.StatusBadRequest, "missing required query parameter: entity_type")
		return
	}

	entityID := r.URL.Query().Get("entity_id")
	if entityID == "" {
		log.Println("ListLogs - missing entity_id parameter")
		util.WriteError(w, http.StatusBadRequest, "missing required query parameter: entity_id")
		return
	}

	params := sqlc.ListLogsParams{
		EntityID:   entityID,
		EntityType: entityType,
		CreatedBy:  util.HashString(apiKey),
	}

	logs, err := h.db.Queries().ListLogs(r.Context(), params)
	if err != nil {
		log.Printf("ListLogs - DB error: %v", err)
		util.WriteError(w, http.StatusInternalServerError, err.Error())
		return
	}

	util.WriteJSON(w, http.StatusOK, logs)
}
