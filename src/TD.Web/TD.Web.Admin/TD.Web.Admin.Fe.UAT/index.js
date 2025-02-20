import { promises as fs } from 'fs'
import path from 'path'
import { createDriver } from './driver.js'
import { ENV } from './constants.js'

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
        console.error('Error running tests:', err.message)
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

    const { beforeExecution, afterExecution, execution } = testModule.default

    if (
        (beforeExecution && !afterExecution) ||
        (!beforeExecution && afterExecution)
    ) {
        throw new Error(
            `Test module ${file} must define both 'beforeExecution' and 'afterExecution' if one of them exists.`
        )
    } else if (beforeExecution && typeof beforeExecution !== 'function') {
        throw new Error(`'beforeExecution' in ${file} must be a function.`)
    } else if (afterExecution && typeof afterExecution !== 'function') {
        throw new Error(`'afterExecution' in ${file} must be a function.`)
    } else if (!execution) {
        throw new Error(`Test ${file} is missing an 'execution' function.`)
    } else if (typeof execution !== 'function') {
        throw new Error(`'execution' in ${file} must be a function.`)
    }

    log.push(`====================`)
    let driver = await createDriver()

    try {
        if (beforeExecution) await testModule.default.beforeExecution()

        log.push(
            `Starting test ${file} in browser: ${(
                await driver.getCapabilities()
            ).get('browserName')}`
        )
        await execution(driver)
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
        if (afterExecution) await afterExecution()
    }

    return result
}

function logTestResults(totalTests, passedTests) {
    console.log(`====================`)
    console.log(`Total tests: ${totalTests}`)
    console.log(`Passed tests: ${passedTests}`)
    console.log(`Failed tests: ${totalTests - passedTests}`)
    console.log(`====================`)
}

runTests()
