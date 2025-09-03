import { handleResponse } from '@/helpers/responseHelpers'
import { AutocompleteMultipleSelect } from '@/widgets/Autocomplete'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
} from '@mui/material'
import { useParams } from 'next/navigation'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'

export default function UserAssignQuizzDialog({ isOpen, onClose, onSubmit }) {
    const [quizzes, setQuizzes] = useState([])
    const [loading, setLoading] = useState(false)
    const [selectedQuizzes, setSelectedQuizzes] = useState([])
    const { id: userId } = useParams()

    useEffect(() => {
        if (!isOpen) return
        setLoading(true)
        fetch(`/api/admin/users/${userId}/unassigned-quizzes`)
            .then((response) => {
                handleResponse(response, (data) => {
                    setQuizzes(data)
                })
            })
            .finally(() => {
                setLoading(false)
            })
    }, [isOpen, userId])

    const handleAssignQuizzesToUser = () => {
        setLoading(true)
        fetch(`/api/admin/users/${userId}/quizzes`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                quizzIds: selectedQuizzes.map((quizz) => quizz.id),
            }),
        })
            .then((response) => {
                handleResponse(response, () => {
                    toast.success('Uspešno dodeljeni kvizovi korisniku')
                    onSubmit(selectedQuizzes)
                    setSelectedQuizzes([])
                })
            })
            .finally(() => {
                setLoading(false)
                onClose()
            })
    }

    return (
        <Dialog open={isOpen} onClose={onClose}>
            <DialogTitle>Dodaj kviz korisniku</DialogTitle>
            <DialogContent>
                <AutocompleteMultipleSelect
                    options={quizzes}
                    disabled={loading}
                    loading={loading}
                    selected={selectedQuizzes}
                    onChange={setSelectedQuizzes}
                    label="Kvizovi"
                />
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={handleAssignQuizzesToUser}
                    variant={`contained`}
                    disabled={loading}
                >
                    Sačuvaj
                </Button>
                <Button onClick={onClose} disabled={loading}>
                    Otkaži
                </Button>
            </DialogActions>
        </Dialog>
    )
}
