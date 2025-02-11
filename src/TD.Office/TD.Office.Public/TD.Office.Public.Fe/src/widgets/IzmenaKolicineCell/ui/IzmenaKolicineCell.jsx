import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Stack, TextField } from '@mui/material'
import { formatNumber } from '../../../helpers/numberHelpers'
import { useState } from 'react'

export const IzmenaKolicineCell = ({ onChange, kolicina, disabled }) => {
    
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [value, setValue] = useState(kolicina)
    
    return (
        <Stack direction={`row`} gap={1}>
            <Dialog
                open={isDialogOpen}
                onClose={() => {
                    setIsDialogOpen(false)
                }}
            >
                <DialogTitle>Izmena kolicine</DialogTitle>
                <DialogContent>
                    <Box p={2}>
                        <TextField
                            label={`Kolicina`}
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
                            onChange(value, () => {
                                setIsDialogOpen(false)
                            })
                        }}
                    >
                        Sacuvaj
                    </Button>
                    <Button
                        variant={`outlined`}
                        onClick={() => {
                            setIsDialogOpen(false)
                            setValue(kolicina)
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
                {formatNumber(kolicina)}
            </Button>
        </Stack>
    )
}