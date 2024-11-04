import {
    LONG_REFRESH_INTERVAL,
    STANDARD_REFRESH_INTERVAL,
    useZStore,
} from './zStore'
import { handleApiError, officeApi } from '../apis/officeApi'

export const reloadVrDoksAsync = async () => {
    await officeApi
        .get(`/komercijalno-vr-dok`)
        .then((response) => {
            useZStore.setState((state) => ({
                ...state,
                komercijalno: {
                    ...state.komercijalno,
                    vrDoks: {
                        ...state.komercijalno.vrDoks,
                        data: response.data,
                        lastRefresh: new Date(),
                    },
                },
            }))
        })
        .catch(handleApiError)
}

export const useVrDoks = () => {
    const vrDoks = useZStore((state) => state.komercijalno.vrDoks)

    if (
        vrDoks.lastRefresh === undefined ||
        vrDoks.lastRefresh <
            new Date(new Date().getTime() - LONG_REFRESH_INTERVAL)
    ) {
        vrDoks.reloadAsync()
    }

    return vrDoks.data
}
