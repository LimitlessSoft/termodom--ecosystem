-- +goose Up
-- +goose StatementBegin
ALTER TABLE logs ALTER COLUMN "new" SET DATA TYPE TEXT USING "new"::TEXT;
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
ALTER TABLE logs ALTER COLUMN "new" SET DATA TYPE JSONB USING "new"::JSONB;
-- +goose StatementEnd
