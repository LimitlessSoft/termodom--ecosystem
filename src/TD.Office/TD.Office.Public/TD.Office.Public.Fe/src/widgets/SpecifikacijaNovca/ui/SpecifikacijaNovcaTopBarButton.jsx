import { Button, Typography } from '@mui/material'

export const SpecifikacijaNovcaTopBarButton = (props) => {
    return (
        <Button
            variant={`${props.isToggled ? 'contained' : 'outlined'}`}
            startIcon={props.startIcon}
            onClick={props.onClick}
            disabled={props.disabled}
            href={props.href}
            target={props.target}
        >
            {props.text && (
                <Typography sx={props.typographySx}>{props.text}</Typography>
            )}
            {props.children && props.children}
        </Button>
    )
}
