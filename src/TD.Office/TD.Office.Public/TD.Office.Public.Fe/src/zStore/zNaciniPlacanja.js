import {
    LONG_REFRESH_INTERVAL,
    STANDARD_REFRESH_INTERVAL,
    useZStore,
} from './zStore'
import { handleApiError, officeApi } from '../apis/officeApi'

export const reloadNaciniPlacanjaAsync = async () => {
    await officeApi
        .get(`/komercijalno-nacini-placanja`)
        .then((response) => {
            useZStore.setState((state) => ({
                ...state,
                komercijalno: {
                    ...state.komercijalno,
                    naciniPlacanja: {
                        ...state.komercijalno.naciniPlacanja,
                        data: response.data,
                        lastRefresh: new Date(),
                    },
                },
            }))
        })
        .catch(handleApiError)
}

export const useZNaciniPlacanja = () => {
    const naciniPlacanja = useZStore(
        (state) => state.komercijalno.naciniPlacanja
    )

    if (
        naciniPlacanja.lastRefresh === undefined ||
        naciniPlacanja.lastRefresh <
            new Date(new Date().getTime() - LONG_REFRESH_INTERVAL)
    ) {
        naciniPlacanja.reloadAsync()
    }

    return naciniPlacanja.data
}
