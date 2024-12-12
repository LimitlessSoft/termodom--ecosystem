import { Box, Button, Dialog } from '@mui/material'
import { toast } from 'react-toastify'
import { useState } from 'react'
import { mainTheme } from '@/themes'
import { Comment } from '@mui/icons-material'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '@/constants'
import PartneriKomercijalnoIFinansijskoTableKomentarCellStepOne from './PartneriKomercijalnoIFinansijskoTableKomentarCellDialogStepOne'
import PartneriKomercijalnoIFinansijskoTableKomentarCellStepTwo from './PartneriKomercijalnoIFinansijskoTableKomentarCellDialogStepTwo'

export const PartneriKomercijalnoIFinansijskoTableKomentarCell = ({
    param,
}) => {
    const [originalComment, setOriginalComment] = useState(param.value || '')
    const [comment, setComment] = useState(param.value || '')
    const [dialogState, setDialogState] = useState({
        isOpen: false,
        step: null,
    })
    const [isUpdating, setIsUpdating] = useState(false)

    const handleOpenDialog = () => setDialogState({ isOpen: true, step: 1 })

    const handleCloseDialog = () => {
        setDialogState({ isOpen: false, step: null })
        setComment(originalComment)
    }

    const handleGoToNextDialog = () =>
        setDialogState((prev) => ({ ...prev, step: prev.step + 1 }))

    const handleGoToPrevDialog = () =>
        setDialogState((prev) => ({ ...prev, step: prev.step - 1 }))

    const handleSaveComment = () => {
        setIsUpdating(true)

        officeApi
            .put(
                ENDPOINTS_CONSTANTS.PARTNERS.PUT_KOMERCIJALNO_I_FINANSIJSKO_DATA_KOMENTAR(
                    param.id
                ),
                {
                    ppid: param.id,
                    komentar: comment,
                }
            )
            .then(() => {
                toast.success('Uspešno sačuvano')
                setOriginalComment(comment)
            })
            .catch(handleApiError)
            .finally(() => {
                setIsUpdating(false)
                setDialogState({ isOpen: false, step: null })
            })
    }

    const isCurrentCommentSameAsInitial =
        comment.trim() === originalComment.trim()

    const renderDialogContent = () => {
        switch (dialogState.step) {
            case 1:
                return (
                    <PartneriKomercijalnoIFinansijskoTableKomentarCellStepOne
                        onSave={handleSaveComment}
                        onGoToNextDialogStep={
                            isCurrentCommentSameAsInitial
                                ? handleCloseDialog
                                : handleGoToNextDialog
                        }
                        comment={comment}
                        handleChangeComment={(value) => setComment(value)}
                        isUpdating={isUpdating}
                    />
                )
            case 2:
                return (
                    <PartneriKomercijalnoIFinansijskoTableKomentarCellStepTwo
                        onSave={handleSaveComment}
                        onClose={handleCloseDialog}
                        onGoToPrevDialogStep={handleGoToPrevDialog}
                    />
                )
        }
    }

    return (
        <Box>
            <Button
                style={{
                    color: comment.trim()
                        ? mainTheme.palette.primary.main
                        : mainTheme.palette.action.disabled,
                }}
                onClick={handleOpenDialog}
            >
                <Comment />
            </Button>
            <Dialog
                open={dialogState.isOpen}
                onClose={
                    isCurrentCommentSameAsInitial || dialogState.step == 2
                        ? handleCloseDialog
                        : handleGoToNextDialog
                }
            >
                {renderDialogContent()}
            </Dialog>
        </Box>
    )
}
