import { IClickable } from "@/interfaces/IClickable"
import { MobileHeaderNotchStyled } from "./MobileHeaderNotchStyled"

export const MobileHeaderNotch = (props: IClickable): JSX.Element => {
    return (
        <MobileHeaderNotchStyled
            onClick={() => {
                props.onClick()
            }}>
            <span></span>
            <span></span>
            <span></span>
        </MobileHeaderNotchStyled>
    )
}