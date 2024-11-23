#!/usr/bin/env node

import { Command } from 'commander'
import chalk from 'chalk'
import { exec } from 'child_process'
import fs from 'fs'
import path from 'path'

const DB_USER = 'postgres'
const DB_HOST = '139.177.181.216'
const DB_PASS = 'FFnF2JegHu0pt6RmBr5ib2mxRIuaCwNe'
const DB_PORT = 5432

const dbConfig = {
    office: {
        production: {
            user: DB_USER,
            password: DB_PASS,
            host: DB_HOST,
            port: DB_PORT,
            database: 'release_tdoffice',
        },
        develop: {
            user: DB_USER,
            password: DB_PASS,
            host: DB_HOST,
            port: DB_PORT,
            database: 'develop_tdoffice',
        },
    },
    web: {
        production: {
            user: DB_USER,
            password: DB_PASS,
            host: DB_HOST,
            port: DB_PORT,
            database: 'release_web',
        },
        develop: {
            user: DB_USER,
            password: DB_PASS,
            host: DB_HOST,
            port: DB_PORT,
            database: 'develop_web',
        },
    },
}

const runCommand = (command) =>
    new Promise((resolve, reject) => {
        exec(command, (error, stdout, stderr) => {
            if (error) {
                console.error(chalk.red(`Error executing command: ${command}`))
                console.error(chalk.red(`stderr: ${stderr}`))
                reject(stderr || error.message)
            } else {
                console.log(chalk.green(`stdout: ${stdout}`))
                resolve(stdout)
            }
        })
    })

const migrateDatabase = async (app, sourceEnv, targetEnv) => {
    try {
        if (!dbConfig[app]) throw new Error(`Invalid application: ${app}`)
        if (!dbConfig[app][sourceEnv])
            throw new Error(`Invalid source environment: ${sourceEnv}`)
        if (!dbConfig[app][targetEnv])
            throw new Error(`Invalid target environment: ${targetEnv}`)
        if (sourceEnv === targetEnv)
            throw new Error('Source and target environments must be different!')

        const source = dbConfig[app][sourceEnv]
        const target = dbConfig[app][targetEnv]

        console.log(
            chalk.blue(
                `Starting migration for ${app} from ${sourceEnv} to ${targetEnv}...`
            )
        )

        // Step 1: Delete the target database (if it exists)
        const dropDatabaseCommand =
            process.platform === 'win32'
                ? `set PGPASSWORD=${target.password} && psql -U ${target.user} -h ${target.host} -p ${target.port} -c "DROP DATABASE IF EXISTS ${target.database};"`
                : `PGPASSWORD="${target.password}" psql -U ${target.user} -h ${target.host} -p ${target.port} -c "DROP DATABASE IF EXISTS ${target.database};"`
        console.log(chalk.red(`Dropping target database for ${app}...`))
        await runCommand(dropDatabaseCommand)

        // Step 2: Backup the source database
        const dumpFile = path.resolve(__dirname, `${app}_${sourceEnv}_dump.sql`)
        const dumpCommand =
            process.platform === 'win32'
                ? `set PGPASSWORD=${source.password} && pg_dump -U ${source.user} -h ${source.host} -p ${source.port} -d ${source.database} -F c -b -v -f ${dumpFile}`
                : `PGPASSWORD="${source.password}" pg_dump -U ${source.user} -h ${source.host} -p ${source.port} -d ${source.database} -F c -b -v -f ${dumpFile}`
        console.log(chalk.green(`Exporting source database for ${app}...`))
        await runCommand(dumpCommand)

        // Step 3: Create a fresh target database
        const createDatabaseCommand =
            process.platform === 'win32'
                ? `set PGPASSWORD=${target.password} && psql -U ${target.user} -h ${target.host} -p ${target.port} -c "CREATE DATABASE ${target.database};"`
                : `PGPASSWORD="${target.password}" psql -U ${target.user} -h ${target.host} -p ${target.port} -c "CREATE DATABASE ${target.database};"`
        console.log(chalk.green(`Creating target database for ${app}...`))
        await runCommand(createDatabaseCommand)

        // Step 4: Restore the backup into the target database
        const restoreCommand =
            process.platform === 'win32'
                ? `set PGPASSWORD=${target.password} && pg_restore -U ${target.user} -h ${target.host} -p ${target.port} -d ${target.database} -c -v ${dumpFile}`
                : `PGPASSWORD="${target.password}" pg_restore -U ${target.user} -h ${target.host} -p ${target.port} -d ${target.database} -c -v ${dumpFile}`
        console.log(
            chalk.green(`Importing backup into target database for ${app}...`)
        )
        await runCommand(restoreCommand)

        // Step 5: Clean up dump file
        console.log(chalk.yellow('Cleaning up...'))
        fs.unlinkSync(dumpFile)

        console.log(chalk.green(`Migration for ${app} completed successfully!`))
    } catch (error) {
        console.error(
            chalk.red(`Migration failed for ${app}: ${error.message}`)
        )
    }
}

// Initialize the CLI with Commander
const program = new Command()

program
    .version('1.0.0')
    .description('CLI to migrate PostgreSQL databases for applications')
    .option(
        '--application <app>',
        'Specify the application(s)',
        (val, prev) => prev.concat(val),
        []
    )
    .requiredOption(
        '--source_env <env>',
        'Source environment (e.g., production, develop)'
    )
    .requiredOption(
        '--target_env <env>',
        'Target environment (e.g., production, develop)'
    )

program.parse(process.argv)

// Extract parsed arguments
const {
    application,
    source_env: sourceEnv,
    target_env: targetEnv,
} = program.opts()

// Execute the migration for each application
const main = async () => {
    if (application.length === 0) {
        console.error(
            chalk.red(
                'You must specify at least one application using --application'
            )
        )
        process.exit(1)
    }

    for (const app of application) {
        console.log(chalk.cyan(`\nStarting migration for application: ${app}`))
        await migrateDatabase(app, sourceEnv, targetEnv)
    }
}

main()
