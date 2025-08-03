CREATE TABLE logs (
    id SERIAL PRIMARY KEY,
    entity_id INT NOT NULL,
    entity_type TEXT NOT NULL,
    action_type TEXT NOT NULL,
    new TEXT NOT NULL,
    old TEXT NOT NULL,
    logged_by TEXT NOT NULL,
    created_by INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);
