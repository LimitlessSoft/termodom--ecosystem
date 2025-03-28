import { formatNumber } from '@/app/helpers/numberHelpers'
import { mainTheme } from '@/app/theme'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { Paper } from '@mui/material'

export const KorpaSummary = (props) => {
    const lightColor = `#777`

    return (
        <Paper
            sx={{
                p: 2,
                textAlign: `right`,
                boxShadow: {
                    xs: 8,
                    md: 1,
                },
                border: {
                    xs: '1px solid gray',
                    md: 'none',
                },
            }}
        >
            <ResponsiveTypography
                sx={{
                    color: lightColor,
                }}
                scale={1.3}
            >
                Ukupno: {formatNumber(props.cart.summary.valueWithoutVAT)} RSD
            </ResponsiveTypography>
            <ResponsiveTypography
                sx={{
                    color: lightColor,
                }}
                scale={1.3}
            >
                PDV: {formatNumber(props.cart.summary.vatValue)} RSD
            </ResponsiveTypography>
            <ResponsiveTypography scale={1.3}>
                Za uplatu: {formatNumber(props.cart.summary.valueWithVAT)} RSD
            </ResponsiveTypography>
            <ResponsiveTypography
                sx={{
                    color: mainTheme.palette.success.main,
                    fontFamily: 'GothamProMedium',
                }}
                scale={1.4}
            >
                Ušteda: {formatNumber(props.cart.summary.discountValue)} RSD
            </ResponsiveTypography>
        </Paper>
    )
}
