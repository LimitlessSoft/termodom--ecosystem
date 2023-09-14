import getConfig from "next/config";

export enum ApiBase {
    Main
}

export interface IResponse {
    status: number,
    notOk: boolean,
    payload: any,
    errors?: string[]
}

export const fetchApi = (apiBase: ApiBase, endpoint: string) => {
    
    const { publicRuntimeConfig } = getConfig()
    let baseUrl: string;

    if(apiBase == null)
        throw new Error(`Parameter 'apiBase' is required!`)

    switch(apiBase) {
        case ApiBase.Main:
            baseUrl = publicRuntimeConfig.API_BASE_URL_MAIN
            break;
        default:
            throw new Error(`Unhandled ApiBase!`)
    }

    return new Promise<IResponse>((resolve, reject) => {
        console.log(`LOgging: ${baseUrl}${endpoint}`)
        fetch(`${baseUrl}${endpoint}`).then((response) => {
            if(response.status == 200) {
                response.json().then((apiResponseObject) => {
                    resolve(apiResponseObject)
                })
            }
        }).catch((reason) => {
            reject(reason)
        })
    })
}