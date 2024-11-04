import { Button, styled } from '@mui/material'

export const SubModuleButtonStyled = styled(Button)(({ theme, props }) => ({
    '&.Mui-disabled': {
        borderColor: props.currentlyActive && theme.palette.primary.light,
        color: props.currentlyActive && theme.palette.primary.light,
        pointerEvents: props.noPermission && 'auto',
        cursor: props.noPermission && 'not-allowed',
    },
}))
