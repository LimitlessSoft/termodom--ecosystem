import { useRouter } from 'next/router'
import { useState, useEffect } from 'react'
import { CircularProgress } from '@mui/material'

export const ActiveLayout = ({ children }) => {
    const router = useRouter()
    const [ActiveLayout, setActiveLayout] = useState(undefined)

    useEffect(() => {
        import(`../../../pages/${router.pathname.split('/')[1]}/layout.jsx`)
            .then((layout) => setActiveLayout(() => layout.default))
            .catch(() => setActiveLayout(null))
    }, [router.pathname])

    return ActiveLayout === undefined ? (
        <CircularProgress />
    ) : ActiveLayout ? (
        <ActiveLayout>{children}</ActiveLayout>
    ) : (
        children
    )
}
