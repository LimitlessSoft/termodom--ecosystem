import { DefaultMetadataDescription, DefaultMetadataTitle } from "@/app/constants"
import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { CustomHead } from "@/widgets/CustomHead"
import { ModKupovinePoruka } from "@/widgets/ModKupovinePoruka"
import { ProizvodiFilter } from "@/widgets/Proizvodi/ProizvodiFilter"
import { ProizvodiList } from "@/widgets/Proizvodi/ProizvodiList"
import { ProizvodiSearch } from "@/widgets/Proizvodi/ProizvodiSearch"
import { Stack, Typography } from "@mui/material"
import { useRouter } from "next/router"

const Proizvodi = (): JSX.Element => {
    return (
        <CenteredContentWrapper>
            <CustomHead/>
            <Stack
                width={'100%'}
                direction={'column'}>
                
                {/* Used for SEO purposes */}
                <Typography hidden variant={'h6'} component={`h1`}>Termodom Web Prodavnica</Typography>
                <Typography hidden variant={'h6'} component={`h2`}>{DefaultMetadataTitle}</Typography>
                
                <ProizvodiFilter />
                <ModKupovinePoruka />
                <ProizvodiSearch />
                <ProizvodiList />
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Proizvodi