import { STANDARD_REFRESH_INTERVAL, useZStore } from './zStore'
import { handleApiError, officeApi } from '../apis/officeApi'
import { useEffect, useState } from 'react'

export const reloadRobaAsync = async () => {
    await officeApi
        .get(`/komercijalno-roba`)
        .then((response) => {
            useZStore.setState((state) => ({
                ...state,
                komercijalno: {
                    ...state.komercijalno,
                    roba: {
                        ...state.komercijalno.roba,
                        data: response.data,
                        lastRefresh: new Date(),
                    },
                },
            }))
        })
        .catch(handleApiError)
}

export const useZRoba = () => {
    const roba = useZStore((state) => state.komercijalno.roba)

    if (
        roba.lastRefresh === undefined ||
        roba.lastRefresh <
            new Date(new Date().getTime() - STANDARD_REFRESH_INTERVAL)
    ) {
        useZStore.getState().komercijalno.roba.reloadAsync()
    }

    return roba.data
}
