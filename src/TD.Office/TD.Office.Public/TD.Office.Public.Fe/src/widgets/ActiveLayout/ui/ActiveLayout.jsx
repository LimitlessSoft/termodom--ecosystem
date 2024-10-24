import { useRouter } from 'next/router'
import { useState, useEffect } from 'react'

export const ActiveLayout = ({ children }) => {
    const router = useRouter()
    const [ActiveLayout, setActiveLayout] = useState(null)

    const getRootPathSegment = (path) => {
        const segments = path.split('/').filter(Boolean)
        return '/' + segments[0]
    }
    const loadLayout = async (rootPathSegment) => {
        try {
            const layout = await import(
                `../../../pages${rootPathSegment}/layout.jsx`
            )
            return layout.default
        } catch (error) {
            console.error('Layout not found:', error)
            return null
        }
    }

    useEffect(() => {
        const rootPathSegment = getRootPathSegment(router.pathname)

        const fetchLayout = async () => {
            const layout = await loadLayout(rootPathSegment)
            setActiveLayout(() => layout)
        }

        fetchLayout()
    }, [router.pathname])

    return ActiveLayout ? <ActiveLayout>{children}</ActiveLayout> : children
}
