/**
 * Logs a server error message to the console and returns an Error object if message is provided, otherwise returns false.
 * If message is empty or not provided, it returns false.
 * Error is logged with full stack trace.
 * @param message
 * @returns {Error|boolean}
 */
export const logServerError = (message) => {
    if (!message || message.length === 0) return false
    const error = new Error(message)
    console.error(error)
    return error
}
/**
 * Logs a server error, calls the provided reject function with the error, and returns the error object if handled, or false otherwise.
 * If message is empty or not provided, it returns false.
 * Error is logged with full stack trace.
 * @param {string} message - Error message to log.
 * @param {function} reject - Function to call with the error.
 */
export const logServerErrorAndReject = (message, reject) => {
    const error = logServerError(message)
    if (!error) return false
    reject(error)
    return error
}
