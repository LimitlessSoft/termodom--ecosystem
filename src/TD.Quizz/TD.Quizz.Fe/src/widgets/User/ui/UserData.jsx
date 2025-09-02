import { handleResponse } from '@/helpers/responseHelpers'
import { Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import UserUpdateDialog from './UserUpdateDialog'

export default function UserData({ userId }) {
    const [data, setData] = useState()

    useEffect(() => {
        fetch(`/api/admin/users/${userId}`).then((response) => {
            handleResponse(response, (data) => setData(data))
        })
    }, [])

    return (
        data && (
            <>
                <Typography variant={`h5`}>
                    Korisnik: {data.username}
                </Typography>
                <UserUpdateDialog
                    data={data}
                    onUpdate={(username) => {
                        setData((prev) => ({
                            ...prev,
                            username,
                        }))
                    }}
                />
            </>
        )
    )
}
