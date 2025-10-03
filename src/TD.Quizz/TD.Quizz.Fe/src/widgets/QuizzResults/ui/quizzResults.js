'use client'
import {
    CircularProgress,
    Divider,
    LinearProgress,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import { QuizzResultItem } from '@/widgets/QuizzResults/ui/quizzResultItem'
import { NewQuizz } from '@/widgets/NewQuizz/ui/newQuizz'
import { useEffect, useState } from 'react'
import { handleResponse } from '@/helpers/responseHelpers'

const getPoints = (result) => {
    if (!result) throw new Error(`Result is required`)
    if (!result.answers) throw new Error(`Result answers are required`)
    return result.answers.filter((a) => a.isCorrect).length
}

export const QuizzResults = () => {
    const [data, setData] = useState(null)
    useEffect(() => {
        fetch(`/api/admin-results`).then((response) => {
            handleResponse(response, (data) => {
                setData(data)
            })
        })
    }, [])

    return (
        <Paper>
            <Stack
                alignItems={`center`}
                justifyContent={`center`}
                sx={{
                    borderRadius: 2,
                    overflow: `hidden`,
                    p: 2,
                }}
            >
                <Typography variant={`h6`}>Rezultati</Typography>
                <Divider sx={{ width: `100%`, mb: 2 }} />
                {!data && <CircularProgress />}
                {data &&
                    (data.length === 0 ? (
                        <Typography variant={`subtitle1`}>
                            Nema rezultata
                        </Typography>
                    ) : (
                        data.map((result, index) => (
                            <QuizzResultItem
                                key={result.id}
                                id={result.id}
                                index={index}
                                title={result.quizzSchemaName}
                                user={result.user || `N/A`}
                                completedAt={result.completed_at}
                                createdAt={result.created_at}
                                points={getPoints(result)}
                                maxPoints={result.answers.length}
                            />
                        ))
                    ))}
            </Stack>
        </Paper>
    )
}
