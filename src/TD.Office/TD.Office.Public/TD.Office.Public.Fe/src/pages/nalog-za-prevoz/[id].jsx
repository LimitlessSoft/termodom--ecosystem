import { NalogZaPrevozPrint } from '@/widgets/NalogZaPrevoz/ui/NalogZaPrevozPrint'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { handleApiError, officeApi } from '@/apis/officeApi'

const NalogZaPrevozSingle = () => {
    const router = useRouter()

    const [data, setData] = useState<any | undefined>(undefined)

    useEffect(() => {
        if (router.query.id == null) return

        officeApi
            .get(`/nalog-za-prevoz/${router.query.id}`)
            .then((response: any) => {
                setData(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [router.query.id])

    return <NalogZaPrevozPrint data={data} />
}

export default NalogZaPrevozSingle
