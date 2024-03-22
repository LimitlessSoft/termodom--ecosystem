import { Typography } from "@mui/material"
import { KorisniciListWithoutReferentItemStyled } from "./KorisniciListWithoutReferentItemStyled"

export const KorisniciListWithoutReferentItem = (props: any) => {
    return (
        <KorisniciListWithoutReferentItemStyled
            onClick={() => {
                props.onClick()
            }}>
            {props.user.nickname} ({props.user.username}) [{props.user.mobile}]
        </KorisniciListWithoutReferentItemStyled>
    )
}