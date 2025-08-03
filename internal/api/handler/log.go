package handler

import (
	"encoding/json"
	"fmt"
	"github.com/filipcvejic/changes-history/internal/db"
	"github.com/filipcvejic/changes-history/internal/db/sqlc"
	"github.com/go-chi/chi/v5"
	"github.com/go-playground/validator/v10"
	"log"
	"net/http"
	"strconv"
)

type LogHandler struct {
	db        db.Service
	Validator *validator.Validate
}

func NewLogHandler(db db.Service) *LogHandler {
	return &LogHandler{db: db}
}

func (h *LogHandler) Create(w http.ResponseWriter, r *http.Request) {
	var body struct {
		EntityId   int32  `json:"entity_id" validate:"required"`
		EntityType string `json:"entity_type" validate:"required"`
		ActionType string `json:"action_type" validate:"required"`
		New        string `json:"new" validate:"required"`
		Old        string `json:"old" validate:"required"`
		LoggedBy   string `json:"logged_by" validate:"required"`
		CreatedBy  int32  `json:"created_by" validate:"required"`
	}

	if err := json.NewDecoder(r.Body).Decode(&body); err != nil {
		log.Printf("error decoding body: %v", err)
		http.Error(w, "Invalid request body", http.StatusBadRequest)
		return
	}

	if err := h.Validator.Struct(body); err != nil {
		http.Error(w, "Validation failed", http.StatusBadRequest)
		return
	}

	params := sqlc.CreateLogParams{
		EntityID:   body.EntityId,
		EntityType: body.EntityType,
		ActionType: body.ActionType,
		New:        body.New,
		Old:        body.Old,
		LoggedBy:   body.LoggedBy,
		CreatedBy:  body.CreatedBy,
	}

	logEntry, err := h.db.Queries().CreateLog(r.Context(), params)
	if err != nil {
		http.Error(w, "Failed to create log", http.StatusInternalServerError)
		return
	}

	data, err := json.Marshal(logEntry)
	if err != nil {
		log.Printf("error handling JSON marshal: %v", err)
		http.Error(w, "Internal server error", http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	w.Write(data)
}

func (h *LogHandler) List(w http.ResponseWriter, r *http.Request) {
	hashStr := chi.URLParam(r, "hash")
	if hashStr == "" {
		http.Error(w, "Missing hash query parameter", http.StatusBadRequest)
		return
	}

	hash, err := strconv.ParseInt(hashStr, 10, 32)
	if err != nil {
		http.Error(w, "Invalid hash format", http.StatusBadRequest)
		return
	}

	params := sqlc.ListLogsParams{
		EntityID:   int32(hash),
		EntityType: "User",
	}

	logs, err := h.db.Queries().ListLogs(r.Context(), params)
	if err != nil {
		fmt.Printf("error listing logs: %v", err)
		http.Error(w, "Failed to list logs", http.StatusInternalServerError)
		return
	}

	data, err := json.Marshal(logs)
	if err != nil {
		log.Printf("error handling JSON marshal: %v", err)
		http.Error(w, "Internal server error", http.StatusInternalServerError)
		return
	}

	w.Write(data)
}
