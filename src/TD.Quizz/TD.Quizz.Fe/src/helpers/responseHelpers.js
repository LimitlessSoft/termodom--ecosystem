import { toast } from 'react-toastify'

export const handleResponse = (response, onOk, options = { }) => {
    console.log(response)
    switch (response.status) {
        case 200:
            onOk?.(response.data);
            break;
        case 201:
            onOk?.(response.data);
            break;
        case 204:
            onOk?.();
            break;
        case 400:
            if (options.onBadRequest) {
                options.onBadRequest(response.error)
                return
            }
            if (response.error)
                toast.error(response.error)
            else
                toast.error(`Bad Request`)
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
                options.onServerError(response.data);
            else
                toast.error("Server error!");
    }
}