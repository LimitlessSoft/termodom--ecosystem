import { alpha, Stack, styled, Typography } from '@mui/material'
import { DataGrid, gridClasses } from '@mui/x-data-grid'

const StripedDataGridStyled = styled(DataGrid)(({ theme }) => ({
    [`& .${gridClasses.row}.even`]: {
        backgroundColor: theme.dataBackground?.primary,
        '&:hover, &mui-hovered': {
            backgroundColor: theme.dataBackground?.primaryHover,
            cursor: 'pointer',
        },
    },
    [`& .${gridClasses.row}.odd`]: {
        backgroundColor: theme.dataBackground?.secondary,
        '&:hover, &mui-hovered': {
            backgroundColor: theme.dataBackground?.secondaryHover,
            cursor: 'pointer',
        },
    },
}))

const NoRowsOverlay = ({ message }) => (
    <Stack alignItems={`center`} justifyContent={`center`} height={`100%`}>
        <Typography>{message}</Typography>
    </Stack>
)

export const StripedDataGrid = ({
    noRowsMessage = 'Nema redova za prikazivanje',
    ...props
}) => (
    <StripedDataGridStyled
        slots={{
            noRowsOverlay: NoRowsOverlay,
        }}
        slotProps={{
            noRowsOverlay: { message: noRowsMessage },
        }}
        {...props}
    />
)
