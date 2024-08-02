import { IPodesavanjaTitleProps } from '@/widgets/Podesavanja/interfaces/IPodesavanjaTitleProps'
import { Box, Typography } from '@mui/material'

export const PodesavanjaTitle = ({ title }: IPodesavanjaTitleProps) => {
    return (
        <Box>
            <Typography sx={{ m: 2 }} variant="h6">
                {title}
            </Typography>
        </Box>
    )
}
