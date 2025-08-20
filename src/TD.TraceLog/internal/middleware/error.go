package middleware

import (
	"errors"
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/go-playground/validator/v10"
	"net/http"
)

func ValidationErrorToText(e validator.FieldError) string {
	switch e.Tag() {
	case "required":
		return fmt.Sprintf("%s is required", e.Field())
	case "max":
		return fmt.Sprintf("%s cannot be longer than %s", e.Field(), e.Param())
	case "min":
		return fmt.Sprintf("%s must be longer than %s", e.Field(), e.Param())
	case "email":
		return fmt.Sprintf("Invalid email format")
	case "len":
		return fmt.Sprintf("%s must be %s characters long", e.Field(), e.Param())
	}
	return fmt.Sprintf("%s is not valid", e.Field())
}

func ErrorHandler() gin.HandlerFunc {
	return func(c *gin.Context) {
		c.Next()

		if len(c.Errors) > 0 {
			for _, e := range c.Errors {
				switch e.Type {
				case gin.ErrorTypePublic:
					if !c.Writer.Written() {
						c.JSON(c.Writer.Status(), gin.H{"error": e.Error()})
					}
				case gin.ErrorTypeBind:
					var errs validator.ValidationErrors
					if errors.As(e.Err, &errs) {
						list := make(map[string]string)
						for _, err := range errs {
							list[err.Field()] = ValidationErrorToText(err)
						}

						status := http.StatusBadRequest
						if c.Writer.Status() != http.StatusOK {
							status = c.Writer.Status()
						}
						c.JSON(status, gin.H{"errors": list})
					} else {
						c.JSON(http.StatusBadRequest, gin.H{"error": "invalid request format"})
					}
				}
			}
		}
	}
}
