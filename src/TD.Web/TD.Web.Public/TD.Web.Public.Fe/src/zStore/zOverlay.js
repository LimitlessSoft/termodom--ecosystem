import { useZStore } from './zStore'

export const useZOverlay = () => {
    const zOverlay = useZStore((state) => state.overlay)
    const { data, show, hide, toggle } = zOverlay
    const displayed = data
    return {
        displayed,
        toggle,
        show,
        hide,
    }
}
