"use client";
import { Home, Login } from '@/widgets'
import { useZUser } from '@/zStore'

const HomePage = () => {
    const zUser = useZUser()
    
    if (!zUser)
        return <Login />
    
    return <Home />
}
export default HomePage