import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { ProizvodiFilter } from "@/widgets/Proizvodi/ProizvodiFilter"
import { ProizvodiList } from "@/widgets/Proizvodi/ProizvodiList"
import { Stack } from "@mui/material"

const Proizvodi = (): JSX.Element => {
    return (
        <CenteredContentWrapper>
            <Stack
                direction={'column'}>
                <ProizvodiFilter />
                <ProizvodiList />
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Proizvodi