import { TextField } from '@mui/material'
import React from 'react'

const defaultAllowedKeys = [
    'Backspace',
    'Delete',
    'ArrowLeft',
    'ArrowRight',
    'Tab',
]

export default function NumberInput({
    label,
    disabled,
    onChange,
    onKeyDown,
    value,
    sx,
    additionalAllowedKeys = [],
}) {
    const allowedKeys = [...defaultAllowedKeys, ...additionalAllowedKeys]

    const handleKeyDown = (e) => {
        if (!/^[0-9]$/.test(e.key) && !allowedKeys.includes(e.key)) {
            e.preventDefault()
        }
        if (onKeyDown) onKeyDown(e)
    }

    return (
        <TextField
            type="number"
            label={label}
            disabled={disabled}
            onChange={onChange}
            onKeyDown={handleKeyDown}
            value={value}
            sx={{
                '& input[type=number]::-webkit-outer-spin-button': {
                    WebkitAppearance: 'none',
                    margin: 0,
                },
                '& input[type=number]::-webkit-inner-spin-button': {
                    WebkitAppearance: 'none',
                    margin: 0,
                },
                '& input[type=number]': {
                    MozAppearance: 'textfield',
                },
                ...sx,
            }}
        />
    )
}
