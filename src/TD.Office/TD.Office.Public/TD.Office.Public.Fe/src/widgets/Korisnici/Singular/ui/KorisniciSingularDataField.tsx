import { Button, CircularProgress, Grid, TextField } from "@mui/material"
import { IKorisniciSingularDataFieldProps } from "../interfaces/IKorisniciSingularDataFieldProps"
import { KorisniciSingularDataFieldStyled } from "../styled/KorisniciSingularDataFieldStyled"
import { Save } from "@mui/icons-material"
import { useState } from "react"

export const KorisniciSingularDataField = (props: IKorisniciSingularDataFieldProps): JSX.Element => {

    const [isUpdating, setIsUpdating] = useState<boolean>(false)
    const [originalValue, setOriginalValue] = useState<string>(props.defaultValue)
    const [value, setValue] = useState<string>(props.defaultValue)

    return (
        <KorisniciSingularDataFieldStyled editable={props.editable}>
            <Grid container direction={`row`} alignItems={`center`} spacing={1}>
                { props.preLabel === undefined ? null : <Grid item>{ props.preLabel }</Grid> }
                <Grid item>
                    <TextField aria-readonly disabled={!props.editable || isUpdating} size={`small`} defaultValue={props.defaultValue}
                        onChange={(e) => {
                            setValue(e.target.value)
                        }}/>
                </Grid>
                {
                    props.editable && originalValue !== value &&
                        <Grid item>
                            <Button disabled={isUpdating} startIcon={isUpdating ? <CircularProgress size={`1em`} /> : <Save />} variant={`contained`} onClick={() => {
                                setIsUpdating(true)
                                if(props.onSave)
                                    props.onSave(value)
                                        .then(() => {
                                            setIsUpdating(false)
                                            setOriginalValue(value)
                                        })
                                        .finally(() => setIsUpdating(false))
                            }}>Sačuvaj</Button>
                        </Grid>
                }
            </Grid>
        </KorisniciSingularDataFieldStyled>
    )
}