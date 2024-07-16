import { ApiBase, fetchApi } from '@/app/api'
import { NalogZaPrevozPrint } from '@/widgets/NalogZaPrevoz/ui/NalogZaPrevozPrint'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'

const NalogZaPrevozSingle = () => {
    const router = useRouter()

    const [data, setData] = useState<any | undefined>(undefined)

    useEffect(() => {
        if (router.query.id == null) return

        fetchApi(ApiBase.Main, `/nalog-za-prevoz/${router.query.id}`).then(
            (response) => {
                response.json().then((response: any) => setData(response))
            }
        )
    }, [router.query.id])

    return <NalogZaPrevozPrint data={data} />
}

export default NalogZaPrevozSingle
