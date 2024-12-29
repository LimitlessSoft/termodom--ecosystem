import { Box, Button, CircularProgress, Stack, TextField } from '@mui/material'
import { useEffect, useState } from 'react'
import { NoteDeleteDialog } from './NoteDeleteDialog'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { toast } from 'react-toastify'
import { NoteChangeNameDialog } from './NoteChangeNameDialog'

export const NoteCard = ({ id, onDelete, onNameChanged }) => {
    const [note, setNote] = useState(undefined)
    const [edited, setEdited] = useState(false)

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
                setNote(data)
                setOriginalText(data.content)
            })
            .catch(handleApiError)
    }, [id])

    if (!note) return <CircularProgress />

    return (
        <Box px={2}>
            <TextField
                disabled={isUpdating}
                onChange={(e) => {
                    setEdited(true)
                    setNote({
                        ...note,
                        content: e.target.value,
                    })
                }}
                fullWidth
                value={note.content}
                multiline
                rows={10}
                sx={{
                    my: 2,
                }}
            />
            <Stack
                direction={`row`}
                justifyContent={`end`}
                paddingBottom={2}
                gap={2}
            >
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
                                    ENDPOINTS_CONSTANTS.NOTES.DELETE(note.id)
                                )
                                .then((response) => {
                                    toast.success('Beleška uspešno obrisana')
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
                                    ENDPOINTS_CONSTANTS.NOTES.PUT_NAME(note.id),
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
                                ...note,
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
        </Box>
    )
}
