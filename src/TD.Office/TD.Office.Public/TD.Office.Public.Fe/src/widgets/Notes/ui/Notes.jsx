import {
    CircularProgress,
    IconButton,
    Paper,
    Stack,
    Tab,
    Tabs,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { NoteCard } from './NoteCard'
import { Add } from '@mui/icons-material'
import { mainTheme } from '../../../themes'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { NoteNewDialog } from './NoteNewDialog'
import { Zoomable } from '../../Zoomable/ui/Zoomable'

export const Notes = () => {
    const [tabId, setTabId] = useState(undefined)
    const [notes, setNotes] = useState(undefined)

    const [isCreating, setIsCreating] = useState(false)
    const [noteNewDialogOpen, setNoteNewDialogOpen] = useState(false)

    useEffect(() => {
        officeApi
            .get(ENDPOINTS_CONSTANTS.NOTES.GET_INITIAL)
            .then(({ data }) => {
                setTabId(data.lastNoteId)
                setNotes(data)
            })
            .catch(handleApiError)
    }, [])

    if (!notes || !tabId) return <CircularProgress />

    return (
        <Zoomable component={Paper} sx={{ position: 'relative' }}>
            <Tabs
                value={tabId}
                variant={`scrollable`}
                scrollButtons="auto"
                sx={{
                    maxWidth: '80%',
                    flexWrap: 'wrap',
                    '& .MuiTabs-flexContainer': {
                        flexWrap: 'wrap',
                    },
                    '& .MuiTab-root': {
                        minWidth: 'fit-content',
                    },
                    '& .MuiTabs-indicator': {
                        display: 'none',
                    },
                }}
                onChange={(e, value) => {
                    setTabId(value)
                }}
            >
                {Object.keys(notes.notes).map((noteId) => (
                    <Tab
                        key={noteId}
                        label={notes.notes[noteId]}
                        value={Number.parseInt(noteId)}
                        sx={{
                            position: 'relative',
                            '&::after': {
                                content: '""',
                                position: 'absolute',
                                bottom: 0,
                                left: '50%',
                                height: '2px',
                                backgroundColor: 'primary.main',
                                width: '100%',
                                transform: 'translateX(-50%) scaleX(0)',
                                opacity: 0,
                                transition:
                                    'transform 0.3s ease, opacity 0.3s ease',
                            },
                            '&.Mui-selected::after': {
                                transform: 'translateX(-50%) scaleX(1)',
                                opacity: 1,
                            },
                        }}
                    />
                ))}
            </Tabs>
            <Stack
                direction={`row`}
                gap={2}
                sx={{
                    position: 'absolute',
                    left: `8px`,
                    top: `8px`,
                }}
            >
                <NoteNewDialog
                    disabled={isCreating}
                    open={noteNewDialogOpen}
                    onCancel={() => {
                        setNoteNewDialogOpen(false)
                    }}
                    onConfirm={(value) => {
                        setIsCreating(true)
                        officeApi
                            .put(ENDPOINTS_CONSTANTS.NOTES.PUT, {
                                name: value,
                                content: '',
                            })
                            .then(({ data }) => {
                                setNotes({
                                    ...notes,
                                    notes: {
                                        ...notes.notes,
                                        [data]: value,
                                    },
                                })
                                setTabId(data)
                            })
                            .catch(handleApiError)
                            .finally(() => {
                                setIsCreating(false)
                                setNoteNewDialogOpen(false)
                            })
                    }}
                />
                <IconButton
                    size={`small`}
                    sx={{
                        backgroundColor: mainTheme.palette.primary.main,
                        color: mainTheme.palette.primary.contrastText,
                    }}
                    onClick={() => {
                        setNoteNewDialogOpen(true)
                    }}
                >
                    <Add />
                </IconButton>
            </Stack>
            <NoteCard
                id={tabId}
                onDelete={() => {
                    const firstNextTabId = Object.keys(notes.notes).find(
                        (noteId) => noteId !== tabId
                    )
                    setTabId(Number.parseInt(firstNextTabId))
                }}
                onNameChanged={(newNam) => {
                    setNotes({
                        ...notes,
                        notes: {
                            ...notes.notes,
                            [tabId]: newNam,
                        },
                    })
                }}
            />
        </Zoomable>
    )
}
