import { useZStore } from './zStore'

export const useZLeftMenuVisible = () => {
    return useZStore((state) => state.ui.lefMenuVisible.data)
}

export const useZLeftMenuVisibleActions = () => {
    const show = useZStore((state) => state.ui.lefMenuVisible.show)
    const hide = useZStore((state) => state.ui.lefMenuVisible.hide)
    return { show, hide }
}
