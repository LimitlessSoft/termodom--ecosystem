import {
    Box,
    Button,
    CircularProgress,
    Grid,
    IconButton,
    Stack,
    TextField,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { NoteDeleteDialog } from './NoteDeleteDialog'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS, NOTE_CONSTANTS } from '@/constants'
import { toast } from 'react-toastify'
import { NoteChangeNameDialog } from './NoteChangeNameDialog'
import { ZoomIn, ZoomOut } from '@mui/icons-material'

export const NoteCard = ({ id, onDelete, onNameChanged }) => {
    const [note, setNote] = useState({
        id: undefined,
        firstPart: '',
        secondPart: '',
    })
    const [edited, setEdited] = useState(false)
    const [lines, setlines] = useState(20)

    const [isUpdating, setIsUpdating] = useState(false)

    const [originalText, setOriginalText] = useState(undefined)
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false)
    const [changeNameDialogOpen, setChangeNameDialogOpen] = useState(false)

    useEffect(() => {
        setNote(undefined)
        setEdited(false)
        officeApi
            .get(ENDPOINTS_CONSTANTS.NOTES.GET(id))
            .then(({ data }) => {
                const splittedNote = data.content?.split(
                    NOTE_CONSTANTS.NOTE_FIELDS_PAYLOAD_SEPARATOR
                )

                setNote({
                    ...data,
                    firstPart: splittedNote ? splittedNote[0] : '',
                    secondPart: splittedNote ? splittedNote[1] : '',
                })
                setOriginalText(data.content)
            })
            .catch(handleApiError)
    }, [id])

    if (!note) return <CircularProgress />

    return (
        <Box px={2}>
            <Grid container spacing={2}>
                <Grid item xs={6}>
                    <TextField
                        disabled={isUpdating}
                        onChange={(e) => {
                            setEdited(true)
                            setNote({
                                ...note,
                                firstPart: e.target.value,
                            })
                        }}
                        fullWidth
                        value={note.firstPart}
                        multiline
                        rows={lines}
                        sx={{
                            my: 2,
                        }}
                    />
                </Grid>
                <Grid item xs={6}>
                    <TextField
                        disabled={isUpdating}
                        onChange={(e) => {
                            setEdited(true)
                            setNote({
                                ...note,
                                secondPart: e.target.value,
                            })
                        }}
                        fullWidth
                        value={note.secondPart}
                        multiline
                        rows={lines}
                        sx={{
                            my: 2,
                        }}
                    />
                </Grid>
            </Grid>
            <Stack
                direction={`row`}
                justifyContent={`space-between`}
                paddingBottom={2}
                gap={2}
            >
                <Box>
                    <IconButton
                        onClick={() => {
                            setlines((prev) => {
                                if (prev < 50) return prev + 5
                                return prev
                            })
                        }}
                    >
                        <ZoomIn color={`secondary`} />
                    </IconButton>
                    <IconButton
                        onClick={() => {
                            setlines((prev) => {
                                if (prev > 5) return prev - 5
                                return prev
                            })
                        }}
                    >
                        <ZoomOut color={`secondary`} />
                    </IconButton>
                </Box>
                <Stack direction={`row`} gap={2}>
                    <>
                        <NoteDeleteDialog
                            open={deleteDialogOpen}
                            onCancel={() => {
                                setDeleteDialogOpen(false)
                            }}
                            onConfirm={() => {
                                setDeleteDialogOpen(false)
                                setIsUpdating(true)
                                officeApi
                                    .delete(
                                        ENDPOINTS_CONSTANTS.NOTES.DELETE(
                                            note.id
                                        )
                                    )
                                    .then((response) => {
                                        toast.success(
                                            'Beleška uspešno obrisana'
                                        )
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setIsUpdating(false)
                                        onDelete()
                                    })
                            }}
                        />
                        <Button
                            variant={`outlined`}
                            disabled={isUpdating}
                            onClick={() => {
                                setDeleteDialogOpen(true)
                            }}
                        >
                            Obrisi
                        </Button>
                    </>
                    <>
                        <NoteChangeNameDialog
                            name={note.name}
                            onCancel={() => {
                                setChangeNameDialogOpen(false)
                            }}
                            open={changeNameDialogOpen}
                            onConfirm={(value) => {
                                setChangeNameDialogOpen(false)
                                setIsUpdating(true)
                                officeApi
                                    .put(
                                        ENDPOINTS_CONSTANTS.NOTES.PUT_NAME(
                                            note.id
                                        ),
                                        {
                                            name: value,
                                        }
                                    )
                                    .then((response) => {
                                        onNameChanged(value)
                                        toast.success('Naziv uspešno izmenjen')
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setIsUpdating(false)
                                    })
                            }}
                        />
                        <Button
                            variant={`outlined`}
                            disabled={isUpdating}
                            onClick={() => {
                                setChangeNameDialogOpen(true)
                            }}
                        >
                            Izmeni naziv
                        </Button>
                    </>
                    <Button
                        disabled={edited === false || isUpdating}
                        variant={`outlined`}
                        onClick={() => {
                            setEdited(false)
                            setNote({
                                ...note,
                                content: originalText,
                            })
                        }}
                    >
                        Odustani od izmena
                    </Button>
                    <Button
                        variant={`contained`}
                        disabled={edited === false || isUpdating}
                        onClick={() => {
                            setIsUpdating(true)
                            officeApi
                                .put(ENDPOINTS_CONSTANTS.NOTES.PUT, {
                                    ...{
                                        id: note.id,
                                        name: note.name,
                                        content: `${note.firstPart}${NOTE_CONSTANTS.NOTE_FIELDS_PAYLOAD_SEPARATOR}${note.secondPart}`,
                                    },
                                    oldContent: originalText,
                                })
                                .then((response) => {
                                    toast.success('Beleška uspešno izmenjena')
                                })
                                .catch(handleApiError)
                                .finally(() => {
                                    setIsUpdating(false)
                                })
                        }}
                    >
                        Sacuvaj
                    </Button>
                </Stack>
            </Stack>
        </Box>
    )
}
