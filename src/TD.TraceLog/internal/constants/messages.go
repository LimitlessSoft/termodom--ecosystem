package constants

type messages struct {
	ConnectionPoolCreationError string
	ConnectionPoolPingError     string
}

var Messages = messages{
	ConnectionPoolCreationError: "Failed to initialize database: unable to create connection pool: %w",
	ConnectionPoolPingError:     "Failed to initialize database: unable to ping connection pool: %w",
}
