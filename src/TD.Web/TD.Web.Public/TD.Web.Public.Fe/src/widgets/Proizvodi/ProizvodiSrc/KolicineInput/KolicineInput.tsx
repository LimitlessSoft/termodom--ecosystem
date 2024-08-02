import { KolicinaInput } from '@/widgets/KolicinaInput'
import { Grid } from '@mui/material'

export const KolicineInput = (props: any): JSX.Element => {
    return (
        <Grid
            container
            spacing={1}
            justifyContent={`center`}
            sx={{ width: '100%', py: 2 }}
        >
            <InnerKolicinaInput
                onValueChange={(val: number) => {
                    props.onBaseKolicinaValueChange(parseFloat(val.toFixed(3)))
                }}
                value={props.baseKolicina}
                unit={props.altUnit == null ? props.baseUnit : props.altUnit}
                onPlusClick={() => {
                    props.onBaseKolicinaValueChange(
                        Math.round(props.baseKolicina + 1)
                    )
                }}
                onMinusClick={() => {
                    if (props.baseKolicina <= 1) return
                    props.onBaseKolicinaValueChange(
                        Math.round(props.baseKolicina - 1)
                    )
                }}
            />
            {props.altUnit == null ? null : (
                <InnerKolicinaInput
                    onValueChange={(val: number) => {
                        props.onBaseKolicinaValueChange(
                            val / props.oneAlternatePackageEquals
                        )
                    }}
                    value={props.altKolicina}
                    unit={props.baseUnit}
                    onPlusClick={() => {
                        props.onBaseKolicinaValueChange(
                            Math.round(props.baseKolicina + 1)
                        )
                    }}
                    onMinusClick={() => {
                        if (props.baseKolicina <= 1) return

                        props.onBaseKolicinaValueChange(
                            Math.round(props.baseKolicina - 1)
                        )
                    }}
                />
            )}
        </Grid>
    )
}

const InnerKolicinaInput = (props: any): JSX.Element => {
    return (
        <Grid item sm={6}>
            <KolicinaInput
                onPlusClick={props.onPlusClick}
                onMinusClick={props.onMinusClick}
                value={props.value}
                unit={props.unit}
                onValueChange={props.onValueChange}
            />
        </Grid>
    )
}
