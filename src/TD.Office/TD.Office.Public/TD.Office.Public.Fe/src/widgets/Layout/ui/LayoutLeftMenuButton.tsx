import { Box, Grid, SvgIconTypeMap, styled } from "@mui/material";
import { ReactNode } from "react";

interface ILayoutLeftMenuButtonProps {
    children: ReactNode,
    onClick?: () => void
}

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

const LayoutLeftMenuButtonStyled = styled(Grid)(
    ({ theme }) => `
        transition-duration: 0.3s;

        svg {
            padding: 0.5rem 1rem;
        }

        &:hover {
            background-color: ${theme.palette.primary.dark};
            cursor: pointer;
        }
    `
)