import { IDividerProps } from "../models/IDividerProps"
import { CircularProgress, Typography } from "@mui/material"
import { DividerStyled } from "./DividerStyled"

export const Divider = (props: IDividerProps): JSX.Element => {
    return (
        <DividerStyled user={props.user}>
            {
                props.user.isLoading ?
                    <CircularProgress color={`primary`} /> :
                    <Typography
                        style={{
                            fontFamily: 'GothamProMedium'
                        }}>
                        { props.user.isLogged ? props.user.data?.nickname : "Jednokratna kupovina" }
                    </Typography>
            }
        </DividerStyled>
    )
}