import { DialogContent, Typography, DialogActions, Button } from '@mui/material'

export default function PartneriKomercijalnoIFinansijskoTableKomentarCellDialogStepTwo({
    onSave,
    onClose,
    onGoToPrevDialogStep,
}) {
    return (
        <>
            <DialogContent>
                <Typography textAlign={`center`}>
                    Da li ste sigurni da želite da izadjete iz komentara?
                </Typography>
            </DialogContent>
            <DialogActions>
                <Button variant={`contained`} onClick={onSave}>
                    Sačuvaj i izadji
                </Button>
                <Button variant={`outlined`} onClick={onGoToPrevDialogStep}>
                    Ipak ostani
                </Button>
                <Button onClick={onClose}>Izadji bez čuvanja</Button>
            </DialogActions>
        </>
    )
}
