import { DefaultMetadataTitle } from '@/app/constants'
import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import { CustomHead } from '@/widgets/CustomHead'
import { Button, Grid, Stack, Typography } from '@mui/material'
import { ProizvodiFilter } from '../../ProizvodiFilter'
import { ModKupovinePoruka } from '@/widgets/ModKupovinePoruka'
import { ProizvodiSearch } from '../../ProizvodiSearch'
import { PhoneEnabled } from '@mui/icons-material'
import { ProizvodiList } from '../../ProizvodiList'

const ProizvodiPage = (props: any) => {
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
                <ProizvodiFilter currentGroup={props.currentGroup} />
                <ModKupovinePoruka />
                <Grid
                    container
                    justifyContent={`space-between`}
                    alignItems={`center`}
                >
                    <Grid item>
                        <ProizvodiSearch />
                    </Grid>
                    <Grid item>
                        {props.currentGroup?.salesMobile && (
                            <Grid
                                container
                                alignItems={`center`}
                                mx={`16px`}
                                gap={2}
                            >
                                <Grid item>
                                    <Typography>Kontakt trgovac:</Typography>
                                </Grid>
                                <Grid item>
                                    <Button
                                        variant={`contained`}
                                        color={`info`}
                                        href={`tel:${props.currentGroup.salesMobile}`}
                                        endIcon={<PhoneEnabled />}
                                    >
                                        <Typography component={`span`}>
                                            {props.currentGroup.salesMobile}
                                        </Typography>
                                    </Button>
                                </Grid>
                            </Grid>
                        )}
                    </Grid>
                </Grid>
                <ProizvodiList currentGroup={props.currentGroup} />
            </Stack>
        </CenteredContentWrapper>
    )
}

export default ProizvodiPage
