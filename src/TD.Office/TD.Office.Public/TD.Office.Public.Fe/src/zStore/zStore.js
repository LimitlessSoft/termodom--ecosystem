import { create } from 'zustand'
import { persist, createJSONStorage } from 'zustand/middleware'
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

export const useZStore = create(
    persist(
        (set) => ({
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
        }),
        {
            name: 'td-office-zstore',
            storage: createJSONStorage(() => localStorage),
            partialize: (state) => ({
                ui: {
                    lefMenuVisible: {
                        data: state.ui.lefMenuVisible.data,
                    },
                },
                companyTypes: {
                    data: state.companyTypes.data,
                    lastRefresh: state.companyTypes.lastRefresh,
                },
                massSMSStatus: {
                    data: state.massSMSStatus.data,
                    lastRefresh: state.massSMSStatus.lastRefresh,
                },
                massSMSQueue: {
                    data: state.massSMSQueue.data,
                    lastRefresh: state.massSMSQueue.lastRefresh,
                },
                massSMSQueueCount: {
                    data: state.massSMSQueueCount.data,
                    lastRefresh: state.massSMSQueueCount.lastRefresh,
                },
                komercijalno: {
                    magacini: {
                        data: state.komercijalno.magacini.data,
                        lastRefresh: state.komercijalno.magacini.lastRefresh,
                    },
                    roba: {
                        data: state.komercijalno.roba.data,
                        lastRefresh: state.komercijalno.roba.lastRefresh,
                    },
                    vrDoks: {
                        data: state.komercijalno.vrDoks.data,
                        lastRefresh: state.komercijalno.vrDoks.lastRefresh,
                    },
                    naciniPlacanja: {
                        data: state.komercijalno.naciniPlacanja.data,
                        lastRefresh:
                            state.komercijalno.naciniPlacanja.lastRefresh,
                    },
                },
            }),
            merge: (persistedState, currentState) => {
                if (!persistedState) return currentState

                const mergeDeep = (current, persisted) => {
                    const result = { ...current }
                    for (const key in persisted) {
                        if (
                            persisted[key] !== null &&
                            typeof persisted[key] === 'object' &&
                            !Array.isArray(persisted[key]) &&
                            current[key]
                        ) {
                            result[key] = mergeDeep(
                                current[key],
                                persisted[key]
                            )
                        } else {
                            result[key] = persisted[key]
                        }
                    }
                    return result
                }

                return mergeDeep(currentState, persistedState)
            },
            onRehydrateStorage: () => (state) => {
                if (!state) return

                const fixDate = (obj) => {
                    if (
                        obj &&
                        obj.lastRefresh &&
                        typeof obj.lastRefresh === 'string'
                    ) {
                        obj.lastRefresh = new Date(obj.lastRefresh)
                    }
                }

                fixDate(state.companyTypes)
                fixDate(state.massSMSStatus)
                fixDate(state.massSMSQueue)
                fixDate(state.massSMSQueueCount)
                if (state.komercijalno) {
                    fixDate(state.komercijalno.magacini)
                    fixDate(state.komercijalno.roba)
                    fixDate(state.komercijalno.vrDoks)
                    fixDate(state.komercijalno.naciniPlacanja)
                }
            },
        }
    )
)

if (typeof window !== 'undefined') {
    window.addEventListener('storage', (event) => {
        if (event.key === 'td-office-zstore') {
            useZStore.persist.rehydrate()
        }
    })
}
