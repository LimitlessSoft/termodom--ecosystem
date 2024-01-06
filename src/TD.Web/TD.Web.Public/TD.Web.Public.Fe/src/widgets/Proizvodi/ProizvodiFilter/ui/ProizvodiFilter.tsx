import { ApiBase, fetchApi } from "@/app/api"
import { Box, Button, Grid, LinearProgress, Stack } from "@mui/material"
import { useEffect, useState } from "react"

export const ProizvodiFilter = (): JSX.Element => {

    const [groups, setGroups] = useState<any | undefined>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, "/products-groups")
        .then((payload) => setGroups(payload))
    }, [])

    return (
        <Grid
            container
            justifyContent={'center'}
            spacing={1}
            sx={{ p: 1 }}>
                {
                    groups == null ?
                        <LinearProgress /> :
                        groups.map((g: any) => {
                            return (
                                <Grid
                                    item
                                    key={`product-group-btn-${g.id}`}>
                                        <Button
                                            variant={'contained'}
                                            color={'warning'}
                                            sx={{ color: 'inherit' }}>
                                                {g.name}
                                        </Button>
                                </Grid>)
                        })
                }
        </Grid>
    )
}