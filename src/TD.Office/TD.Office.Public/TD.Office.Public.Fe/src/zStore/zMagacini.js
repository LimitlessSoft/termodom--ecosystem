import { STANDARD_REFRESH_INTERVAL, useZStore } from './zStore'
import { handleApiError, officeApi } from '../apis/officeApi'

export const reloadMagaciniAsync = async () => {
    await officeApi
        .get(`/stores`)
        .then((response) => {
            useZStore.setState((state) => ({
                ...state,
                komercijalno: {
                    ...state.komercijalno,
                    magacini: {
                        ...state.komercijalno.magacini,
                        data: response.data,
                        lastRefresh: new Date(),
                    },
                },
            }))
        })
        .catch(handleApiError)
}

export const useZMagacini = () => {
    const magacini = useZStore((state) => state.komercijalno.magacini)

    if (
        magacini.lastRefresh === undefined ||
        magacini.lastRefresh <
            new Date(new Date().getTime() - STANDARD_REFRESH_INTERVAL)
    ) {
        useZStore.getState().komercijalno.magacini.reloadAsync()
    }

    return magacini.data
}
