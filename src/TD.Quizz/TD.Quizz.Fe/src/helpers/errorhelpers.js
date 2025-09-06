/**
 * Logs a server error message to the console and returns an Error object if message is provided, otherwise returns false.
 * If message is empty or not provided, it returns false.
 * Stack trace is ensured (if missing, a new stack trace is generated, otherwise existing stack is preserved).
 * @param {string|Error} error - Error or message to log.
 * @returns {Error|boolean} - Returns the Error object if message is provided, otherwise false.
 */
export const logServerError = (error) => {
    if (!error) return false
    if (typeof error === 'string' && error.length === 0) return false
    if (error instanceof Error && !error.stack) error.stack = new Error().stack
    const e = parseErrorObject(error)
    console.error(e)
    return e
}
/**
 * Logs a server error, calls the provided reject function with the error, and returns the error object if handled, or false otherwise.
 * If error is empty or not provided, it returns false.
 * Stack trace is ensured (if missing, a new stack trace is generated, otherwise existing stack is preserved).
 * @param {string|Error} error - Error or message to log.
 * @param {function} reject - Function to call with the error.
 * @returns {Error|boolean} - Returns the Error object if handled, otherwise false.
 */
export const logServerErrorAndReject = (error, reject) => {
    const e = logServerError(error)
    if (!e) return false
    reject(e)
    return e
}
/**
 * Parses the input into an Error object.
 * If input is null or undefined, returns null.
 * @param error
 * @returns {Error|null}
 */
const parseErrorObject = (error) => {
    if (!error) return null
    if (typeof error === 'string') return new Error(error)
    if (error instanceof Error) {
        if (!error.stack) error.stack = new Error().stack
        return error
    }
    return new Error(JSON.stringify(error))
}
