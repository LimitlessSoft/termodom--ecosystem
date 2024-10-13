import { create } from 'zustand'
import { handleApiError, officeApi } from '../apis/officeApi'

export const STANDARD_REFRESH_INTERVAL = 1000 * 60 * 10

export const useZStore = create((set) => ({
    komercijalno: {
        magacini: {
            data: undefined,
            lastRefresh: undefined,
            reloadAsync: async () => {
                await officeApi
                    .get(`/stores`)
                    .then((response) => {
                        set((state) => ({
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
            },
        },
    },
}))
