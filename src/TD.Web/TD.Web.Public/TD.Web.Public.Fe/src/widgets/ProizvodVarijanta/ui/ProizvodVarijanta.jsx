import { Box, Button, Typography } from '@mui/material'
import NextLink from 'next/link'
import { useZOverlay } from '@/zStore'

export const ProizvodVarijanta = ({ text, href, current }) => {
    const zOverlay = useZOverlay()
    if (current) {
        return (
            <Box
                sx={{
                    borderRadius: `16px`,
                    textAlign: `center`,
                    backgroundColor: `primary.main`,
                    color: `primary.contrastText`,
                    padding: `8px 16px`,
                }}
            >
                <Typography sx={{ userSelect: 'none' }}>{text}</Typography>
            </Box>
        )
    }
    return (
        <Button
            href={`/proizvodi/${href}`}
            onClick={() => {
                zOverlay.show()
            }}
            LinkComponent={NextLink}
            variant={`outlined`}
            sx={{
                borderRadius: `16px`,
                textAlign: `center`,
            }}
        >
            <Typography>{text}</Typography>
        </Button>
    )
}
