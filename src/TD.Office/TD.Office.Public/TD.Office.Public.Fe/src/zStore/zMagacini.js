import { STANDARD_REFRESH_INTERVAL, useZStore } from './zStore'

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
