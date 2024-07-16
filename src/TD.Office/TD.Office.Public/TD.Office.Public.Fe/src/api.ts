import { getCookie } from 'react-use-cookie';
import { toast } from "react-toastify";

export enum ApiBase1 {
    Main
}

export interface IResponse {
    status: number,
    notOk: boolean,
    payload: any,
    errors?: string[]
}

export interface IRequest1 {
    method: string,
    body?: any,
    contentType?: ContentType1
}

export enum ContentType1 {
    ApplicationJson,
    FormData,
    TextPlain
}

export const fetchApi1 = (apiBase: ApiBase1, endpoint: string, request?: IRequest1, authorizationToken?: string) => {

    let baseUrl: string;

    if(apiBase == null)
        throw new Error(`Parameter 'apiBase' is required!`)

    switch(apiBase) {
        case ApiBase1.Main:
            baseUrl = process.env.NEXT_PUBLIC_API_BASE_MAIN_URL!
            break;
        default:
            throw new Error(`Unhandled ApiBase!`)
    }

    let contentType: string = ''

    switch(request?.contentType) {
        case ContentType1.ApplicationJson:
            contentType = 'application/json'
            break
        case ContentType1.FormData:
            contentType = 'multipart/form-data; boundary=----'
            break
        case ContentType1.TextPlain:
            contentType = 'text/plain'
            break
        case null:
            contentType = ''
            break
    }

    let headersVal: { [key: string]: string } = {
        'Authorization': 'bearer ' + (authorizationToken == null || authorizationToken?.length == 0 ? getCookie('token') : authorizationToken!)
    }

    if(request?.contentType != ContentType1.FormData) {
        headersVal['Content-Type'] = contentType
    }

    var requestUrl = `${baseUrl}${endpoint}`
    var requestObject = {
        body: request == null || request.contentType == null ? null : request.contentType == ContentType1.FormData ? request.body : JSON.stringify(request.body),
        method: request?.method ?? 'GET',
        headers: headersVal
    }

    return new Promise<any>((resolve, reject) => {
        fetch(requestUrl, requestObject).then((response) => {
            if(response.status == 200) {
                resolve(response)
            } else if(response.status == 400) {
                if (parseInt(response.headers.get(`content-length`)!) === 0)
                {
                    toast("Bad request", { type: 'error' })
                    reject()
                    return
                }
                response.text()
                    .then((t: string) => {
                        try {
                            let json = JSON.parse(t)
                            json.forEach((e: any) => {
                                toast(e.ErrorMessage, { type: 'error' })
                            })
                            reject()
                        }
                        catch {
                            toast(t, { type: 'error' })
                            reject()
                        }
                    })
            } else if(response.status == 404) {
                toast('Resource not found!', { type: 'error' })
                reject()
            } else if(response.status == 500) {
                toast('Unknown api error!', { type: 'error' })
                reject()
            } else if(response.status == 401) {
                toast('Unauthorized!', { type: 'error' })
            } else {
                toast(`Error fetching api (${response.status})!`, { type: 'error' })
                reject(response.status)
            }
        }).catch((reason) => {
            toast(`Unknown api error!`, { type: 'error' })
        })
    })
}