import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Typography,
} from '@mui/material'

interface IAzurirajMaxWebOsnoveDialogProps {
    isOpen: boolean
    handleClose: (nastaviAkciju: boolean) => void
}

export const AzurirajMaxWebOsnoveDialog = (
    props: IAzurirajMaxWebOsnoveDialogProps
) => {
    return (
        <Dialog
            open={props.isOpen}
            onClose={() => {
                props.handleClose(false)
            }}
        >
            <DialogTitle>Ažuriraj Max Web Osnove</DialogTitle>

            <DialogContent>
                <Typography>
                    Ova akcija će ažurirati sve Max Web Osnove na trenutnu
                    Prodajnu Cenu Komercijalno.
                </Typography>
            </DialogContent>

            <DialogActions>
                <Button onClick={() => props.handleClose(true)}>
                    Ažuriraj!
                </Button>
                <Button onClick={() => props.handleClose(false)}>
                    Odustani!
                </Button>
            </DialogActions>
        </Dialog>
    )
}
