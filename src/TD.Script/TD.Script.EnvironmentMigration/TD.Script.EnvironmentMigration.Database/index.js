import { exec } from 'child_process'
import util from 'util'
import pg from 'pg'

const { Pool } = pg

// Used to execute shell commands as promises
const execPromise = util.promisify(exec)

// Extract environment variables
const {
    APPLICATION,
    SOURCE_ENV,
    TARGET_ENV,
    PGHOST,
    PGPORT,
    PGUSER,
    PGPASSWORD,
} = process.env

// Validate required environment variables
if (
    !APPLICATION ||
    !SOURCE_ENV ||
    !TARGET_ENV ||
    !PGHOST ||
    !PGPORT ||
    !PGUSER ||
    !PGPASSWORD
) {
    console.error(
        'Missing required arguments. Ensure application, source, and target are set.'
    )
    process.exit(1)
}

// Create a pool to connect to the database
const maintenancePool = new Pool({
    user: PGUSER,
    host: PGHOST,
    password: PGPASSWORD,
    port: PGPORT,
})

// Drop target database if it exists
async function dropTargetDatabase() {
    console.log(``)
    console.log(`====================================`)
    console.log(`Starting drop target database`)
    console.log(``)
    console.log(`Dropping target database ${TARGET_ENV}_${APPLICATION}...`)
    const client = await maintenancePool.connect()

    try {
        const dropQuery = `DROP DATABASE IF EXISTS ${TARGET_ENV}_${APPLICATION};`
        await client.query(dropQuery)

        console.log(
            `Target database ${TARGET_ENV}_${APPLICATION} dropped successfully.`
        )
    } catch (err) {
        console.error('Error dropping target database:', err.message)
        process.exit(1)
    } finally {
        client.release()
    }
    console.log(`====================================`)
    console.log(``)
}

async function disconnectAllActiveUsers() {
    console.log(``)
    console.log(`====================================`)
    console.log(`Starting disconnect all active users`)
    console.log(``)
    console.log(
        `Disconnecting all active users from ${SOURCE_ENV}_${APPLICATION}...`
    )

    const client = await maintenancePool.connect()

    try {
        const disconnectQuery = `SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '${TARGET_ENV}_${APPLICATION}' AND pid <> pg_backend_pid();`
        await client.query(disconnectQuery)

        console.log(
            `All active users disconnected from ${SOURCE_ENV}_${APPLICATION}.`
        )
    } catch (err) {
        console.error('Error disconnecting active users:', err.message)
    } finally {
        client.release()
    }
    console.log(`====================================`)
    console.log(``)
}

// Create target database
async function createTargetDatabase() {
    console.log(``)
    console.log(`====================================`)
    console.log('Starting create target database')
    console.log(``)
    console.log(`Creating new target database ${TARGET_ENV}_${APPLICATION}...`)

    const client = await maintenancePool.connect()

    try {
        const createQuery = `CREATE DATABASE ${TARGET_ENV}_${APPLICATION};`
        await client.query(createQuery)

        console.log(
            `Target database ${TARGET_ENV}_${APPLICATION} created successfully.`
        )
    } catch (err) {
        console.error('Error creating target database:', err.message)
    } finally {
        client.release()
    }
    console.log(`====================================`)
    console.log(``)
}

// Migrate data from source to target database
async function migrateData() {
    console.log(``)
    console.log(`====================================`)
    console.log('Starting migrate data')
    console.log(``)
    console.log(
        `Copying data from ${SOURCE_ENV}_${APPLICATION} to ${TARGET_ENV}_${APPLICATION}...`
    )

    try {
        console.log(`Dumping data from source database...`)
        // Creates dump from source database to file
        await execPromise(
            `pg_dump -U postgres -h ${PGHOST} ${SOURCE_ENV}_${APPLICATION} > /asd.txt`
        )

        console.log(`Restoring data to target database...`)
        // Restores dump to target database
        await execPromise(
            `psql -U postgres -h ${PGHOST} ${TARGET_ENV}_${APPLICATION} < /asd.txt`
        )

        console.log(`Data migration completed successfully.`)
    } catch (err) {
        console.error('Error migrating data:', err.message)
    }
    console.log(`====================================`)
    console.log(``)
}

async function runMigration() {
    try {
        await disconnectAllActiveUsers()

        await dropTargetDatabase()

        await createTargetDatabase()

        await migrateData()

        console.log(
            `Database migration from ${SOURCE_ENV} to ${TARGET_ENV} completed successfully.`
        )
    } catch (err) {
        console.error(`Migration process failed: ${err.message}`)
    }
}

await runMigration()
