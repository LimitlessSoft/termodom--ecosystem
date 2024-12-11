import {
    DialogTitle,
    DialogContent,
    TextField,
    DialogActions,
    Button,
    CircularProgress,
} from '@mui/material'

export default function PartneriKomercijalnoIFinansijskoTableKomentarCellDialogStepOne({
    onSave,
    onGoToNextDialogStep,
    comment,
    handleChangeComment,
    isUpdating,
}) {
    return (
        <>
            <DialogTitle>Komentar</DialogTitle>
            <DialogContent>
                <TextField
                    multiline
                    rows={10}
                    value={comment}
                    onChange={(e) => handleChangeComment(e.target.value)}
                    fullWidth
                />
            </DialogContent>
            <DialogActions>
                <Button
                    startIcon={isUpdating && <CircularProgress size={20} />}
                    disabled={isUpdating}
                    variant="contained"
                    onClick={onSave}
                >
                    Sačuvaj
                </Button>
                <Button onClick={onGoToNextDialogStep}>Zatvori</Button>
            </DialogActions>
        </>
    )
}
