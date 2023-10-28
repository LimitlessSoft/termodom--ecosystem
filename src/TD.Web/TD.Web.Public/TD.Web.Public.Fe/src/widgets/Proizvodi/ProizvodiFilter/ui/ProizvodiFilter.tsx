import { ApiBase, fetchApi } from "@/app/api"
import { Box, Button, LinearProgress, Stack } from "@mui/material"
import { useEffect, useState } from "react"

export const ProizvodiFilter = (): JSX.Element => {

    const [groups, setGroups] = useState<any | undefined>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, "/products-groups")
        .then((payload) => setGroups(payload))
    }, [])

    return (
        <Stack
            direction={'row'}
            spacing={1}
            sx={{ m: 2 }}>
                {
                    groups == null ?
                        <LinearProgress /> :
                        groups.map((g: any) => {
                            return (
                            <Button
                                key={`product-group-btn-${g.id}`}
                                variant={'contained'}
                                color={'warning'}
                                sx={{ color: 'inherit' }}>
                                    {g.name}
                            </Button>)

                        })
                }
        </Stack>
    )
}