import { LayoutLeftMenuButtonStyled } from '../styled/LayoutLeftMenuButtonStyled'

function LayoutLeftMenuButton({ onClick, children }) {
    return (
        <LayoutLeftMenuButtonStyled
            onClick={() => {
                if (onClick) onClick()
            }}
        >
            {children}
        </LayoutLeftMenuButtonStyled>
    )
}

export default LayoutLeftMenuButton
