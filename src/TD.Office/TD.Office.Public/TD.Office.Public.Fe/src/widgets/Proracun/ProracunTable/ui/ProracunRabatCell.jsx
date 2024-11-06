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

export const ProracunRabatCell = (props) => {
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [value, setValue] = useState(props.rabat)

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
                            officeApi
                                .put(
                                    `proracuni/${props.proracunId}/items/${props.stavkaId}/rabat`,
                                    {
                                        rabat: value,
                                    }
                                )
                                .then(() => {
                                    props.onRabatSaved(props.stavkaId, value)
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
                            setValue(props.rabat)
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
                {formatNumber(props.rabat)}%
            </Button>
        </Stack>
    )
}
