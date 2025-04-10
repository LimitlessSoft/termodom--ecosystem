import { handleApiError, officeApi } from '../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../constants'
import { STANDARD_REFRESH_INTERVAL, useZStore } from './zStore'

export const reloadMassSMSQueueCountAsync = () => {
    officeApi
        .get(ENDPOINTS_CONSTANTS.MASS_SMS.QUEUE_COUNT)
        .then((response) => {
            useZStore.setState((state) => ({
                ...state,
                massSMSQueueCount: {
                    ...state.massSMSQueueCount,
                    data: response.data,
                    lastRefresh: new Date(),
                },
            }))
        })
        .catch((err) => handleApiError(err))
}

export const useZMassSMSQueueCount = () => {
    const massSMSQueueCount = useZStore((state) => state.massSMSQueueCount)

    if (
        massSMSQueueCount.lastRefresh === undefined ||
        massSMSQueueCount.lastRefresh <
            new Date(new Date().getTime() - STANDARD_REFRESH_INTERVAL)
    ) {
        useZStore.getState().massSMSQueueCount.reloadAsync()
    }

    return massSMSQueueCount.data
}

export const forceReloadMassSMSQueueCountAsync = async () => {
    await useZStore.getState().massSMSQueueCount.reloadAsync()
}
