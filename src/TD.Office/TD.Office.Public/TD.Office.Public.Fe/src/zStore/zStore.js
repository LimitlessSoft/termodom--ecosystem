import { create } from 'zustand'
import { reloadMagaciniAsync } from './zMagacini'
import { reloadVrDoksAsync } from './zVrDoks'
import { reloadNaciniPlacanjaAsync } from './zNaciniPlacanja'
import { reloadRobaAsync } from './zRoba'
import { reloadCompanyTypesAsync } from './zCompanyTypes'
import { reloadMassSMSStatusAsync } from './zMassSMSStatus'
import { reloadMassSMSQueueAsync } from './zMassSMSQueue'
import { reloadMassSMSQueueCountAsync } from './zMassSMSQueueCount'

export const STANDARD_REFRESH_INTERVAL = 1000 * 60 * 10
export const LONG_REFRESH_INTERVAL = 1000 * 60 * 60 * 24

export const useZStore = create((set) => ({
    ui: {
        lefMenuVisible: {
            data: true,
            hide: () => {
                set((state) => ({
                    ui: {
                        ...state.ui,
                        lefMenuVisible: {
                            ...state.ui.lefMenuVisible,
                            data: false,
                        },
                    },
                }))
            },
            show: () => {
                set((state) => ({
                    ui: {
                        ...state.ui,
                        lefMenuVisible: {
                            ...state.ui.lefMenuVisible,
                            data: true,
                        },
                    },
                }))
            },
        },
    },
    companyTypes: {
        data: undefined,
        lastRefresh: undefined,
        reloadAsync: async () => {
            await reloadCompanyTypesAsync()
        },
    },
    massSMSStatus: {
        data: undefined,
        lastRefresh: undefined,
        reloadAsync: async () => {
            await reloadMassSMSStatusAsync()
        },
    },
    massSMSQueue: {
        data: undefined,
        lastRefresh: undefined,
        reloadAsync: async () => {
            await reloadMassSMSQueueAsync()
        },
    },
    massSMSQueueCount: {
        data: undefined,
        lastRefresh: undefined,
        reloadAsync: async () => {
            await reloadMassSMSQueueCountAsync()
        },
    },
    komercijalno: {
        magacini: {
            data: undefined,
            lastRefresh: undefined,
            reloadAsync: async () => {
                await reloadMagaciniAsync()
            },
        },
        roba: {
            data: undefined,
            lastRefresh: undefined,
            reloadAsync: async () => {
                await reloadRobaAsync()
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
