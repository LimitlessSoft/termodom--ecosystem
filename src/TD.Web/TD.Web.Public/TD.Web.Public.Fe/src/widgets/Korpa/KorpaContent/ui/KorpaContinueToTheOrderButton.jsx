import { ArrowForward } from '@mui/icons-material'
import { Button } from '@mui/material'
import NextLink from 'next/link'

const KorpaContinueToTheOrderButton = ({ sx }) => {
    return (
        <Button
            variant="contained"
            color="success"
            fullWidth
            component={NextLink}
            href={`/zavrsi-porudzbinu`}
            endIcon={<ArrowForward />}
            sx={sx}
        >
            Dovrši porudžbinu
        </Button>
    )
}

export default KorpaContinueToTheOrderButton
