import { TextField, Button, Box } from '@mui/material'
import { useState } from 'react'

export default function PorudzbinaKomentar({ label, defaultValue, onSave }) {
    const [comment, setComment] = useState('')

    return (
        <Box position={`relative`}>
            <TextField
                multiline
                rows={4}
                variant={`filled`}
                label={label}
                defaultValue={defaultValue}
                fullWidth
                onChange={(e) => setComment(e.target.value)}
                sx={{
                    height: '100%',
                }}
            />
            <Box position={`absolute`} top={10} right={10}>
                <Button
                    variant={`contained`}
                    sx={{ p: `4px 8px` }}
                    onClick={() => onSave(comment)}
                >
                    Sacuvaj
                </Button>
            </Box>
        </Box>
    )
}
