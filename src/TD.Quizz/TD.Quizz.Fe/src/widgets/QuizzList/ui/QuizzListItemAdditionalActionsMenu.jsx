import { handleResponse } from '@/helpers/responseHelpers'
import ThreeDotsMenu from '@/widgets/ThreeDotsMenu/ui/ThreeDotsMenu'
import { toast } from 'react-toastify'

export default function QuizzListItemAdditionalActionsMenu({
    quizzId,
    quizzName,
}) {
    const handleAssignQuizzToAllUsers = () => {
        fetch(`/api/admin/quizzes/${quizzId}/assign-to-all`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
        }).then((response) =>
            handleResponse(response, () => {
                toast.success(
                    `Uspešno ste dodelili kviz '${quizzName}' svim korinicima`
                )
            })
        )
    }

    return (
        <ThreeDotsMenu
            options={[
                {
                    label: 'Dodeli kviz svima',
                    onClick: handleAssignQuizzToAllUsers,
                },
            ]}
        />
    )
}
