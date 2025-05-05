import { KolicinaInput } from '@/widgets/KolicinaInput'
import { Grid } from '@mui/material'

export const KolicineInput = ({
    baseQuantity,
    alternateQuantity,
    oneAlternatePackageEquals,
    onQuantityChange,
    alternateUnit,
    baseUnit,
    disabled,
}) => {
    return (
        <Grid
            container
            spacing={1}
            justifyContent={`center`}
            sx={{ width: '100%', py: 2 }}
        >
            <InnerKolicinaInput
                disabled={disabled}
                onValueChange={(value) =>
                    onQuantityChange(parseFloat(value.toFixed(3)))
                }
                value={baseQuantity}
                unit={alternateUnit || baseUnit}
                onPlusClick={() =>
                    onQuantityChange(Math.round(baseQuantity + 1))
                }
                onMinusClick={() => {
                    if (baseQuantity <= 1) return
                    onQuantityChange(Math.round(baseQuantity - 1))
                }}
            />
            {alternateUnit && (
                <InnerKolicinaInput
                    disabled={disabled}
                    onValueChange={(value) =>
                        onQuantityChange(value / oneAlternatePackageEquals)
                    }
                    value={alternateQuantity}
                    unit={baseUnit}
                    onPlusClick={() =>
                        onQuantityChange(Math.round(baseQuantity + 1))
                    }
                    onMinusClick={() => {
                        if (baseQuantity <= 1) return

                        onQuantityChange(Math.round(baseQuantity - 1))
                    }}
                />
            )}
        </Grid>
    )
}

const InnerKolicinaInput = (props) => {
    return (
        <Grid item sm={6}>
            <KolicinaInput
                disabled={props.disabled}
                onPlusClick={props.onPlusClick}
                onMinusClick={props.onMinusClick}
                value={props.value}
                unit={props.unit}
                onValueChange={props.onValueChange}
            />
        </Grid>
    )
}
