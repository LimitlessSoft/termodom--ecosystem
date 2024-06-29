import { toast } from "react-toastify";
import { getCookie } from 'react-use-cookie';
import getConfig from 'next/config'
import {headers} from "next/headers";

export enum ApiBase {
    Main
}

export interface IResponse {
    status: number,
    notOk: boolean,
    payload: any,
    errors?: string[]
}

export interface IRequest {
    method: string,
    body?: any,
    contentType?: ContentType
}

export enum ContentType {
    ApplicationJson,
    FormData,
    TextPlain
}

export const fetchApi = (apiBase: ApiBase, endpoint: string, request?: IRequest, authorizationToken?: string) => {

    const publicRuntimeConfig = getConfig()!
    let baseUrl: string;

    if(apiBase == null)
        throw new Error(`Parameter 'apiBase' is required!`)
    
    switch(apiBase) {
        case ApiBase.Main:
            baseUrl = process.env.NEXT_PUBLIC_API_BASE_MAIN_URL!
            break;
        default:
            throw new Error(`Unhandled ApiBase!`)
    }

    let contentType: string = ''

    switch(request?.contentType) {
        case ContentType.ApplicationJson:
            contentType = 'application/json'
            break
        case ContentType.FormData:
            contentType = 'multipart/form-data; boundary=----'
            break
        case ContentType.TextPlain:
            contentType = 'text/plain'
            break
        case null:
            contentType = ''
            break
    }

    let headersVal: { [key: string]: string } = {
        'Authorization': 'bearer ' + (authorizationToken == null || authorizationToken?.length == 0 ? getCookie('token') : authorizationToken!)
    }
    console.log("Preparing headers:", headersVal)

    if(request?.contentType != ContentType.FormData) {
        headersVal['Content-Type'] = contentType
    }

    var requestUrl = `${baseUrl}${endpoint}`
    var requestObject = {
        body: request == null || request.contentType == null ? null : request.contentType == ContentType.FormData ? request.body : JSON.stringify(request.body),
        method: request?.method ?? 'GET',
        headers: headersVal
    }

    return new Promise<any>((resolve, reject) => {
        fetch(requestUrl, requestObject).then((response) => {
            console.log(response)
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
                            console.log(json)
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