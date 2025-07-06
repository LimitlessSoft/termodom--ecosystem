import { Grid, Typography } from '@mui/material'
import { SpecifikacijaNovcaDataFieldTextFieldStyled } from '@/widgets/SpecifikacijaNovca/styled/SpecifikacijaNovcaDataFieldTextFieldStyled'
import { formatNumber } from '@/helpers/numberHelpers'

export const SpecifikacijaNovcaDataField = (props) => {
    return (
        <Grid item>
            <SpecifikacijaNovcaDataFieldTextFieldStyled
                fullWidth
                readOnly={props.readOnly}
                disabled={props.readOnly || props.disabled}
                variant={`outlined`}
                label={props.label}
                onKeyDown={(event) => {
                    if (!props.multiline) {
                        const allowedKeys = [
                            '0',
                            '1',
                            '2',
                            '3',
                            '4',
                            '5',
                            '6',
                            '7',
                            '8',
                            '9',
                            '.',
                            ',',
                            'Backspace',
                        ]
                        if (!allowedKeys.includes(event.key))
                            event.preventDefault()
                    }
                }}
                onChange={(event) => {
                    if (props.onChange) {
                        props.onChange(
                            event.target.value == '' && !props.multiline
                                ? '0'
                                : event.target.value
                        )
                    }
                }}
                value={props.value}
                multiline={props.multiline}
            />
            {props.subLabel && (
                <Typography textAlign={`right`}>
                    = {formatNumber(+props.subLabel)}
                </Typography>
            )}
        </Grid>
    )
}
