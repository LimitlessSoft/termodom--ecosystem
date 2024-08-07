import { webApi } from '@/api/webApi'
import { useUser } from '@/app/hooks'
import { ProizvodCard } from '@/widgets/Proizvodi/ProizvodiList/ui/ProizvodCard'
import { Box, CircularProgress, Grid, Pagination, Stack } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'

export async function getServerSideProps(context: any) {
    const { group } = context.params

    const res = await webApi.get('/products-groups')
    const groups = res.data.map((group: any) => group.name)

    const groupExists = groups.some(
        (propGroup: string) => propGroup.toLowerCase() === group.toLowerCase()
    )

    if (!groupExists) {
        return {
            notFound: true,
        }
    }

    return {
        props: {
            group,
        },
    }
}

const Group = (props: any) => {
    const user = useUser(false, false)
    const [currentPage, setCurrentPage] = useState(1)
    const [pagination, setPagination] = useState<any | undefined>(undefined)
    const [products, setProducts] = useState<any | undefined>([])

    const router = useRouter()
    const { group } = router.query

    const pageSize = 40
    useEffect(() => {
        if (group) {
            webApi
                .get(
                    `/products?pageSize=${pageSize}&currentPage=${currentPage}&groupName=${group[0]!.toUpperCase() + group?.slice(1)}`
                )
                .then((res) => {
                    setProducts(res.data.payload)
                    setPagination(res.data.pagination)
                })
        }
    }, [group, currentPage])

    return (
        <Grid>
            <Box
                sx={{
                    width: '100%',
                    my: 2,
                }}
            >
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
