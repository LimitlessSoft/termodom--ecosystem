import { ManageAccounts } from '@mui/icons-material'
import { Grid, IconButton, Typography } from '@mui/material'
import NextLink from 'next/link'
import React from 'react'

export default function UsersListItem({ data, index }) {
    const bgColor = index % 2 === 0 ? `#f0f0f0` : `#e0e0e0`

    return (
        <Grid
            container
            sx={{
                width: 300,
                backgroundColor: bgColor,
                py: 1,
                px: 2,
            }}
            justifyContent={`space-between`}
            alignItems={`center`}
        >
            <Typography>
                [{data.id}] {data.username} ({data.type})
            </Typography>
            <IconButton
                LinkComponent={NextLink}
                href={`/admin/users/${data.id}`}
            >
                <ManageAccounts />
            </IconButton>
        </Grid>
    )
}
