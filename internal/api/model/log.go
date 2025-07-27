package model

import (
	"encoding/json"
	"github.com/google/uuid"
	"time"
)

type Log struct {
	Id         uuid.UUID       `json:"id"`
	EntityId   int64           `json:"entity_id"`
	EntityType string          `json:"entity_type"`
	ActionType string          `json:"action_type"`
	New        json.RawMessage `json:"new"`
	Old        json.RawMessage `json:"old"`
	CreatedBy  int32           `json:"created_by"`
	CreatedAt  time.Time       `json:"created_at"`
}
