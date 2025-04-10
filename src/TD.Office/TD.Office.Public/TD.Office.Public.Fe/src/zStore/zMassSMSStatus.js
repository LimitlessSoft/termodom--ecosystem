import { handleApiError, officeApi } from '../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../constants'
import { STANDARD_REFRESH_INTERVAL, useZStore } from './zStore'

export const reloadMassSMSStatusAsync = () => {
    officeApi
        .get(ENDPOINTS_CONSTANTS.MASS_SMS.STATUS)
        .then((response) => {
            useZStore.setState((state) => ({
                ...state,
                massSMSStatus: {
                    ...state.massSMSStatus,
                    data: response.data,
                    lastRefresh: new Date(),
                },
            }))
        })
        .catch((err) => handleApiError(err))
}

export const useZMassSMSStatus = () => {
    const massSMSStatus = useZStore((state) => state.massSMSStatus)

    if (
        massSMSStatus.lastRefresh === undefined ||
        massSMSStatus.lastRefresh <
            new Date(new Date().getTime() - STANDARD_REFRESH_INTERVAL)
    ) {
        useZStore.getState().massSMSStatus.reloadAsync()
    }

    return massSMSStatus.data
}

export const forceReloadZMassSMSStatus = () => {
    useZStore.getState().massSMSStatus.reloadAsync()
}
