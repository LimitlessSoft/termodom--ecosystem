import { TextField, Button, Box, IconButton } from '@mui/material'
import { useState } from 'react'
import { Save, SaveAs, SaveAsTwoTone, SaveRounded } from '@mui/icons-material'

export default function KomentarInput({ label, defaultValue, onSave }) {
    const [initialValue, setInitialValue] = useState(defaultValue || '')
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
            {initialValue !== comment && (
                <Box position={`absolute`} top={0} right={0}>
                    <IconButton
                        onClick={() => {
                            onSave(comment).then(() => {
                                setInitialValue(comment)
                            })
                        }}
                    >
                        <SaveRounded color={`success`} />
                    </IconButton>
                </Box>
            )}
        </Box>
    )
}
