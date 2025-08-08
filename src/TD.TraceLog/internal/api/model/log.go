package model

type RequestCreateLog struct {
	EntityID   string `json:"entity_id" validate:"required"`
	EntityType string `json:"entity_type" validate:"required"`
	ActionType string `json:"action_type" validate:"required"`
	New        string `json:"new" validate:"required"`
	Old        string `json:"old" validate:"required"`
	LoggedBy   string `json:"logged_by" validate:"required"`
}
