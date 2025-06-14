import { handleApiError, webApi } from '@/api/webApi'
import { DefaultMetadataTitle } from '@/app/constants'
import { useUser } from '@/app/hooks'
import { IProductGroupDto } from '@/dtos'
import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import { CustomHead } from '@/widgets/CustomHead'
import { ModKupovinePoruka } from '@/widgets/ModKupovinePoruka'
import { ProizvodiFilter } from '@/widgets/Proizvodi/ProizvodiFilter'
import { ProizvodiList } from '@/widgets/Proizvodi/ProizvodiList'
import { ProizvodiSearch } from '@/widgets/Proizvodi/ProizvodiSearch'
import { PhoneEnabled } from '@mui/icons-material'
import { Alert, Button, Grid, Stack, Typography } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'

const Group = (props) => {
    const router = useRouter()
    const user = useUser(false, false)
    const [group, setGroup] = useState()

    const [isLoading, setIsLoading] = useState(false)

    useEffect(() => {
        if (props.isHomePage || !router.query.group) return

        if (!Array.isArray(router.query.group))
            throw new Error('Expected array got string')

        webApi
            .get(`/products-groups/${router.query.group?.pop()}`)
            .then((responseData) => setGroup(responseData.data))
            .catch((err) => handleApiError(err))
    }, [router.query.group])

    if (!props.isHomePage && !group) return <></>

    return (
        <CenteredContentWrapper>
            <CustomHead />
            <Stack width={'100%'} direction={'column'}>
                {/* Used for SEO purposes */}
                <Typography hidden variant={'h6'} component={`h1`}>
                    Termodom Web Prodavnica
                </Typography>
                <Typography hidden variant={'h6'} component={`h2`}>
                    {DefaultMetadataTitle}
                </Typography>
                <ProizvodiFilter currentGroup={group} />
                <ModKupovinePoruka />
                {!user ||
                    (!user.isLogged && (
                        <Grid
                            sx={{
                                display: `flex`,
                                mx: 2,
                                mb: 2,
                                justifyContent: {
                                    xs: `stretch`,
                                    md: `left`,
                                },
                            }}
                        >
                            <Alert
                                severity={`info`}
                                variant={`filled`}
                                elevation={1}
                                sx={{
                                    fontSize: 16,
                                    justifyContent: `center`,
                                    width: {
                                        xs: '100%',
                                        sm: 'max-content',
                                    },
                                }}
                            >
                                <Typography>Cena zavisi od količine</Typography>
                            </Alert>
                        </Grid>
                    ))}
                <Grid
                    sx={{
                        display: `flex`,
                        flexDirection: {
                            xs: `column`,
                            md: `row`,
                        },
                        justifyContent: `space-between`,
                        alignItems: {
                            xs: `start`,
                            md: `center`,
                        },
                        gap: 2,
                        mx: 2,
                        mb: 1,
                    }}
                >
                    <Grid item>
                        <ProizvodiSearch disabled={isLoading} />
                    </Grid>
                    <Grid item>
                        {group?.salesMobile && (
                            <Grid
                                container
                                alignItems={`center`}
                                gap={2}
                                justifyContent={`space-between`}
                            >
                                <Grid item>
                                    <Typography>Kontakt trgovac:</Typography>
                                </Grid>
                                <Grid item>
                                    <Button
                                        variant={`contained`}
                                        color={`info`}
                                        href={`tel:${group.salesMobile}`}
                                        endIcon={<PhoneEnabled />}
                                    >
                                        <Typography component={`span`}>
                                            {group.salesMobile}
                                        </Typography>
                                    </Button>
                                </Grid>
                            </Grid>
                        )}
                    </Grid>
                </Grid>
                <ProizvodiList
                    onStartedLoading={() => {
                        setIsLoading(true)
                    }}
                    onFinishedLoading={() => {
                        setIsLoading(false)
                    }}
                    currentGroup={group}
                />
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Group
