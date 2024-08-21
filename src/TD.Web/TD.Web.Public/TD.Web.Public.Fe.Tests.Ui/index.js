// This file is used to run all the tests in the tests directory
// It reads all the files in the tests directory and runs the default function exported by each file
// The default function is expected to be an async function that runs the test
import fs from 'fs'
import path from 'path'
import { createDriver } from './driver.js'
import { ENV } from './constants.js'

const testsDir = path.resolve('./tests')

let passedTests = 0
let totalTests = 0

fs.readdir(testsDir, async (err, files) => {
    if (err) {
        console.error('Error reading tests directory:', err)
        return
    }
    
    // Use this to run only specific tests when debugging
    // Specify exact file names with the .js extension
    const runOnlyTheseTests = ENV === 'local'
        ? []
        : null

    const testFiles =
        runOnlyTheseTests && runOnlyTheseTests.length > 0
            ? runOnlyTheseTests
            : files.filter(file => file.endsWith('.js'))
    totalTests = testFiles.length

    for (const file of testFiles) {
        const filePath = path.join(testsDir, file)
        const testModule = await import(filePath)
        if (typeof testModule.default === 'function') {
            console.log(`====================`)
            console.log(`Running test: ${file}`)
            let driver = await createDriver()
            try {
                await testModule.default(driver)
                console.log(`Test ${file} finished successfully`)
                passedTests++
            }
            catch (err) {
                console.error(`Test ${file} failed`)
                console.error(err)
            }
            finally {
                await driver.quit()
            }
            console.log()
        }
    }
    
    console.log(`====================`)
    console.log(`Total tests: ${totalTests}`)
    console.log(`Passed tests: ${passedTests}`)
    console.log(`Failed tests: ${totalTests - passedTests}`)
    console.log(`====================`)
})