import { Box, CircularProgress, Grid, Pagination, Stack } from '@mui/material'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { useUser } from '@/app/hooks'
import { ProizvodCard } from './ProizvodCard'
import { webApi } from '@/api/webApi'

export const ProizvodiList = (props: any): JSX.Element => {
    const user = useUser(false, false)
    const router = useRouter()

    const pageSize = 40

    const [pagination, setPagination] = useState<any | undefined>(null)
    const [products, setProducts] = useState<any | undefined>([])
    const [currentPage, setCurrentPage] = useState<number>(
        router.query.page
            ? parseInt(
                  Array.isArray(router.query.page)
                      ? router.query.page[0]
                      : router.query.page.toString()
              )
            : 1
    )

    useEffect(() => {
        webApi
            .get('/products', {
                params: {
                    pageSize,
                    currentPage,
                    groupName: props.currentGroup?.name,
                    KeywordSearch: router.query.pretraga,
                },
            })
            .then((res) => {
                setProducts(res.data.payload)
                setPagination(res.data.pagination)
            })
    }, [props.currentGroup, router.query.pretraga, currentPage])

    return (
        <Box
            sx={{
                width: '100%',
                my: 2,
            }}
        >
            {pagination && products.length > 0 ? (
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
                                setCurrentPage(page)
                            }}
                            page={currentPage}
                            size={'large'}
                            count={pagination.totalPages}
                            variant={'outlined'}
                        />
                    </Stack>
                </Box>
            ) : (
                <CircularProgress />
            )}
        </Box>
    )
}
