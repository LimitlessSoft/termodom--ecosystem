import {
    Box,
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
} from '@mui/material'
import { toast } from 'react-toastify'
import { useState } from 'react'
import { mainTheme } from '../../../../themes'
import { Comment } from '@mui/icons-material'
import { handleApiError, officeApi } from '../../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../../constants'

export const PartneriKomercijalnoIFinansijskoTableKomentarCell = (param) => {
    const [isOpen, setIsOpen] = useState(false)
    const [comment, setComment] = useState(param.value)
    const [isUpdating, setIsUpdating] = useState(false)

    return (
        <Box>
            <Dialog
                open={isOpen === true}
                onClose={() => {
                    setIsOpen(false)
                }}
            >
                <DialogTitle>Komentar</DialogTitle>
                <DialogContent>
                    <TextField
                        multiline
                        rows={10}
                        value={comment}
                        onChange={(e) => {
                            setComment(e.target.value)
                        }}
                    />
                </DialogContent>
                <DialogActions>
                    <Button
                        startIcon={isUpdating && <CircularProgress />}
                        disabled={isUpdating}
                        variant={'contained'}
                        onClick={() => {
                            setIsUpdating(true)

                            officeApi
                                .put(
                                    ENDPOINTS_CONSTANTS.PARTNERS.PUT_KOMERCIJALNO_I_FINANSIJSKO_DATA_KOMENTAR(
                                        param.id
                                    ),
                                    {
                                        id: param.id,
                                        komentar: comment,
                                    }
                                )
                                .then((response) => {
                                    toast.success('Uspešno sačuvano')
                                })
                                .catch(handleApiError)
                                .finally(() => {
                                    setIsUpdating(false)
                                    setIsOpen(false)
                                })
                        }}
                    >
                        Sačuvaj
                    </Button>
                    <Button
                        onClick={() => {
                            setIsOpen(false)
                        }}
                    >
                        Zatvori
                    </Button>
                </DialogActions>
            </Dialog>
            <Button
                style={{
                    color:
                        param.value != null && param.value.length > 0
                            ? mainTheme.palette.primary.main
                            : mainTheme.palette.action.disabled,
                }}
                onClick={() => setIsOpen(true)}
            >
                <Comment />
            </Button>
        </Box>
    )
}
