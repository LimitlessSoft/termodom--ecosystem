import { toast } from 'react-toastify'

export const handleResponse = async (response, onOk, options = {}) => {
    switch (response.status) {
        case 200:
            onOk?.(await response.json());
            break;
        case 201:
            onOk?.(await response.json());
            break;
        case 204:
            onOk?.();
            break;
        case 400:
            response.json().then((j) => {
                if (options.onBadRequest) {
                    options.onBadRequest(response.error)
                    return
                }
                toast.error(j || `Bad Request`)
            })
            break;
        case 401:
            if (options.onUnAuthenticated)
                options.onUnAuthenticated()
            else
                toast.error("Niste autentikovani");
            break;
        case 403:
            if (options.onForbidden)
                options.onForbidden();
            else
                toast.error("Nemate pravo");
            break;
        case 404:
            if (options.onNotFound)
                options.onNotFound();
            else
                toast.error("Nije pronađeno");
            break;
        case 500:
        default:
            if (options.onServerError)
                options.onServerError();
            else
                toast.error("Server error!");
    }
}