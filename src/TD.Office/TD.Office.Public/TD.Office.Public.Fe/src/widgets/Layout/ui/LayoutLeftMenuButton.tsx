import { ILayoutLeftMenuButtonProps } from "../interfaces/ILayoutLeftMenuButtonProps";
import { LayoutLeftMenuButtonStyled } from "../styled/LayoutLeftMenuButtonStyled";

export const LayoutLeftMenuButton = (props: ILayoutLeftMenuButtonProps): JSX.Element => {
    const { children } = props
    return (
        <LayoutLeftMenuButtonStyled item onClick={() => {
            if (props.onClick) {
                props.onClick()
            }
        }}>
            { children }
        </LayoutLeftMenuButtonStyled>
    )
}