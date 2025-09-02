import { handleResponse } from '@/helpers/responseHelpers'
import { FileOpen } from '@mui/icons-material'
import {
    Box,
    CircularProgress,
    Divider,
    Grid,
    IconButton,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import moment from 'moment'
import { useEffect, useState } from 'react'
import NextLink from 'next/link'

export default function UserQuizzesResults({ userId }) {
    const [results, setResults] = useState()

    useEffect(() => {
        fetch(`/api/admin/users/${userId}/results`).then((response) => {
            handleResponse(response, (data) => setResults(data))
        })
    }, [])

    return (
        <Paper sx={{ p: 2 }}>
            <Typography variant={`h6`}>Rezultati korisnika</Typography>
            <Divider sx={{ mb: 2 }} />
            <Stack spacing={1}>
                {results ? (
                    results.map((result, index) => (
                        <Grid
                            key={result.id}
                            container
                            justifyContent={`space-between`}
                            alignItems={`center`}
                            gap={2}
                            sx={{
                                bgcolor:
                                    index % 2 === 0 ? `#f0f0f0` : `#e0e0e0`,
                                borderRadius: 2,
                                py: 1,
                                px: 2,
                            }}
                        >
                            <Box>
                                <Typography variant={`subtitle1`}>
                                    {result.quizzSchemaName}
                                </Typography>
                                <Typography color={`#888`}>
                                    {
                                        result.answers.filter(
                                            (a) => a.isCorrect
                                        ).length
                                    }{' '}
                                    / {result.answers.length}
                                </Typography>
                                <Typography color={`#888`}>
                                    {moment(result.completedAt).format(
                                        'DD.MM.YYYY HH:mm:ss'
                                    )}
                                </Typography>
                            </Box>
                            <IconButton
                                href={`/${result.id}`}
                                LinkComponent={NextLink}
                                target={`_blank`}
                            >
                                <FileOpen />
                            </IconButton>
                        </Grid>
                    ))
                ) : (
                    <CircularProgress />
                )}
            </Stack>
        </Paper>
    )
}
