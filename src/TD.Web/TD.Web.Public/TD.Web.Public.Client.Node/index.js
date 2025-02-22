const axios = require('axios')
const UsersEndpoints = require('./src/endpoints/usersEndpoints')

const handleApiError = (error) => {
    if (error.code === 'ERR_CANCELED') return

    const method = error.config?.method?.toUpperCase() || 'UNKNOWN METHOD'
    const url = error.config?.url || 'UNKNOWN URL'

    let errorMessage = `Error in request: ${method} ${url} - `

    if (error.response) {
        switch (error.response.status) {
            case 400:
                if (!error.response.data) {
                    errorMessage += 'Error 400'
                    break
                }

                if (Array.isArray(error.response.data)) {
                    errorMessage += error.response.data
                        .map((item) => item.ErrorMessage)
                        .join(', ')
                    break
                }

                errorMessage += error.response.data
                break
            case 401:
                errorMessage += 'Unauthenticated'
                break
            case 403:
                errorMessage += 'Unauthorized'
                break
            case 404:
                errorMessage += 'Not found'
                break
            case 500:
                errorMessage += 'Server error'
                break
            default:
                errorMessage += 'Unknown error'
                break
        }
    }

    throw new Error(errorMessage)
}

module.exports = class WebPublicClient {
    constructor(url) {
        this.url = url;
        this.axios = axios.create({
            baseURL: url,
        })
        this.axios.interceptors.response.use(
            (response) => response,
            (error) => handleApiError(error)
        )
        this.users = new UsersEndpoints(this.axios)
    }
}