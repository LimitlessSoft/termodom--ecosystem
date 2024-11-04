import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Stack,
    TextField,
} from '@mui/material'
import { formatNumber } from '../../../../helpers/numberHelpers'
import { useState } from 'react'
import { handleApiError, officeApi } from '../../../../apis/officeApi'

export const ProracunKolicinaCell = (props) => {
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [value, setValue] = useState(props.kolicina)

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
                            officeApi
                                .put(
                                    `proracuni/${props.proracunId}/items/${props.stavkaId}/kolicina`,
                                    {
                                        kolicina: value,
                                    }
                                )
                                .then(() => {
                                    props.onKolicinaSaved(props.stavkaId, value)
                                })
                                .catch(handleApiError)
                                .finally(() => {
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
                            setValue(props.kolicina)
                        }}
                    >
                        Odustani
                    </Button>
                </DialogActions>
            </Dialog>
            <Button
                disabled={props.disabled}
                variant={`outlined`}
                onClick={() => {
                    setIsDialogOpen(true)
                }}
            >
                {formatNumber(props.kolicina)}
            </Button>
        </Stack>
    )
}
