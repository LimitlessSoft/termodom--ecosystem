import colorConstants from '@/constants/colorConstants'
import { Box, Grid, Paper, Stack, Typography } from '@mui/material'
import React from 'react'

const legendOptions = [
    {
        text: 'Tačan odgovor',
        borderColor: colorConstants.CORRECT_ANSWER_BORDER_COLOR,
    },
    {
        text: 'Netačan odgovor',
        borderColor: colorConstants.INCORRECT_ANSWER_BORDER_COLOR,
    },
    {
        text: 'Korisnikov izabran odgovor',
        backgroundColor:
            colorConstants.ADMIN_PREVIEW_USER_SELECTED_ANSWER_BACKGROUND_COLOR,
    },
    {
        text: 'Neizabran odgovor',
        backgroundColor:
            colorConstants.ADMIN_PREVIEW_NOT_SELECTED_ANSWER_BACKGROUND_COLOR,
    },
]

export default function QuizzSummaryLegend() {
    return (
        <Box sx={{ py: 20 }}>
            <Paper sx={{ scale: 2, padding: 2, borderRadius: 2 }}>
                <Stack spacing={1}>
                    <Typography variant="h6">Legenda odgovora</Typography>
                    <Stack>
                        {legendOptions.map((option, index) => (
                            <Grid
                                key={index}
                                container
                                alignItems="center"
                                gap={1}
                            >
                                <Box
                                    sx={{
                                        border: option.borderColor
                                            ? `1px solid ${option.borderColor}`
                                            : null,
                                        backgroundColor:
                                            option.backgroundColor ?? 'white',
                                        width: 20,
                                        height: 20,
                                        borderRadius: 1,
                                    }}
                                />
                                <Typography>{option.text}</Typography>
                            </Grid>
                        ))}
                    </Stack>
                </Stack>
            </Paper>
        </Box>
    )
}
