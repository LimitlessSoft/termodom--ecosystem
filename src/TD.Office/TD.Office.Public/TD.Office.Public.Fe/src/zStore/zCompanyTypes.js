import { STANDARD_REFRESH_INTERVAL, useZStore } from './zStore'
import { handleApiError, officeApi } from '../apis/officeApi'

export const reloadCompanyTypesAsync = async () => {
    await officeApi
        .get(`/company-types`)
        .then((response) => {
            useZStore.setState((state) => ({
                ...state,
                companyTypes: {
                    ...state.companyTypes,
                    data: response.data,
                    lastRefresh: new Date(),
                }
            }))
        })
        .catch(handleApiError)
}

export const useZCompanyTypes = () => {
    const companyTypes = useZStore((state) => state.companyTypes)

    if (
        companyTypes.lastRefresh === undefined ||
        companyTypes.lastRefresh <
            new Date(new Date().getTime() - STANDARD_REFRESH_INTERVAL)
    ) {
        useZStore.getState().companyTypes.reloadAsync()
    }

    return companyTypes.data
}
