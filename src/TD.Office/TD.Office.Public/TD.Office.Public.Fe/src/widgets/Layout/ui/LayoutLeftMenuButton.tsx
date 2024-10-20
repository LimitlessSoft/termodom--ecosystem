import { ILayoutLeftMenuButtonProps } from '../interfaces/ILayoutLeftMenuButtonProps'
import { LayoutLeftMenuButtonStyled } from '../styled/LayoutLeftMenuButtonStyled'
import { Tooltip } from '@mui/material'

export const LayoutLeftMenuButton = (props: ILayoutLeftMenuButtonProps) => {
    const { children } = props
    return (
        <Tooltip title={props.tooltip} placement={`right`}>
            <LayoutLeftMenuButtonStyled
                item
                onClick={() => {
                    if (props.onClick) {
                        props.onClick()
                    }
                }}
            >
                {children}
            </LayoutLeftMenuButtonStyled>
        </Tooltip>
    )
}
