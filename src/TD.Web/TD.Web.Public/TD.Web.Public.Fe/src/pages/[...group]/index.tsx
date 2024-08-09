import { webApi } from '@/api/webApi'
import { useUser } from '@/app/hooks'
import { ProizvodCard } from '@/widgets/Proizvodi/ProizvodiList/ui/ProizvodCard'
import { Box, CircularProgress, Grid, Pagination, Stack } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { ProizvodiFilter } from '@/widgets/Proizvodi/ProizvodiFilter'

export async function getServerSideProps(context: any) {
    const group = context.params.group.pop()

    const response = await webApi
        .get(`/products-groups/${group}`)
        .then((responseData) => responseData.data)
        .catch((err) => err.response)

    if (response.status == 404) {
        return {
            notFound: true,
        }
    }

    return {
        props: {
            error: {
                message: response.statusText,
                status: response.status,
            },
            group: response,
        },
    }
}

const Group = (props: any) => {
    const user = useUser(false, false)
    const [currentPage, setCurrentPage] = useState(1)
    const [pagination, setPagination] = useState<any | undefined>(undefined)
    const [products, setProducts] = useState<any | undefined>([])

    const router = useRouter()

    const pageSize = 40
    useEffect(() => {
        webApi
            .get(
                `/products?pageSize=${pageSize}&currentPage=${currentPage}&groupName=${props.group.name}`
            )
            .then((res) => {
                setProducts(res.data.payload)
                setPagination(res.data.pagination)
            })
    }, [props.group, currentPage])

    return (
        <Grid>
            <Box
                sx={{
                    width: '100%',
                    my: 2,
                }}
            >
                <ProizvodiFilter currentGroup={props.group} />
                {products.length === 0 || !pagination ? (
                    <CircularProgress />
                ) : (
                    <Box>
                        <Grid justifyContent={'center'} container>
                            {products.map((p: any) => {
                                return (
                                    <ProizvodCard
                                        currentGroup={props.currentGroup}
                                        key={`proizvod-card-` + p.src}
                                        proizvod={p}
                                        user={user}
                                    />
                                )
                            })}
                        </Grid>
                        <Stack
                            alignItems={'center'}
                            sx={{
                                m: 5,
                            }}
                        >
                            <Pagination
                                onChange={(event, page) => {
                                    router.push({
                                        pathname: router.pathname,
                                        query: {
                                            ...router.query,
                                            page: page.toString(),
                                        },
                                    })
                                }}
                                page={currentPage}
                                size={'large'}
                                count={pagination.totalPages}
                                variant={'outlined'}
                            />
                        </Stack>
                    </Box>
                )}
            </Box>
        </Grid>
    )
}

export default Group
