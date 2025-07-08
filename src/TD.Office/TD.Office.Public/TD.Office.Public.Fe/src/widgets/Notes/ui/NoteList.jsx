import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    IconButton,
    Paper,
    Stack,
    TextField,
    Typography,
    useTheme,
} from '@mui/material'
import { Delete } from '@mui/icons-material'
import {
    parseNoteList,
    stringifyNoteList,
} from '../../../helpers/noteListHelpers'
import Grid2 from '@mui/material/Unstable_Grid2'
import { useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { toast } from 'react-toastify'

const NewNote = ({ originalNote, onAdded }) => {
    const [isOpen, setIsOpen] = useState(false)
    const [value, setValue] = useState('')
    const parsedNotes = parseNoteList(originalNote.content)

    return (
        <Grid2 xs={12}>
            <Dialog open={isOpen} onClose={() => setIsOpen(false)}>
                <DialogTitle>Novi task</DialogTitle>
                <DialogContent>
                    <TextField
                        placeholder={`Tekst taska`}
                        value={value}
                        onChange={(event) => setValue(event.target.value)}
                    />
                </DialogContent>
                <DialogActions>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            parsedNotes.push(value)
                            const stringifiedNotes =
                                stringifyNoteList(parsedNotes)
                            const note = {
                                ...originalNote,
                                content: stringifiedNotes,
                            }
                            officeApi
                                .put(ENDPOINTS_CONSTANTS.NOTES.PUT, {
                                    ...note,
                                    oldContent: originalNote.content,
                                })
                                .then((response) => {
                                    toast.success('Task uspešno dodat')
                                    setIsOpen(false)
                                    onAdded(note, parsedNotes)
                                })
                                .catch(handleApiError)
                        }}
                    >
                        Kreiraj task
                    </Button>
                    <Button
                        variant={`outlined`}
                        onClick={() => {
                            setIsOpen(false)
                            setValue('')
                        }}
                    >
                        Odustani
                    </Button>
                </DialogActions>
            </Dialog>
            <Button
                variant={`contained`}
                onClick={() => {
                    setIsOpen(true)
                }}
            >
                Novi Task
            </Button>
        </Grid2>
    )
}

export const NoteList = ({ note }) => {
    const theme = useTheme()
    const [originalNote, setOriginalNote] = useState(note)
    const [parsedNotes, setParsedNotes] = useState(parseNoteList(note.content))

    return (
        <Paper
            sx={{
                backgroundColor: theme.palette.info.light,
                my: theme.spacing(1),
            }}
        >
            <Grid2 container gap={2} p={2}>
                <NewNote
                    originalNote={originalNote}
                    onAdded={(note, parsedNotes) => {
                        setParsedNotes(parsedNotes)
                        setOriginalNote(note)
                    }}
                />
                {parsedNotes.length === 0 && (
                    <Grid2>
                        <Paper
                            sx={{
                                p: 2,
                                backgroundColor: 'orange',
                                boxShadow: 1,
                            }}
                        >
                            <Typography variant="body1">
                                Lista nema taskove
                            </Typography>
                        </Paper>
                    </Grid2>
                )}
                {parsedNotes.map((note, index) => (
                    <Grid2 key={index}>
                        <Paper
                            sx={{
                                p: 1,
                                backgroundColor: 'background.paper',
                                boxShadow: 1,
                            }}
                        >
                            <Stack
                                direction="row"
                                justifyContent="space-between"
                                alignItems="center"
                            >
                                <Typography variant="body1">{note}</Typography>
                                <IconButton
                                    onClick={() => {
                                        const parsedN = [...parsedNotes]
                                        parsedN.splice(index, 1)
                                        const stringifiedNotes =
                                            stringifyNoteList(parsedN)
                                        const n = {
                                            ...originalNote,
                                            content: stringifiedNotes,
                                        }
                                        officeApi
                                            .put(
                                                ENDPOINTS_CONSTANTS.NOTES.PUT,
                                                {
                                                    ...n,
                                                    oldContent:
                                                        originalNote.content,
                                                }
                                            )
                                            .then((response) => {
                                                toast.success(
                                                    'Task uspešno izbrisan'
                                                )
                                                setParsedNotes(parsedN)
                                                setOriginalNote(n)
                                            })
                                            .catch(handleApiError)
                                    }}
                                >
                                    <Delete />
                                </IconButton>
                            </Stack>
                        </Paper>
                    </Grid2>
                ))}
            </Grid2>
        </Paper>
    )
}
