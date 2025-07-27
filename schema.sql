CREATE TABLE logs (
    id UUID PRIMARY KEY,
    entity_id BIGINT NOT NULL,
    entity_type TEXT NOT NULL,
    action_type TEXT NOT NULL,
    new JSONB NOT NULL,
    old JSONB NOT NULL,
    created_by INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);
