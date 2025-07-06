import { mainTheme } from '@/app/theme'
import { Replay, Search } from '@mui/icons-material'
import { Button, Grid, IconButton, Paper, Typography } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { ProizvodiSearchInputBaseStyled } from './ProizvodiSearchInputBaseStyled'
import posthog from 'posthog-js'
import { POSTHOG_CONSTANTS } from '@/constants'

export const ProizvodiSearch = (props: any) => {
    const router = useRouter()
    const [searchValue, setSearchValue] = useState<string>('')

    const updateSearchQueryParameter = () => {
        const text = searchValue?.trim()
        if (text?.length > 0) {
            posthog.capture(POSTHOG_CONSTANTS.POSTHOG_PRODUCT_SEARCH_EVENT, {
                text: searchValue?.trim(),
            })
        }
        router.push({
            pathname: router.asPath.split('?')[0],
            query: { ...router.query, pretraga: searchValue?.trim() },
        })
    }

    const resetSearchQueryParameter = () => {
        router.push({
            pathname: router.asPath.split('?')[0],
            query: { ...router.query, pretraga: null },
        })
    }

    useEffect(() => {
        setSearchValue(router.query.pretraga?.toString() ?? '')
    }, [router.query.pretraga])

    return (
        <Grid>
            <Paper
                sx={{
                    p: '2px 0px',
                    display: 'flex',
                    alignItems: 'center',
                    width: 400,
                    maxWidth: `calc(100vw - 32px)`,
                    border: `1px solid ${mainTheme.palette.primary.main}`,
                }}
            >
                <ProizvodiSearchInputBaseStyled
                    id={`proizvodi-search-input`}
                    disabled={props.disabled}
                    sx={{ ml: 1, flex: 1 }}
                    value={searchValue}
                    placeholder="Pretraga svih proizvoda"
                    inputProps={{ 'aria-label': 'Pretraga svih proizvoda' }}
                    onKeyDown={(e) => {
                        if (e.key == 'Enter') {
                            updateSearchQueryParameter()
                        }
                    }}
                    onChange={(e) => {
                        setSearchValue(e.currentTarget.value)
                    }}
                />
                <IconButton
                    type="button"
                    sx={{ p: '10px' }}
                    aria-label="search"
                    disabled={props.disabled}
                    onClick={() => {
                        updateSearchQueryParameter()
                    }}
                >
                    <Search />
                </IconButton>
            </Paper>
            {/*{props.disabled && (*/}
            {/*    <Box p={2}>*/}
            {/*        <LinearProgress />*/}
            {/*    </Box>*/}
            {/*)}*/}
            {router.query.pretraga && router.query.pretraga.length > 0 && (
                <Typography
                    mx={4}
                    my={1}
                    color={mainTheme.palette.secondary.main}
                    variant={`subtitle1`}
                >
                    Rezultati su trenutno filtrirani po pretrazi:{' '}
                    <b>
                        &quot;{router.query.pretraga?.toString() ?? 'Sve grupe'}
                        &quot;
                    </b>
                    <Button
                        startIcon={<Replay />}
                        sx={{
                            py: 1,
                        }}
                        onClick={() => {
                            resetSearchQueryParameter()
                        }}
                    />
                </Typography>
            )}
        </Grid>
    )
}
