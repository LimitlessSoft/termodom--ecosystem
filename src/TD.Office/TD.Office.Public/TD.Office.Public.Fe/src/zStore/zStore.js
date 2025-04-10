import { create } from 'zustand'
import { reloadMagaciniAsync } from './zMagacini'
import { reloadVrDoksAsync } from './zVrDoks'
import { reloadNaciniPlacanjaAsync } from './zNaciniPlacanja'
import { reloadRobaAsync } from './zRoba'
import { reloadCompanyTypesAsync } from './zCompanyTypes'
import { reloadMassSMSStatusAsync } from './zMassSMSStatus'
import { reloadMassSMSQueueAsync } from './zMassSMSQueue'

export const STANDARD_REFRESH_INTERVAL = 1000 * 60 * 10
export const LONG_REFRESH_INTERVAL = 1000 * 60 * 60 * 24

export const useZStore = create((set) => ({
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
