import { STOCK_TYPES, STOCK_TYPES_MESSAGES, TIP_LAGERA } from '../../constants'
import { IPorudzbinaAdminInfoProps } from '../models/IPorudzbinaAdminInfoProps'
import { Alert, Grid, Stack, Typography, styled } from '@mui/material'

export const PorudzbinaAdminInfo = ({
    porudzbina,
    stockTypes,
    isDelivery,
}: IPorudzbinaAdminInfoProps): JSX.Element => {
    const LabelStyled = styled(`span`)(
        ({ theme }) => `
            color: #777;
        `
    )

    const TypographyStyled = styled(Typography)(
        ({ theme }) => `
            font-weight: 600;
        `
    )

    const hasTranzitItem = porudzbina?.items.some((item) => {
        const stockType = stockTypes.find((type) => type.id === item.stockType)
        return stockType?.name === STOCK_TYPES.TRANZIT
    })

    const hasVelikaStovaristaItem = porudzbina?.items.some((item) => {
        const stockType = stockTypes?.find((type) => type.id === item.stockType)
        return stockType?.name === STOCK_TYPES.VELIKA_STOVARISTA
    })

    return (
        <Grid
            container
            p={2}
            alignItems={`start`}
            justifyContent={`space-between`}
            spacing={2}
        >
            <Grid item xs={12} md={4}>
                {porudzbina.referent == null ? null : (
                    <TypographyStyled
                        sx={{
                            my: 2,
                        }}
                    >
                        <LabelStyled>Referent obrade:</LabelStyled>{' '}
                        {porudzbina.referent.name}
                    </TypographyStyled>
                )}
                <TypographyStyled>
                    <LabelStyled>Kupac je ostavio napomenu:</LabelStyled>{' '}
                    {porudzbina.note}
                </TypographyStyled>
                <TypographyStyled>
                    <LabelStyled>Mobilni telefon kupca:</LabelStyled>{' '}
                    {porudzbina.userInformation.mobile}
                </TypographyStyled>
                {porudzbina.deliveryAddress && (
                    <TypographyStyled>
                        <LabelStyled>
                            Kupac je ostavio adresu isporuke:
                        </LabelStyled>{' '}
                        {porudzbina.deliveryAddress}
                    </TypographyStyled>
                )}
                <TypographyStyled>
                    <LabelStyled>Ime kupca:</LabelStyled>{' '}
                    {porudzbina.userInformation.name} (
                    {porudzbina.userInformation.id == null
                        ? 'jednokratni'
                        : 'profi'}
                    )
                </TypographyStyled>
            </Grid>
            <Grid item xs={12} md={8}>
                <Stack direction={`column`} gap={2}>
                    {hasVelikaStovaristaItem && !isDelivery && (
                        <Alert
                            severity={`info`}
                            variant={TIP_LAGERA.ALERT_VARIANT}
                            sx={{ ...TIP_LAGERA.ALERT_ALIGNMENT }}
                        >
                            {STOCK_TYPES_MESSAGES.VELIKA_STOVARISTA_MESSAGE}
                        </Alert>
                    )}
                    {hasTranzitItem && (
                        <Alert
                            severity={`warning`}
                            variant={TIP_LAGERA.ALERT_VARIANT}
                            sx={{ ...TIP_LAGERA.ALERT_ALIGNMENT }}
                        >
                            {STOCK_TYPES_MESSAGES.TRANZIT_MESSAGE}
                        </Alert>
                    )}
                </Stack>
            </Grid>
        </Grid>
    )
}
