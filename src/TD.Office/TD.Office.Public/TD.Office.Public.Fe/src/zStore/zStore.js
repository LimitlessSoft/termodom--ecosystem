import { create } from 'zustand'
import { reloadMagaciniAsync } from './zMagacini'

export const STANDARD_REFRESH_INTERVAL = 1000 * 60 * 10

export const useZStore = create((set) => ({
    komercijalno: {
        magacini: {
            data: undefined,
            lastRefresh: undefined,
            reloadAsync: async () => {
                await reloadMagaciniAsync()
            },
        },
    },
}))
