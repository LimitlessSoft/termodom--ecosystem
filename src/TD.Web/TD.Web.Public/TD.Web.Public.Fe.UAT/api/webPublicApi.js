import axios from 'axios'

function getCookie(name) {
    const value = `; ${document.cookie}`
    const parts = value.split(`; ${name}=`)
    if (parts.length === 2) return parts.pop().split(';').shift()
    return null
}

export const webPublicApi = axios.create({
    baseURL: 'http://localhost:8080',
})

export const handleApiError = (error) => {
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

webPublicApi.interceptors.response.use(
    (response) => response,
    (error) => handleApiError(error)
)
