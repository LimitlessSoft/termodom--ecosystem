import { create } from 'zustand'
import { persist } from 'zustand/middleware'

const useStore = create(persist(set => ({
    user: null,
    setUser: (user) => set(() => ({ user })),
})), {
    name: 'user-storage', // unique name for the storage
    getStorage: () => sessionStorage, // use localStorage as the storage
})

export const useZUser = () => useStore((state) => state.user)
export const useZSetUser = () => useStore((state) => state.setUser)