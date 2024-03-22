import { TableCell, TableRow } from "@mui/material"
import { KorisniciListRowStyled } from "./KorisniciListRowStyled"

const mapUserTypeToColor = (userType: string): string => {
    switch (userType) {
        case `Korisnik`:
            return `green`
        case `Admin`:
            return `yellow`
        case `Super Admin`:
            return `red`
        default:
            return `white`
    }
}

export const KorisniciListRow = (props: any): JSX.Element => {


    return (
        <KorisniciListRowStyled
            key={props.user.id}
            onClick={() => {
                props.onClick(props.user.username)
            }}>
            <TableCell align="center" width={props.userTypeColWidth}
                style={{
                    backgroundColor: mapUserTypeToColor(props.user.userType)
                }}></TableCell>
            <TableCell align="center">{props.user.id}</TableCell>
            <TableCell align="center">{props.user.nickname}</TableCell>
            <TableCell align="center">{props.user.username} - {props.user.isActive}</TableCell>
            <TableCell align="center">{props.user.mobile}</TableCell>
        </KorisniciListRowStyled>
    )
}