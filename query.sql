-- -- name: GetLog :one
-- SELECT * FROM logs
-- WHERE id = $1 LIMIT 1;

-- name: ListLogs :many
SELECT * FROM logs
WHERE entity_id = $1 AND entity_type = $2
ORDER BY created_at DESC;

-- name: CreateLog :one
INSERT INTO logs (
    entity_id, entity_type, action_type, new, old, created_by, created_at
) VALUES (
    $1, $2, $3, $4, $5, $6, $7
) RETURNING *;

-- -- name: UpdateUser :exec
-- UPDATE logs
--     set name = $2,
--     bio = $3
-- WHERE id = $1
-- RETURNING *;

-- -- name: DeleteUser :exec
-- DELETE FROM logs
-- WHERE id = $1;