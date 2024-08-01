import { alpha, styled } from '@mui/material'
import { DataGrid, gridClasses } from '@mui/x-data-grid'

export const StripedDataGrid = styled(DataGrid)(({ theme }) => ({
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
