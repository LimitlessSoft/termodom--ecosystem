'use client'
import { useState } from 'react'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    MenuItem,
    Select,
    Stack,
    Typography,
} from '@mui/material'

export const StartQuizzDialog = ({ isOpen, onCancel, onStart }) => {
    const [quizzType, setQuizzType] = useState('proba')
    return (
        <Dialog
            open={isOpen}
            onClose={() => {
                onCancel()
            }}
            maxWidth={`md`}
        >
            <DialogTitle>Započni kviz</DialogTitle>
            <DialogContent>
                <Stack spacing={1}>
                    <Select
                        variant={`outlined`}
                        value={quizzType}
                        onChange={(e) => {
                            setQuizzType(e.target.value)
                        }}
                    >
                        <MenuItem value={`proba`}>Proba</MenuItem>
                        <MenuItem value={`ucenje`}>Učenje</MenuItem>
                        <MenuItem value={`ocenjivanje`}>Ocenjivanje</MenuItem>
                    </Select>
                    {quizzType === `proba` && (
                        <Typography>
                            Ovaj kviz je namenjen za vežbanje i mozete ga
                            ponoviti vise puta.
                        </Typography>
                    )}
                    {quizzType === `ucenje` && (
                        <Typography>
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 5ca913d5 (just saving)
=======
>>>>>>> 7e8db912 (Implemented 'ucenje' session type)
                            Ovaj kviz je namenjen za vežbanje i možete ga
                            ponoviti više puta, posle svakog odgovorenog pitanja
                            dobijate tačne odgovore za isto.
=======
                            Ovaj kviz je namenjen za vežbanje i mozete ga
                            ponoviti vise puta, posle svakog odgovorenog pitanja
                            dobijate tacne odgovore za isto.
<<<<<<< HEAD
>>>>>>> 3c45c730 (just saving)
=======
                            Ovaj kviz je namenjen za vežbanje i možete ga
                            ponoviti više puta, posle svakog odgovorenog pitanja
                            dobijate tačne odgovore za isto.
>>>>>>> c96d11f6 (Implemented 'ucenje' session type)
=======
>>>>>>> 460fe2d3 (just saving)
<<<<<<< HEAD
>>>>>>> 5ca913d5 (just saving)
=======
=======
                            Ovaj kviz je namenjen za vežbanje i možete ga
                            ponoviti više puta, posle svakog odgovorenog pitanja
                            dobijate tačne odgovore za isto.
>>>>>>> 9dbcf031 (Implemented 'ucenje' session type)
>>>>>>> 7e8db912 (Implemented 'ucenje' session type)
                        </Typography>
                    )}
                    {quizzType === `ocenjivanje` && (
                        <Typography color={`error`}>
                            Ovaj kviz mozete pokrenuti samo jednom.
                        </Typography>
                    )}
                </Stack>
            </DialogContent>
            <DialogActions>
                <Button
                    variant={`contained`}
                    onClick={() => {
                        onStart(quizzType)
                    }}
                >
                    Zapocni
                </Button>
                <Button
                    variant={`outlined`}
                    onClick={() => {
                        onCancel()
                    }}
                >
                    Odustani
                </Button>
            </DialogActions>
        </Dialog>
    )
}
