import { Box, Grid, Pagination, Stack, Typography } from '@mui/material'
import { useEffect, useRef, useState } from 'react'
import { useRouter } from 'next/router'
import { useUser } from '@/app/hooks'
import ProizvodCard from './ProizvodCard'
import { handleApiError, webApi } from '@/api/webApi'
import { IProductDto } from '@/dtos'
import { PayloadPagination as IProductsPagination } from '@/types'
import {
    PAGE_SEGMENTS,
    PAGE_SIZE,
    PRODUCTS_LIST_INITIAL_STATE,
} from '../constants'
import { ProizvodiListContentLoader } from '@/widgets/Proizvodi/ProizvodiList/ui/ProizvodiListContentLoader'

const ProizvodiList = (props: any) => {
    const user = useUser(false, false)
    const router = useRouter()

    const [pagination, setPagination] = useState<
        IProductsPagination | undefined
    >(PRODUCTS_LIST_INITIAL_STATE)
    const [products, setProducts] = useState<IProductDto[] | undefined>(
        PRODUCTS_LIST_INITIAL_STATE
    )
    const [currentPage, setCurrentPage] = useState<number>(
        router.query.page
            ? parseInt(
                  Array.isArray(router.query.page)
                      ? router.query.page[0]
                      : router.query.page.toString()
              )
            : 1
    )

    const controller = useRef(new AbortController())

    const fetchProducts = () => {
        setProducts(PRODUCTS_LIST_INITIAL_STATE)
        setPagination(PRODUCTS_LIST_INITIAL_STATE)

        controller.current.abort()
        controller.current = new AbortController()
        props.onStartedLoading()

        const fetchProductsSegments = Array.from(
            { length: PAGE_SEGMENTS },
            (_, i) => {
                return fetchProductsSegment(
                    Math.floor(PAGE_SIZE / PAGE_SEGMENTS),
                    i + currentPage
                )
            }
        )

        fetchProductsSegments.map((fetchProductsSegment) =>
            fetchProductsSegment
                .then((res) => {
                    setProducts((prev) => [...(prev ?? []), ...res.payload])
                    setPagination(res.pagination)
                })
                .catch((err) => handleApiError(err))
        )

        Promise.all(fetchProductsSegments).finally(() =>
            props.onFinishedLoading()
        )
    }

    const fetchProductsSegment = async (pageSize: number, cPage: number) => {
        const products = await webApi.get('/products', {
            signal: controller.current.signal,
            params: {
                pageSize: pageSize,
                currentPage: cPage,
                groupName: props.currentGroup?.name,
                KeywordSearch: router.query.pretraga,
            },
        })
        return products.data
    }

    useEffect(() => {
        if (!fetchProducts) return

        fetchProducts()
    }, [props.currentGroup, currentPage])

    useEffect(() => {
        if (parseInt(router.query.page as string) === 1) {
            fetchProducts()
        } else {
            handlePageChange(1) // this will trigger fetchProducts
        }
    }, [router.query.pretraga])

    const handlePageChange = (page: number) => {
        router.push({
            pathname: router.asPath.split('?')[0],
            query: {
                ...router.query,
                page: page.toString(),
            },
        })
        setCurrentPage(page)
    }

    return (
        <Box>
            {!products || !pagination ? (
                <Grid justifyContent={`center`} container>
                    {[...Array(40)].map((_, index) => (
                        <ProizvodiListContentLoader key={index} />
                    ))}
                </Grid>
            ) : products.length === 0 ? (
                <Typography p={2}>Nema proizvoda za prikazivanje</Typography>
            ) : (
                <>
                    <Grid justifyContent={`center`} container>
                        {products.map((p) => (
                            <ProizvodCard
                                currentGroup={props.currentGroup}
                                key={`proizvod-card-${p.src}`}
                                proizvod={p}
                                user={user}
                            />
                        ))}
                    </Grid>
                    <Stack alignItems="center" sx={{ m: 5 }}>
                        <Pagination
                            onChange={(event, page) => {
                                handlePageChange(page)
                            }}
                            page={currentPage}
                            size={`large`}
                            count={pagination.totalPages}
                            variant={`outlined`}
                        />
                    </Stack>
                </>
            )}
        </Box>
    )
}

export default ProizvodiList
