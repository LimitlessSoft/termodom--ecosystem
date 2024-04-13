import { ApiBase, fetchApi } from "@/app/api"
import { KeyboardBackspace } from "@mui/icons-material"
import { Box, Button, CircularProgress, Grid, LinearProgress, Stack } from "@mui/material"
import { useRouter } from "next/router"
import { use, useEffect, useState } from "react"
import { ProizvodiFilterButton } from "./ProizvodiFilterButton"
import { toast } from "react-toastify"

export const ProizvodiFilter = (): JSX.Element => {

    const router = useRouter()
    const [currentGroup, setCurrentGroup] = useState<any>(null)
    const [groups, setGroups] = useState<any | undefined>(null)

    useEffect(() => {
        setGroups(null)

        let url = `/products-groups`
        if(router.query.grupa != null && router.query.grupa !== 'undefined' && router.query.grupa !== 'null' && router.query.grupa !== '' && router.query.grupa != undefined)
            url += `?parentName=${router.query.grupa}`

        fetchApi(ApiBase.Main, url)
            .then((payload) => setGroups(payload))
    }, [router.query.grupa])

    useEffect(() => {
        if(currentGroup == null || currentGroup.welcomeMessage == null || currentGroup.welcomeMessage.length === 0)
            return

        toast.info(currentGroup.welcomeMessage)
    }, [currentGroup])

    useEffect(() => {
        if(router.query.grupa == null || router.query.grupa == undefined || router.query.grupa.length === 0)
        {
            setCurrentGroup(null)
            return
        }
        fetchApi(ApiBase.Main, `/products-groups/${router.query.grupa}`)
            .then((payload) =>
            {
                setCurrentGroup(payload)
            })
    }, [router.query.grupa])

    return (
        <Grid
            container
            justifyContent={'center'}
            spacing={1}
            sx={{ py: 1, my: 1 }}>
                {
                    groups == null || currentGroup == null ?
                        null :
                        <Grid
                            item>
                                <Button
                                    variant={'contained'}
                                    color={'warning'}
                                    sx={{ color: 'inherit' }}
                                    onClick={() => {
                                        router.push({
                                            pathname: router.pathname,
                                            query: {
                                                ...router.query,
                                                grupa: currentGroup.parentName,
                                                pretraga: null
                                            }
                                        })
                                    }}>
                                        <KeyboardBackspace />
                                </Button>
                        </Grid>
                }
                {
                    groups == null ?
                        <CircularProgress /> :
                        groups.map((g: any) => {
                            return <ProizvodiFilterButton key={g.name} group={g} />
                        })
                }
        </Grid>
    )
}