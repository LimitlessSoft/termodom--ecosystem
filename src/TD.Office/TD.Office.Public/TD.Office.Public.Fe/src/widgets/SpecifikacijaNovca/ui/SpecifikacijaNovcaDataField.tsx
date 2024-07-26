import { ISpecifikacijaNovcaDataFieldProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaDataFieldProps'
import { Grid, TextField, Typography } from '@mui/material'
import { SpecifikacijaNovcaDataFieldTextFieldStyled } from '@/widgets/SpecifikacijaNovca/styled/SpecifikacijaNovcaDataFieldTextFieldStyled'
import { useRef } from 'react'

export const SpecifikacijaNovcaDataField = (
    props: ISpecifikacijaNovcaDataFieldProps
) => {
    return (
        <Grid item>
            <SpecifikacijaNovcaDataFieldTextFieldStyled
                fullWidth
                readOnly={props.readOnly}
                disabled={props.readOnly}
                variant={`outlined`}
                // defaultValue={props.defaultValue}
                label={props.label}
                onChange={(event) => {
                    if (props.onChange) {
                        props.onChange(
                            event.target.value == '' ? '0' : event.target.value
                        )
                    }
                }}
                value={props.value}
                multiline={props.multiline}
            />
            {props.subLabel && (
                <Typography textAlign={`right`}> = {props.subLabel}</Typography>
            )}
        </Grid>
    )
}
