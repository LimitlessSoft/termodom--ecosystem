import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { CustomHead } from "@/widgets/CustomHead"
import { ModKupovinePoruka } from "@/widgets/ModKupovinePoruka"
import { ProizvodiFilter } from "@/widgets/Proizvodi/ProizvodiFilter"
import { ProizvodiList } from "@/widgets/Proizvodi/ProizvodiList"
import { ProizvodiSearch } from "@/widgets/Proizvodi/ProizvodiSearch"
import { Stack } from "@mui/material"

const Proizvodi = (): JSX.Element => {
    return (
        <CenteredContentWrapper>
            <CustomHead/>
            <Stack
                width={'100%'}
                direction={'column'}>
                <ProizvodiFilter />
                <ModKupovinePoruka />
                <ProizvodiSearch />
                <ProizvodiList />
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Proizvodi