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

    return (
        data && (
            <>
                <Stack direction="row" spacing={2} alignItems={`center`}>
                    <Tooltip title="Prethodni korisnik">
                        <IconButton
                            disabled={!data.prevId}
                            color="primary"
                            onClick={() => onUserSwitch(data.prevId)}
                        >
                            <ArrowCircleLeftOutlined fontSize="large" />
                        </IconButton>
                    </Tooltip>
                    <Typography variant={`h5`}>
                        Korisnik: {data.username}
                    </Typography>
                    <Tooltip title="Sledeći korisnik">
                        <IconButton
                            disabled={!data.nextId}
                            color="primary"
                            onClick={() => onUserSwitch(data.nextId)}
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
