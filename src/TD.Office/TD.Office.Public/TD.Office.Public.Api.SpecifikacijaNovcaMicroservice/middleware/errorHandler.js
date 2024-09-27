const errorHandler = (err, req, res, next) => {
    const statusCode = err.status || 500
    const message = err.message || 'Internal Server Error'

    return res.status(statusCode).json({
        message,
        ...(process.env.NODE_ENV === 'development' && { stack: err.stack }),
    })
}

module.exports = errorHandler
