import { Button, Typography } from '@mui/material'
import { ISpecifikacijaNovcaTopBarButtonProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaTopBarButtonProps'

export const SpecifikacijaNovcaTopBarButton = (
    props: ISpecifikacijaNovcaTopBarButtonProps
) => {
    return (
        <Button
            variant={`${props.isSelected ? 'contained' : 'outlined'}`}
            startIcon={props.startIcon}
            onClick={props.onClick}
        >
            {props.text && (
                <Typography sx={props.typographySx}>{props.text}</Typography>
            )}
            {props.children && props.children}
        </Button>
    )
}
