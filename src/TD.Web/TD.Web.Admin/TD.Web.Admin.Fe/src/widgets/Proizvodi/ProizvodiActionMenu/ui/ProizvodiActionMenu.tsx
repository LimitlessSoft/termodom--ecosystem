import { AddCircle } from "@mui/icons-material"
import { Box, Button, Typography } from "@mui/material"
import NextLink from 'next/link'

export const ProizvodiActionMenu = (): JSX.Element => {
    return (
        <Box
        sx={{ m: 2 }}>
            {
                <Button
                variant={`contained`}
                LinkComponent={NextLink}
                href="/proizvodi/novi"
                startIcon={<AddCircle />}
                >
                    <Typography>
                        Novi proizvod
                    </Typography>
                </Button>
            }
        </Box>
    )
}