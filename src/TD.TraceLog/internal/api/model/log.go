package model

type RequestCreateLog struct {
	EntityId   string `json:"entity_id" binding:"required"`
	EntityType string `json:"entity_type" binding:"required"`
	ActionType string `json:"action_type" binding:"required"`
	New        string `json:"new" binding:"required"`
	Old        string `json:"old" binding:"required"`
	LoggedBy   string `json:"logged_by" binding:"required"`
}
