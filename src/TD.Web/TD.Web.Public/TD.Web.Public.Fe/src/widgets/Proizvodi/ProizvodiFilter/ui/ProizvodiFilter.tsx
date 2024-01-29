import { ApiBase, fetchApi } from "@/app/api"
import { KeyboardBackspace } from "@mui/icons-material"
import { Box, Button, CircularProgress, Grid, LinearProgress, Stack } from "@mui/material"
import { useRouter } from "next/router"
import { use, useEffect, useState } from "react"

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
            sx={{ p: 1, my: 1 }}>
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
                                                grupa: currentGroup.parentName
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
                            return (
                                <Grid
                                    item
                                    key={`product-group-btn-${g.id}`}>
                                        <Button
                                            variant={'contained'}
                                            color={'warning'}
                                            sx={{ color: 'inherit' }}
                                            onClick={() => {
                                                router.push({
                                                    pathname: router.pathname,
                                                    query: {
                                                        ...router.query,
                                                        grupa: g.name
                                                    }
                                                })
                                            }}>
                                                {g.name}
                                        </Button>
                                </Grid>)
                        })
                }
        </Grid>
    )
}