import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import { ProizvodiSearch } from '@/widgets/Proizvodi/ProizvodiSearch'
import { ProizvodiFilter } from '@/widgets/Proizvodi/ProizvodiFilter'
import { ProizvodiList } from '@/widgets/Proizvodi/ProizvodiList'
import { ModKupovinePoruka } from '@/widgets/ModKupovinePoruka'
import { DefaultMetadataTitle } from '@/app/constants'
import { Stack, Typography } from '@mui/material'
import { CustomHead } from '@/widgets/CustomHead'
import { ApiBase, fetchApi } from '@/app/api'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'

const Proizvodi = (): JSX.Element => {
    const router = useRouter()

    const [currentGroup, setCurrentGroup] = useState<any>(null)

    useEffect(() => {
        if (
            router.query.grupa == null ||
            router.query.grupa == undefined ||
            router.query.grupa.length === 0
        ) {
            setCurrentGroup(null)
            return
        }
        fetchApi(ApiBase.Main, `/products-groups/${router.query.grupa}`).then(
            (response) =>
                response.json().then((data: any) => setCurrentGroup(data))
        )
    }, [router.query.grupa])
    return (
        <CenteredContentWrapper>
            <CustomHead />
            <Stack width={'100%'} direction={'column'}>
                {/* Used for SEO purposes */}
                <Typography hidden variant={'h6'} component={`h1`}>
                    Termodom Web Prodavnica
                </Typography>
                <Typography hidden variant={'h6'} component={`h2`}>
                    {DefaultMetadataTitle}
                </Typography>

                <ProizvodiFilter currentGroup={currentGroup} />
                <ModKupovinePoruka />
                <ProizvodiSearch />
                <ProizvodiList currentGroup={currentGroup} />
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Proizvodi
