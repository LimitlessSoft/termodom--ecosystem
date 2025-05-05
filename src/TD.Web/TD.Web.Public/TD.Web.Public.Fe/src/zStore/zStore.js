import { create } from 'zustand'

export const useZStore = create((set) => ({
    overlay: {
        data: false,
        show: () =>
            set((state) => ({ overlay: { ...state.overlay, data: true } })),
        hide: () =>
            set((state) => ({ overlay: { ...state.overlay, data: false } })),
        toggle: () =>
            set((state) => ({
                overlay: { ...state.overlay, data: !state.overlay.data },
            })),
    },
}))
