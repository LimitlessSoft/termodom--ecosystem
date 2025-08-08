package util

import (
	"encoding/json"
	"errors"
	"github.com/go-playground/validator/v10"
	"net/http"
)

func WriteJSON(w http.ResponseWriter, status int, v any) {
	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(status)
	_ = json.NewEncoder(w).Encode(v)
}

func WriteError(w http.ResponseWriter, status int, msg string) {
	WriteJSON(w, status, map[string]string{"error": msg})
}

func WriteValidationErrors(w http.ResponseWriter, err error) {
	var errs validator.ValidationErrors
	if errors.As(err, &errs) {
		errorList := make([]map[string]string, 0)
		for _, ve := range errs {
			errorList = append(errorList, map[string]string{
				"field":   ve.Field(),
				"message": ve.Error(),
			})
		}

		WriteJSON(w, http.StatusBadRequest, map[string]interface{}{
			"errors": errorList,
		})
	}
}
