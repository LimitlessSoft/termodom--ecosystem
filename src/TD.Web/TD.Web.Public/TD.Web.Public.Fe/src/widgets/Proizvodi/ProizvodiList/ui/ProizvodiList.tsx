import {
    Box,
    CircularProgress,
    Grid,
    Pagination,
    Stack,
    Typography,
} from '@mui/material'
import { ChangeEvent, useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { useUser } from '@/app/hooks'
import { ProizvodCard } from './ProizvodCard'
import { handleApiError, webApi } from '@/api/webApi'
import { IProductDto } from '@/dtos'
import { IProductsPagination } from '../interfaces/IProductsPagination'
import { PRODUCTS_LIST_INITIAL_STATE } from '../constants'

export const ProizvodiList = (props: any): JSX.Element => {
    const user = useUser(false, false)
    const router = useRouter()

    const pageSize = 40

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

    useEffect(() => {
        setProducts(PRODUCTS_LIST_INITIAL_STATE)
        setPagination(PRODUCTS_LIST_INITIAL_STATE)

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
            .catch((err) => handleApiError(err))
    }, [props.currentGroup, router.query.pretraga, currentPage])

    const handlePageChange = (_event: ChangeEvent<unknown>, page: number) => {
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
                <CircularProgress />
            ) : products.length === 0 ? (
                <Typography p={2}>Nema proizvoda za prikazivanje</Typography>
            ) : (
                <>
                    <Grid justifyContent="center" container>
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
                            onChange={handlePageChange}
                            page={currentPage}
                            size="large"
                            count={pagination.totalPages}
                            variant="outlined"
                        />
                    </Stack>
                </>
            )}
        </Box>
    )
}
