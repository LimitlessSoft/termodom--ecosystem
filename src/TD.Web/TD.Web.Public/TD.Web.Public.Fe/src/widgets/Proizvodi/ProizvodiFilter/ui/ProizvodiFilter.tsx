import { KeyboardBackspace } from '@mui/icons-material'
import { Button, CircularProgress, Grid } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { ProizvodiFilterButton } from './ProizvodiFilterButton'
import { toast } from 'react-toastify'
import { useUser } from '@/app/hooks'
import { webApi } from '@/api/webApi'

export const ProizvodiFilter = (props: any) => {
    const user = useUser(false, false)
    const router = useRouter()
    const [groups, setGroups] = useState<any | undefined>(null)

    useEffect(() => {
        setGroups(null)

        webApi
            .get('/products-groups', {
                params: { parentName: props.currentGroup?.name },
            })
            .then((res) => setGroups(res.data))
        if (
            props.currentGroup == null ||
            props.currentGroup.welcomeMessage == null ||
            props.currentGroup.welcomeMessage.length === 0
        )
            return

        toast.info(props.currentGroup.welcomeMessage)
    }, [props.currentGroup])

    return (
        <Grid
            container
            justifyContent={'center'}
            spacing={1}
            sx={{ py: 1, my: 1 }}
        >
            {groups && props.currentGroup && (
                <Grid item>
                    <Button
                        variant={'contained'}
                        color={'warning'}
                        sx={{ color: 'inherit' }}
                        onClick={() => {
                            router.push(
                                `/${props.currentGroup.parentName ?? ''}`
                            )
                        }}
                    >
                        <KeyboardBackspace />
                    </Button>
                </Grid>
            )}
            {groups ? (
                groups.map((g: any) => {
                    return <ProizvodiFilterButton key={g.name} group={g} />
                })
            ) : (
                <CircularProgress />
            )}
            {user?.isLogged && (
                <Grid item>
                    <Button
                        variant={'contained'}
                        color={`success`}
                        onClick={() => {
                            router.push('/omiljeni-proizvodi')
                        }}
                    >
                        Omiljeni
                    </Button>
                </Grid>
            )}
        </Grid>
    )
}
