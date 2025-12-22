import { AddCircle } from '@mui/icons-material'
import { Box, Button, Typography } from '@mui/material'
import NextLink from 'next/link'

export const BlogoviActionMenu = () => {
    return (
        <Box sx={{ m: 2 }}>
            <Button
                variant="contained"
                LinkComponent={NextLink}
                href="/blogovi/novi"
                startIcon={<AddCircle />}
            >
                <Typography>Novi blog</Typography>
            </Button>
        </Box>
    )
}
