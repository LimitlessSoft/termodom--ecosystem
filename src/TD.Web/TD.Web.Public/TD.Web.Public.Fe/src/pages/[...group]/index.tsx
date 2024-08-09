import { webApi } from '@/api/webApi'
import { useUser } from '@/app/hooks'
import { ProizvodCard } from '@/widgets/Proizvodi/ProizvodiList/ui/ProizvodCard'
import { Box, CircularProgress, Grid, Pagination, Stack } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { ProizvodiFilter } from '@/widgets/Proizvodi/ProizvodiFilter'
import { IServerSideProps } from '@/interfaces/IServerSideProps'
import { IProductGroupDto } from '@/dtos'
import { buildServerSideProps } from '@/helpers/serverSideHelpers'

export async function getServerSideProps(context: any) {
    const group = context.params.group.pop()

    const props: IServerSideProps<IProductGroupDto> = {
        data: null,
        statusCode: null,
    }

    await webApi
        .get(`/products-groups/${group}`)
        .then((responseData) => {
            props.data = responseData.data
        })
        .catch((err) => {
            props.statusCode = err.response.status
        })

    return buildServerSideProps(props)
}

const Group = (props: any) => {
    const router = useRouter()
    const user = useUser(false, false)
    const [currentPage, setCurrentPage] = useState(
        router.query.page
            ? parseInt(
                  Array.isArray(router.query.page)
                      ? router.query.page[0]
                      : router.query.page.toString()
              )
            : 1
    )
    const [pagination, setPagination] = useState<any | undefined>(undefined)
    const [products, setProducts] = useState<any | undefined>([])

    const pageSize = 40
    useEffect(() => {
        webApi
            .get(
                `/products?pageSize=${pageSize}&currentPage=${currentPage}&groupName=${props.data.name}`
            )
            .then((res) => {
                setProducts(res.data.payload)
                setPagination(res.data.pagination)
            })
    }, [props.data, currentPage])

    return (
        <Grid>
            <Box
                sx={{
                    width: '100%',
                    my: 2,
                }}
            >
                <ProizvodiFilter currentGroup={props.data} />
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
                                        pathname: router.asPath.split('?')[0],
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
                )}
            </Box>
        </Grid>
    )
}

export default Group
