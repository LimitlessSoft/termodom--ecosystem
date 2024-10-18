import Grid2 from '@mui/material/Unstable_Grid2'
import { Box, Button, IconButton, Typography } from '@mui/material'
import {
    HorizontalActionBar,
    HorizontalActionBarButton,
    ProracunTable,
} from '@/widgets'
import { useRouter } from 'next/router'
import { AddCircle } from '@mui/icons-material'

const ProracunPage = () => {
    const router = useRouter()

    return (
        <Box>
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text="Nazad"
                    onClick={() => router.push(`/korisnici`)}
                />
            </HorizontalActionBar>
            <HorizontalActionBar>
                <IconButton>
                    <AddCircle color={`primary`} fontSize={`large`} />
                </IconButton>
            </HorizontalActionBar>
            <ProracunTable />
        </Box>
    )
}

export default ProracunPage
