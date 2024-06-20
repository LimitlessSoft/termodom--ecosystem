import getConfig from "next/config";
import { toast } from "react-toastify";
import { getCookie } from 'react-use-cookie';

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
    FormData
}

export const fetchApi = (apiBase: ApiBase, endpoint: string, request?: IRequest, rawResponse: boolean = true, authorizationToken?: string) => {
    
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
        case null:
            contentType = ''
            break
    }

    let headersVal: { [key: string]: string } = {
        'Authorization': 'bearer ' + (authorizationToken == null || authorizationToken?.length == 0 ? getCookie('token') : authorizationToken!)
    }

    if(request?.contentType != ContentType.FormData) {
        headersVal['Content-Type'] = contentType
    }

    var requestUrl = `${baseUrl}${endpoint}`
    var requestObject = {
        body: request == null || request.contentType == null ? null : request.contentType == ContentType.FormData ? request.body : JSON.stringify(request.body),
        method: request?.method ?? 'GET',
        headers: headersVal
    }

    // console.log(`fetching: ${requestUrl} /with object: ${JSON.stringify(requestObject)}`)

    return new Promise<any>((resolve, reject) => {
        fetch(requestUrl, requestObject).then((response) => {
            console.log(response)
            if(response.status == 200) {
                response.json()
                .then((apiResponseObject) => {
                    if(rawResponse)
                        resolve(apiResponseObject)
                    else
                        resolve(apiResponseObject.payload)
                    return 
                })
            } else if(response.status == 400) {
                response.json()
                .then((apiResponseObject) => {
                    console.log(apiResponseObject)
                    apiResponseObject.map((o: any) => {
                        toast(o.ErrorMessage, { type: 'error' })
                    })
                    reject()
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
            console.log(reason)
            toast(`Unknown api error!`, { type: 'error' })
        })
    })
}