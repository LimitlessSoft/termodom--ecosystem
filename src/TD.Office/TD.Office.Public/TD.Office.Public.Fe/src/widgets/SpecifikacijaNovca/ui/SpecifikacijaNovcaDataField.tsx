import { ISpecifikacijaNovcaDataFieldProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaDataFieldProps'
import { Grid, TextField, Typography } from '@mui/material'
import { SpecifikacijaNovcaDataFieldTextFieldStyled } from '@/widgets/SpecifikacijaNovca/styled/SpecifikacijaNovcaDataFieldTextFieldStyled'

export const SpecifikacijaNovcaDataField = (
    props: ISpecifikacijaNovcaDataFieldProps
) => {
    return (
        <Grid item>
            <SpecifikacijaNovcaDataFieldTextFieldStyled
                readonly={props.readonly}
                disabled={props.readonly}
                variant={`outlined`}
                defaultValue={props.defaultValue}
                label={props.label}
                onChange={(val) => {
                    if (props.onChange) {
                        props.onChange(val.target.value)
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
