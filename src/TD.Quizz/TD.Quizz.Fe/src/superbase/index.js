import { createClient } from '@supabase/supabase-js'

const projectUrl = process.env.SUPERBASE_URL
const key = process.env.SUPERBASE_KEY
const schema = process.env.SUPERBASE_SCHEMA
const superbaseClient = createClient(projectUrl, key)
export const superbaseSchema = superbaseClient.schema(schema)
