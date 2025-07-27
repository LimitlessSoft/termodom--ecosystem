package handler

import (
	"changes-history/internal/db"
	"changes-history/internal/db/sqlc"
	"encoding/json"
	"fmt"
	"github.com/go-chi/chi/v5"
	"log"
	"net/http"
	"strconv"
	"time"
)

type LogHandler struct {
	db db.Service
}

func NewLogHandler(db db.Service) *LogHandler {
	return &LogHandler{db: db}
}

func (h *LogHandler) Create(w http.ResponseWriter, r *http.Request) {
	var body struct {
		EntityId   int64  `json:"entity_id"`
		EntityType string `json:"entity_type"`
		ActionType string `json:"action_type"`
		New        string `json:"new"`
		Old        string `json:"old"`
		CreatedBy  int32  `json:"created_by"`
	}

	if err := json.NewDecoder(r.Body).Decode(&body); err != nil {
		http.Error(w, "Invalid request body", http.StatusBadRequest)
		return
	}

	if body.EntityType == "" || body.ActionType == "" || body.New == "" || body.Old == "" {
		http.Error(w, "Missing required fields", http.StatusBadRequest)
		return
	}

	params := sqlc.CreateLogParams{
		EntityID:   body.EntityId,
		EntityType: body.EntityType,
		ActionType: body.ActionType,
		New:        json.RawMessage(body.New),
		Old:        json.RawMessage(body.Old),
		CreatedBy:  body.CreatedBy,
		CreatedAt:  time.Now(),
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

	hash, err := strconv.ParseInt(hashStr, 10, 64)
	if err != nil {
		http.Error(w, "Invalid hash format", http.StatusBadRequest)
		return
	}

	params := sqlc.ListLogsParams{
		EntityID:   hash,
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
