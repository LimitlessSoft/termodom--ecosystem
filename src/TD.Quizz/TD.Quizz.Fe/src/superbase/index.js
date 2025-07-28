import { createClient } from '@supabase/supabase-js'

const projectUrl = "https://bziuicjjwweilryhhovs.supabase.co"
const key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJ6aXVpY2pqd3dlaWxyeWhob3ZzIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc1MzUyNTc3NywiZXhwIjoyMDY5MTAxNzc3fQ.gUPjb3P9cwBR2DdjVl2FZNHIKd09INSYMFLh2pCnzvY"
const superbaseClient = createClient(projectUrl, key)
export const superbaseSchema = superbaseClient.schema("public")