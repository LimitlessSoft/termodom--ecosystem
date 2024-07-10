import { ApiBase, fetchApi } from "@/app/api"
import { Box, CircularProgress, Grid, Pagination, Stack } from "@mui/material"
import { useEffect,  useState } from "react"
import { useRouter } from "next/router"
import { useUser } from "@/app/hooks"
import { ProizvodCard } from "./ProizvodCard"

export const ProizvodiList = (props: any): JSX.Element => {

    const user = useUser(false, false)
    const router = useRouter()

    console.log(user)

    const pageSize = 20

    const [pagination, setPagination] = useState<any | undefined>(null)
    const [products, setProducts] = useState<any | undefined>(null)
    const [currentPage, setCurrentPage] = useState<number>(1)

    useEffect(() => {
        let cp = router.query.page;
        setCurrentPage(cp == null ? 1 : parseInt(cp.toString()))
    }, [router])


    const ucitajProizvode = (page: number, grupa?: string, pretraga?: string) => {

        setProducts(null)
        let url = `/products?pageSize=${pageSize}&currentPage=${page}`
        if(grupa != null && grupa !== 'undefined' && grupa !== 'null' && grupa !== '' && grupa != undefined)
            url += `&groupName=${grupa}`

        if(pretraga != null && pretraga !== 'undefined' && pretraga !== 'null' && pretraga !== '' && pretraga != undefined)
            url += `&KeywordSearch=${pretraga}`

        fetchApi(ApiBase.Main, url, undefined)
        .then((response: any) => response.json())
        .then((data: any) => {
            setProducts(data.payload)
            setPagination(data.pagination)
        })
    }

    useEffect(() => {
        if(user.isLoading)
            return

        ucitajProizvode(currentPage, router.query.grupa?.toString(), router.query.pretraga?.toString())
    }, [user, currentPage, router.query.grupa, router.query.pretraga])

    return (
        <Box
            sx={{
                width: '100%',
                my: 2
            }}>
                {
                    products == null || pagination == null ?
                        <CircularProgress /> :
                        <Box>
                            <Grid
                                justifyContent={'center'}
                                container>
                                    {
                                        products.map((p: any) => {
                                            return <ProizvodCard currentGroup={props.currentGroup} key={`proizvod-card-` + p.src} proizvod={p} user={user} />
                                        })
                                    }
                            </Grid>
                            <Stack
                                alignItems={'center'}
                                sx={{
                                    m: 5,
                                }}>
                                    <Pagination
                                        onChange={(event, page) => {
                                            router.push({
                                                pathname: router.pathname,
                                                query: { ...router.query, page: page.toString() }
                                            })
                                        }}
                                        page={currentPage}
                                        size={'large'}
                                        count={pagination.totalPages}
                                        variant={'outlined'} />
                            </Stack>
                        </Box>
                }
        </Box>
    )
}