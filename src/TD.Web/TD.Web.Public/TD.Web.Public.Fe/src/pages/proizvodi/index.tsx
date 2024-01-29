import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { ModKupovinePoruka } from "@/widgets/ModKupovinePoruka"
import { ProizvodiFilter } from "@/widgets/Proizvodi/ProizvodiFilter"
import { ProizvodiList } from "@/widgets/Proizvodi/ProizvodiList"
import { Stack } from "@mui/material"

const Proizvodi = (): JSX.Element => {
    return (
        <CenteredContentWrapper>
            <Stack
                width={'100%'}
                direction={'column'}>
                <ProizvodiFilter />
                <ModKupovinePoruka />
                <ProizvodiList />
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Proizvodi