import { Stack, Switch, TextField, Typography } from '@mui/material'
import { useEffect, useState } from 'react'

export const QuizEditDuration = ({
    defaultQuestionDuration,
    duration,
    onDurationChange,
}) => {
    const handleChange = (e) => setValue(e.target.value)
    const [value, setValue] = useState(duration)
    const [state, setState] = useState(!!duration)
    useEffect(() => {
        onDurationChange(state ? parseInt(value) : null)
    }, [state, value])
    return (
        <Stack
            direction={`row`}
            alignItems={`center`}
            justifyContent={`end`}
            gap={2}
            px={2}
        >
            <Typography component={`label`}>
                Defaultno vreme ({defaultQuestionDuration}s)
            </Typography>
            <Switch
                checked={state}
                onChange={(e) => {
                    setState(e.target.checked)
                }}
            />
            <Typography component={`label`}>Custom vreme</Typography>
            {state === true && (
                <TextField
                    size={`small`}
                    defaultValue={value}
                    onChange={handleChange}
                    onKeyDown={(event) => {
                        const allowedKeys = [
                            '0',
                            '1',
                            '2',
                            '3',
                            '4',
                            '5',
                            '6',
                            '7',
                            '8',
                            '9',
                            '.',
                            ',',
                            'Backspace',
                            'Delete',
                            'ArrowLeft',
                            'ArrowRight',
                        ]
                        if (!allowedKeys.includes(event.key))
                            event.preventDefault()
                    }}
                />
            )}
        </Stack>
    )
}
