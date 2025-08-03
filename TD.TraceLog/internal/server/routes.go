package server

import (
	"github.com/filipcvejic/changes-history/internal/api/auth"
	"github.com/filipcvejic/changes-history/internal/api/handler"
	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	"github.com/go-chi/cors"
	"net/http"
)

func (s *Server) RegisterRoutes() http.Handler {
	r := chi.NewRouter()
	r.Use(middleware.Logger)

	r.Use(cors.Handler(cors.Options{
		AllowedOrigins:   []string{"https://*", "http://*"},
		AllowedMethods:   []string{"GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH"},
		AllowedHeaders:   []string{"Accept", "Authorization", "Content-Type"},
		AllowCredentials: true,
		MaxAge:           300,
	}))

	logHandler := handler.NewLogHandler(s.db)

	r.Group(func(r chi.Router) {
		r.Use(auth.APIKeyMiddleware(s.keyStore))

		r.Post("/history", logHandler.Create)
		r.Get("/history/{hash}", logHandler.List)
	})

	return r
}
