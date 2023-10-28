import { ProizvodiFilter } from "@/widgets/Proizvodi/ProizvodiFilter"
import { ProizvodiList } from "@/widgets/Proizvodi/ProizvodiList"
import { Box, Stack } from "@mui/material"

const Proizvodi = (): JSX.Element => {
    return (
        <Box
            sx={{
                maxWidth: '1100px',
                minHeight: '100vh',
                margin: 'auto' }}>
            <Stack
                direction={'column'}>
                    <ProizvodiFilter />
                    <ProizvodiList />
                </Stack>
        </Box>
    )
}

export default Proizvodi