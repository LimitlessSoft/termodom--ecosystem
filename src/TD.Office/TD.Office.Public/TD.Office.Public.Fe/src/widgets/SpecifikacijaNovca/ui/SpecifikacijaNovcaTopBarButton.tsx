import { Button, Typography } from '@mui/material'
import { ISpecifikacijaNovcaTopBarButtonProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaTopBarButtonProps'

export const SpecifikacijaNovcaTopBarButton = (
    props: ISpecifikacijaNovcaTopBarButtonProps
) => {
    return (
        <Button variant={`outlined`} startIcon={props.startIcon}>
            {props.text && (
                <Typography sx={props.typographySx}>{props.text}</Typography>
            )}
            {props.children && props.children}
        </Button>
    )
}
