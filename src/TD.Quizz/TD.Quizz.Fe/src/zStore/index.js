import { create } from 'zustand'

const useStore = create((set) => ({
    sessionFetching: false,
    setSessionFetching: (value) => set({ sessionFetching: value }),
}))

export const useZSessionFetching = () => {
    const sessionFetching = useStore((state) => state.sessionFetching)
    const setSessionFetching = useStore((state) => state.setSessionFetching)

    return {
        isFetching: sessionFetching,
        set: setSessionFetching,
    }
}
