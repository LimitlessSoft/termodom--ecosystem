import { toast } from 'react-toastify'

export const handleResponse = async (response, onOk, options = {}) => {
    switch (response.status) {
        case 200:
            onOk?.(await response.json())
            break
        case 201:
            onOk?.(await response.json())
            break
        case 204:
            onOk?.()
            break
        case 400:
            response.json().then((j) => {
                if (options.onBadRequest) {
                    options.onBadRequest(response.error)
                    return
                }
                if (typeof j === 'string') {
                    toast.error(j || `Bad Request`)
                    return
                }
                if (j?.error) {
                    if (Array.isArray(j.error))
                        j.error.map((e) => toast.error(e))
                    else toast.error(j.error || `Bad Request`)
                }
            })
            break
        case 401:
            if (options.onUnAuthenticated) options.onUnAuthenticated()
            else toast.error('Niste autentikovani')
            break
        case 403:
            if (options.onForbidden) options.onForbidden()
            else toast.error('Nemate pravo')
            break
        case 404:
            response.json().then((data) => {
                if (options.onNotFound) {
                    options.onNotFound()
                    return
                }

                toast.error(data.error || 'Nije pronađeno')
            })
            break
        case 500:
        default:
            if (options.onServerError) options.onServerError()
            else toast.error('Server error!')
    }
}
