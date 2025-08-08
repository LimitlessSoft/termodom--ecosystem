package api

import (
	"github.com/filipcvejic/trace-logs/internal/api/handler"
	"github.com/filipcvejic/trace-logs/internal/middleware/auth"
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

	r.Route("/history", func(h chi.Router) {
		h.Group(func(r chi.Router) {
			r.Use(auth.APIKeyMiddleware(s.keyStore))
			r.Post("/", logHandler.CreateLog)
			r.Get("/", logHandler.ListLogs)
		})
	})

	return r
}
