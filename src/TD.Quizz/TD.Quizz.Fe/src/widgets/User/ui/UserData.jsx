import { handleResponse } from '@/helpers/responseHelpers'
import { IconButton, Stack, Tooltip, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import UserUpdateDialog from './UserUpdateDialog'
import {
    ArrowCircleLeftOutlined,
    ArrowCircleRightOutlined,
} from '@mui/icons-material'

export default function UserData({ userId, onUserSwitch }) {
    const [data, setData] = useState()

    useEffect(() => {
        fetch(`/api/admin/users/${userId}`).then((response) => {
            handleResponse(response, (data) => {
                setData(data)
            })
        })
    }, [userId])

    const handleSwitchUser = (navigation) => {
        fetch(`/api/admin/users/${userId}/${navigation}`).then((response) => {
            handleResponse(response, (data) => {
                if (data.id) {
                    onUserSwitch(data.id)
                }
            })
        })
    }

    return (
        data && (
            <>
                <Stack direction="row" spacing={2} alignItems={`center`}>
                    <Tooltip title="Prethodni korisnik">
                        <IconButton
                            color="primary"
                            onClick={() => handleSwitchUser('prev')}
                        >
                            <ArrowCircleLeftOutlined fontSize="large" />
                        </IconButton>
                    </Tooltip>
                    <Typography variant={`h5`}>
                        Korisnik: {data.username}
                    </Typography>
                    <Tooltip title="Sledeći korisnik">
                        <IconButton
                            color="primary"
                            onClick={() => handleSwitchUser('next')}
                        >
                            <ArrowCircleRightOutlined fontSize="large" />
                        </IconButton>
                    </Tooltip>
                </Stack>
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
