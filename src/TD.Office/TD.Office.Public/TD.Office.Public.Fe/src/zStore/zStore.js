import { create } from 'zustand'
import { reloadMagaciniAsync } from './zMagacini'
import { reloadVrDoksAsync } from './zVrDoks'
import { reloadNaciniPlacanjaAsync } from './zNaciniPlacanja'

export const STANDARD_REFRESH_INTERVAL = 1000 * 60 * 10
export const LONG_REFRESH_INTERVAL = 1000 * 60 * 60 * 24

export const useZStore = create((set) => ({
    komercijalno: {
        magacini: {
            data: undefined,
            lastRefresh: undefined,
            reloadAsync: async () => {
                await reloadMagaciniAsync()
            },
        },
        vrDoks: {
            data: undefined,
            lastRefresh: undefined,
            reloadAsync: async () => {
                await reloadVrDoksAsync()
            },
        },
        naciniPlacanja: {
            data: undefined,
            lastRefresh: undefined,
            reloadAsync: async () => {
                await reloadNaciniPlacanjaAsync()
            },
        },
    },
}))
