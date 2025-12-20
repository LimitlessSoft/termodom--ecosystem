import { useRouter } from 'next/router'
import { reloadRobaAsync, useZRoba } from '../../zStore/zRoba'
import { useEffect, useRef, useState } from 'react'
import {
    CircularProgress,
    IconButton,
    Stack,
    Typography,
    Tooltip,
} from '@mui/material'
import { IzborRobeSearch } from './IzborRobeSearch'
import { IzborRobeTable } from './IzborRobeTable'
import RefreshIcon from '@mui/icons-material/Refresh'
import dayjs from 'dayjs'

export const IzborRobeWidget = () => {
    const router = useRouter()
    const zRoba = useZRoba()

    const [search, setSearch] = useState('')

    const hostChannel = useRef(null)

    const [isRefreshing, setIsRefreshing] = useState(false)

    useEffect(() => {
        if (!router) return
        hostChannel.current = new BroadcastChannel(router.query.channel)
    }, [router])

    return (
        <Stack p={2} gap={2}>
            <Stack
                direction={`row`}
                justifyContent={`space-between`}
                alignItems={`center`}
            >
                <Typography variant={`h5`}>Izbor robe</Typography>
                <Stack direction={`row`} alignItems={`center`} gap={1}>
                    <Typography variant={`caption`} color={`textSecondary`}>
                        Poslednje osvežavanje:{' '}
                        {zRoba?.lastRefresh
                            ? dayjs(zRoba.lastRefresh).format(
                                  'DD.MM.YYYY. HH:mm:ss'
                              )
                            : 'N/A'}
                    </Typography>
                    <Tooltip title={`Osveži podatke`}>
                        <IconButton
                            disabled={isRefreshing}
                            onClick={async () => {
                                setIsRefreshing(true)
                                await reloadRobaAsync()
                                setIsRefreshing(false)
                            }}
                        >
                            <RefreshIcon />
                        </IconButton>
                    </Tooltip>
                </Stack>
            </Stack>
            <IzborRobeSearch
                onChange={(text) => {
                    setSearch(text)
                }}
            />
            {(!zRoba?.data || zRoba.data.length === 0) && (
                <Stack alignItems={`center`} p={4}>
                    <CircularProgress />
                </Stack>
            )}
            {zRoba?.data && zRoba.data.length > 0 && (
                <IzborRobeTable
                    roba={zRoba.data}
                    filter={{ search: search }}
                    inputKolicine={true}
                    onSelectRoba={(robaId, kolicina) => {
                        hostChannel.current.postMessage({
                            type: 'select-roba',
                            payload: { robaId, kolicina },
                        })
                    }}
                />
            )}
        </Stack>
    )
}
