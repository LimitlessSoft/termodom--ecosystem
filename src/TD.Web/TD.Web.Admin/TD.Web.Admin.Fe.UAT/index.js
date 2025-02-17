import { promises as fs } from 'fs'
import path from 'path'
import { createDriver } from './driver.js'
import { ENV } from './constants.js'
//postgres package install

//flow:
//before test -
//run test -
//after test -

const testsDir = path.resolve('./tests')

async function runTests() {
    let passedTests = 0
    let totalTests = 0

    try {
        const files = await fs.readdir(testsDir)
        const testFiles = filterTestFiles(files)
        totalTests = testFiles.length

        const testResults = await Promise.all(testFiles.map(runTest))
        passedTests = testResults.filter((result) => result.passed).length

        logTestResults(totalTests, passedTests)

        if (passedTests !== totalTests) {
            throw new Error('Some tests failed')
        }
    } catch (err) {
        console.error('Error running tests:', err)
        process.exit(1)
    }
}

function filterTestFiles(files) {
    const runOnlyTheseTests = ENV === 'local' ? [] : null
    return runOnlyTheseTests && runOnlyTheseTests.length > 0
        ? runOnlyTheseTests
        : files.filter((file) => file.endsWith('.js'))
}

async function runTest(file) {
    const filePath = path.join(testsDir, file)
    const testModule = await import(filePath)
    const log = []
    const result = { passed: false }

    if (typeof testModule.default.execution === 'function') {
        log.push(`====================`)
        let driver = await createDriver()

        if (typeof testModule.default.beforeExecution === 'function') {
            testModule.default.beforeExecution()
        }

        try {
            log.push(
                `Starting test ${file} in browser: ${(
                    await driver.getCapabilities()
                ).get('browserName')}`
            )
            await testModule.default.execution(driver)
            log.push(`Test ${file} finished successfully`)
            console.log(log.join('\n'))
            result.passed = true
        } catch (err) {
            log.push(`Test ${file} failed`)
            log.push(err)
            console.log(log.join('\n'))
            result.passed = false
        } finally {
            await driver.quit()
        }

        if (typeof testModule.default.afterExecution === 'function') {
            testModule.default.afterExecution()
        }
        return result
    }
}

function logTestResults(totalTests, passedTests) {
    console.log(`====================`)
    console.log(`Total tests: ${totalTests}`)
    console.log(`Passed tests: ${passedTests}`)
    console.log(`Failed tests: ${totalTests - passedTests}`)
    console.log(`====================`)
}

runTests()
