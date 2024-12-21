import { DataGrid, gridClasses } from '@mui/x-data-grid'
import { Stack, Typography, styled } from '@mui/material'

const StripedDataGridStyled = styled(DataGrid)(({ theme }) => ({
    [`& .${gridClasses.row}.even`]: {
        backgroundColor: theme.palette.action.hover,
        '&:hover, &.Mui-hovered': {
            backgroundColor: theme.palette.action.selected,
        },
    },
    [`& .${gridClasses.row}.odd`]: {
        backgroundColor: theme.palette.background.paper,
        '&:hover, &.Mui-hovered': {
            backgroundColor: theme.palette.action.hover,
        },
    },
}))

const NoRowsOverlay = ({ message }) => (
    <Stack alignItems="center" justifyContent="center" height="100%">
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
        getRowClassName={(params) =>
            params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd'
        }
        {...props}
    />
)
