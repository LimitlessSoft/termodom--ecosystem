'use client'

import {
    CircularProgress,
    Divider,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import React, { useEffect, useState } from 'react'
import UsersListAddNewUser from './UsersListAddNewUser'
import { handleResponse } from '@/helpers/responseHelpers'
import UsersListItem from './UsersListItem'

export default function UsersList() {
    const [users, setUsers] = useState([])

    useEffect(() => {
        fetch('/api/admin/users').then(async (response) => {
            handleResponse(response, (data) => {
                setUsers(data)
            })
        })
    }, [])

    return (
        <Paper sx={{ p: 2 }}>
            <Stack spacing={1} alignItems={`center`}>
                <Stack
                    direction={`row`}
                    width={`100%`}
                    alignItems={`center`}
                    justifyContent={`space-between`}
                >
                    <Typography variant={`h6`}>Lista korisnika</Typography>
                    <UsersListAddNewUser
                        onCreate={(user) =>
                            setUsers((prev) =>
                                [...prev, user].sort((a, b) =>
                                    a.username.localeCompare(b.username)
                                )
                            )
                        }
                    />
                </Stack>
                <Divider sx={{ width: `100%`, mb: 2 }} />
                <Stack gap={1}>
                    {users.map((user, index) => (
                        <UsersListItem
                            key={user.id}
                            data={user}
                            index={index}
                            onDelete={() =>
                                setUsers((prev) =>
                                    prev.filter(
                                        (prevItem) => prevItem.id != user.id
                                    )
                                )
                            }
                        />
                    ))}
                </Stack>
                {(!users || users.length === 0) && <CircularProgress />}
            </Stack>
        </Paper>
    )
}
