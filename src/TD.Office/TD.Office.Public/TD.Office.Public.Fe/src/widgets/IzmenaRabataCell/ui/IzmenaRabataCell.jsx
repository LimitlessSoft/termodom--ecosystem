import { useState } from 'react'
import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Stack, TextField } from '@mui/material'
import { formatNumber } from '../../../helpers/numberHelpers'

export const IzmenaRabataCell = ({ onChange, rabat, disabled }) => {
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [value, setValue] = useState(rabat)

    return (
        <Stack direction={`row`} gap={1}>
            <Dialog
                open={isDialogOpen}
                onClose={() => {
                    setIsDialogOpen(false)
                }}
            >
                <DialogTitle>Izmena rabata</DialogTitle>
                <DialogContent>
                    <Box p={2}>
                        <TextField
                            label={`Rabat`}
                            value={value}
                            onChange={(e) => {
                                setValue(e.target.value)
                            }}
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            onChange(value)
                        }}
                    >
                        Sacuvaj
                    </Button>
                    <Button
                        variant={`outlined`}
                        onClick={() => {
                            setIsDialogOpen(false)
                            setValue(rabat)
                        }}
                    >
                        Odustani
                    </Button>
                </DialogActions>
            </Dialog>
            <Button
                disabled={disabled}
                variant={`outlined`}
                onClick={() => {
                    setIsDialogOpen(true)
                }}
            >
                {formatNumber(rabat)}%
            </Button>
        </Stack>
    )
}