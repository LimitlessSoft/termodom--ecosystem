import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { OtpremnicaSingleWrapper } from '../../widgets'

const OtpremnicaSinglePage = () => {
    const router = useRouter()
    
    const [id, setId] = useState(undefined)
    
    useEffect(() => {
        if (router.query.id) setId(router.query.id)
        else setId(undefined)
    }, [router, router.query.id])
    
    if (!id) return null
    
    return <OtpremnicaSingleWrapper id={id} />
}

export default OtpremnicaSinglePage