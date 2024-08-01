import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { Comment } from '@mui/icons-material'
import { Button, Fade, Grid, Grow, Slide, Zoom } from '@mui/material'
import { useState } from 'react'
import { SpecifikacijaNovcaOstaloCommentFieldStyled } from '../styled/SpecifikacijaNovcaOstaloCommentFieldStyled'
import { ISpecifikacijaNovcaOstaloCommentButtonProps } from '../interfaces/ISpecifikacijaNovcaOstaloCommentButtonProps'

export const SpecifikacijaNovcaOstaloCommentButton = ({
    comment,
}: ISpecifikacijaNovcaOstaloCommentButtonProps) => {
    const [isCommentShown, setIsCommentShown] = useState(false)

    return (
        <Grid item position={`relative`}>
            <Fade in={isCommentShown} timeout={300} unmountOnExit>
                <SpecifikacijaNovcaOstaloCommentFieldStyled>
                    <EnchantedTextField textAlignment="left" value={comment} />
                </SpecifikacijaNovcaOstaloCommentFieldStyled>
            </Fade>
            <Button
                variant={`contained`}
                onClick={() => setIsCommentShown((prevState) => !prevState)}
            >
                <Comment />
            </Button>
        </Grid>
    )
}
