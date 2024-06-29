import { ApiBase, fetchApi } from "@/app/api"
import { KeyboardBackspace } from "@mui/icons-material"
import { Button, CircularProgress, Grid } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"
import { ProizvodiFilterButton } from "./ProizvodiFilterButton"
import { toast } from "react-toastify"
import { useUser } from "@/app/hooks"

export const ProizvodiFilter = (props: any): JSX.Element => {

    const user = useUser(false, false)
    const router = useRouter()
    const [groups, setGroups] = useState<any | undefined>(null)

    useEffect(() => {
        setGroups(null)

        let url = `/products-groups`
        if(router.query.grupa != null && router.query.grupa !== 'undefined' && router.query.grupa !== 'null' && router.query.grupa !== '' && router.query.grupa != undefined)
            url += `?parentName=${router.query.grupa}`

        fetchApi(ApiBase.Main, url)
            .then((payload) => payload.json().then((r: any) => setGroups(r)))
    }, [router.query.grupa])

    useEffect(() => {
        if(props.currentGroup == null || props.currentGroup.welcomeMessage == null || props.currentGroup.welcomeMessage.length === 0)
            return

        toast.info(props.currentGroup.welcomeMessage)
    }, [props.currentGroup])

    return (
        <Grid
            container
            justifyContent={'center'}
            spacing={1}
            sx={{ py: 1, my: 1 }}>
                {
                    groups == null || props.currentGroup == null ?
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
                                                grupa: props.currentGroup.parentName,
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
                {
                    user?.isLogged  &&
                    <Grid item>
                        <Button
                            variant={'contained'}
                            color={`success`}
                            onClick={() => {
                                router.push("/proizvodi/omiljeni")
                            }}>
                                Omiljeni
                        </Button>
                    </Grid>
                }
        </Grid>
    )
}