import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Typography,
} from '@mui/material'
import { mainTheme } from '@/themes'

interface IAzuriranjeCenaPrimeniUsloveDialogProps {
    isOpen: boolean
    handleClose: (nastaviAkciju: boolean) => void
}

export const AzuriranjeCenaPrimeniUsloveDialog = (
    props: IAzuriranjeCenaPrimeniUsloveDialogProps
) => {
    return (
        <Dialog open={props.isOpen} onClose={() => props.handleClose(false)}>
            <DialogTitle>Primeni uslove formiranja Min Web Osnove</DialogTitle>
            <DialogContent>
                <Typography>
                    Da li ste sigurni da želite da primenite uslove formiranja
                    Min Web Osnove?
                </Typography>
                <Typography
                    py={5}
                    color={mainTheme.palette.warning.main}
                    textAlign={`justify`}
                >
                    Pre ove akcije obavezno pokrenite &quot;AŽURIRAJ MAX WEB
                    OSNOVE&quot; jer ukoliko min osnova bude veća od max osnove,
                    min osnova će biti postavljena na vrednost max osnove (tako
                    da ako je max osnova 0, i min osnova ce biti 0 uvek)
                </Typography>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        props.handleClose(true)
                    }}
                >
                    Primeni uslove!
                </Button>
                <Button onClick={() => props.handleClose(false)}>
                    Odustani!
                </Button>
            </DialogActions>
        </Dialog>
    )
}
