import { handleApiError, officeApi } from '../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../constants'
import { STANDARD_REFRESH_INTERVAL, useZStore } from './zStore'

export const reloadMassSMSQueueAsync = async () => {
    await officeApi
        .get(ENDPOINTS_CONSTANTS.MASS_SMS.QUEUE)
        .then((response) => {
            useZStore.setState((state) => ({
                ...state,
                massSMSQueue: {
                    ...state.massSMSQueue,
                    data: response.data,
                    lastRefresh: new Date(),
                },
            }))
        })
        .catch((err) => handleApiError(err))
}

export const useZMassSMSQueue = () => {
    const massSMSQueue = useZStore((state) => state.massSMSQueue)

    if (
        massSMSQueue.lastRefresh === undefined ||
        massSMSQueue.lastRefresh <
            new Date(new Date().getTime() - STANDARD_REFRESH_INTERVAL)
    ) {
        useZStore.getState().massSMSQueue.reloadAsync()
    }

    return massSMSQueue.data
}

export const forceReloadZMassSMSQueueAsync = async () => {
    await useZStore.getState().massSMSQueue.reloadAsync()
}
