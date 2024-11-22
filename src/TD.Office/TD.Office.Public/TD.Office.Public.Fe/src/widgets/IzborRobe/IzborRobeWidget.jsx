import { useRouter } from 'next/router'
import { useZRoba } from '../../zStore/zRoba'
import { useEffect, useRef, useState } from 'react'
import { CircularProgress, Stack, Typography } from '@mui/material'
import { IzborRobeSearch } from './IzborRobeSearch'
import { IzborRobeTable } from './IzborRobeTable'

export const IzborRobeWidget = () => {
    const router = useRouter()
    const zRoba = useZRoba()

    const [search, setSearch] = useState('')

    const hostChannel = useRef(null)

    useEffect(() => {
        if (!router) return
        hostChannel.current = new BroadcastChannel(router.query.channel)
    }, [router])

    return (
        <Stack p={2} gap={2}>
            <Typography variant={`h5`}>Izbor robe</Typography>
            <IzborRobeSearch
                onChange={(text) => {
                    setSearch(text)
                }}
            />
            {(!zRoba || zRoba.length === 0) && <CircularProgress />}
            {zRoba && zRoba.length > 0 && (
                <IzborRobeTable
                    roba={zRoba}
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
