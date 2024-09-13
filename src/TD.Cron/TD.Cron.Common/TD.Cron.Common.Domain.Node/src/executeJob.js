const executeJobAsync = async (job) => {
    try {
        const startTime = new Date()

        await job()

        const endTime = new Date()
        console.log('Job executed successfully!')

        const executionTime = new Date(endTime - startTime)
            .toISOString()
            .substr(11, 12)
        console.log('Execution time: ' + executionTime)
    } catch (err) {
        console.log('An error occurred while execution the job!')
        console.log(err)
    } finally {
        process.exit(0)
    }
}

module.exports = executeJobAsync
