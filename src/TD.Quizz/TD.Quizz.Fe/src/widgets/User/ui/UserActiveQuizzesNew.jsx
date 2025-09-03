import { AddCircle } from '@mui/icons-material'
import { IconButton } from '@mui/material'
import { useState } from 'react'
import UserAssignQuizzDialog from './UserAssignQuizzDialog'

export default function UserActiveQuizzesNew({ onAddNew }) {
    const [isOpen, setIsOpen] = useState(false)

    return (
        <>
            <UserAssignQuizzDialog
                isOpen={isOpen}
                onSubmit={onAddNew}
                onClose={() => setIsOpen(false)}
            />
            <IconButton onClick={() => setIsOpen(true)}>
                <AddCircle color={`primary`} />
            </IconButton>
        </>
    )
}
