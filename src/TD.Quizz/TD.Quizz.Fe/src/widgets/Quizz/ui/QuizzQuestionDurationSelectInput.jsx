import { Stack, Switch, TextField, Typography } from '@mui/material'
import React, { useState } from 'react'

export default function QuizzQuestionDurationSelectInput({
    defaultDuration,
    duration,
    onChangeDuration,
}) {
    const [checked, setChecked] = useState(!duration.isUsingDefault)

    const handleChangeSwitch = (event) => {
        setChecked(event.target.checked)
        onChangeDuration(event.target.checked ? 0 : null)
    }

    const handleChangeCustomDuration = (event) => {
        onChangeDuration(Number(event.target.value) || null)
    }

    return (
        <Stack
            direction="row"
            spacing={1}
            alignItems="center"
            justifyContent="flex-start"
        >
            <Typography>Defaultno vreme ({defaultDuration}s)</Typography>
            <Switch checked={checked} onChange={handleChangeSwitch} />
            <Stack direction="row" spacing={1} alignItems="center">
                <Typography>Custom vreme</Typography>
                {checked && (
                    <TextField
                        label="Vreme u sekundama"
                        size="small"
                        value={duration.value ?? ''}
                        onChange={handleChangeCustomDuration}
                    />
                )}
            </Stack>
        </Stack>
    )
}
