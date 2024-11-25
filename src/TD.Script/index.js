import { exec } from 'child_process'
import util from 'util'
import pg from 'pg'

const { Pool } = pg

const execPromise = util.promisify(exec)

const {
    APPLICATION,
    SOURCE_ENV,
    TARGET_ENV,
    PG_HOST,
    PG_PORT,
    PG_USER,
    PG_PASSWORD,
} = process.env

if (!APPLICATION || !SOURCE_ENV || !TARGET_ENV) {
    console.error(
        'Missing required arguments. Ensure application, source, and target are set.'
    )
    process.exit(1)
}

const maintenancePool = new Pool({
    user: PG_USER,
    host: PG_HOST,
    password: PG_PASSWORD,
    port: PG_PORT,
})

async function dropTargetDatabase() {
    const client = await maintenancePool.connect()

    try {
        //todo disconnect all active connections

        console.log(`Dropping target database ${TARGET_ENV}_${APPLICATION}...`)

        const dropQuery = `DROP DATABASE IF EXISTS ${TARGET_ENV}_${APPLICATION};`
        await client.query(dropQuery)

        console.log(
            `Target database ${TARGET_ENV}_${APPLICATION} dropped successfully.`
        )
    } catch (err) {
        // if (err.code === '3D000') {
        //     console.info("Target database doesn't exist. Skipping droping")
        //     return
        // }
        console.error('Error dropping target database:', err.message)
    } finally {
        client.release()
    }
}

async function createTargetDatabase() {
    console.log('Starting create target database')

    const client = await maintenancePool.connect()

    try {
        console.log(
            `Creating new target database ${TARGET_ENV}_${APPLICATION}...`
        )

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
}

async function migrateData() {
    try {
        console.log(
            `Copying data from ${SOURCE_ENV}_${APPLICATION} to ${TARGET_ENV}_${APPLICATION}...`
        )

        const query = `pg_dump -U ${PG_USER} -h ${PG_HOST} -p ${PG_PORT} ${SOURCE_ENV}_${APPLICATION} | PGPASSWORD="${PG_PASSWORD}" psql -U ${PG_USER} -h ${PG_HOST} -p ${PG_PORT} ${TARGET_ENV}_${APPLICATION}`

        console.log(query)

        await execPromise(query)
        console.log(`Data migration completed successfully.`)
    } catch (err) {
        console.error('Error migrating data:', err.message)
    }
}

async function runMigration() {
    try {
        await dropTargetDatabase()

        await createTargetDatabase()

        // await migrateData()

        console.log(
            `Migration completed successfully for application: ${APPLICATION}`
        )
    } catch (err) {
        console.error(`Migration process failed: ${err.message}`)
    }
}

runMigration()
