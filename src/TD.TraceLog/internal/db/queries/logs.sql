-- name: ListLogs :many
SELECT * FROM logs
WHERE created_by = $1 AND entity_id = $2 AND entity_type = $3
ORDER BY created_at DESC;

-- name: CreateLog :one
INSERT INTO logs (
    entity_id, entity_type, action_type, new, old,logged_by, created_by
) VALUES (
    $1, $2, $3, $4, $5, $6, $7
) RETURNING *;