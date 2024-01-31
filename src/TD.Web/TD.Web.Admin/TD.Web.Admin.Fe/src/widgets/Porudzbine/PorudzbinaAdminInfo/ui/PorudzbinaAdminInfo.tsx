import { Grid, Typography, styled } from "@mui/material"
import { IPorudzbinaAdminInfoProps } from "../models/IPorudzbinaAdminInfoProps"

export const PorudzbinaAdminInfo = (props: IPorudzbinaAdminInfoProps): JSX.Element => {

    const LabelStyled = styled(`span`)(
        ({ theme }) => (`
            color: #777;
        `)
    )

    const TypographyStyled = styled(Typography)(
        ({ theme }) => (`
            font-weight: 600;
        `)
    )

    return (
        <Grid
            sx={{
                m: 2
            }}>
            <TypographyStyled>
                <LabelStyled>Referent obrade:</LabelStyled> {props.porudzbina.referent}
            </TypographyStyled>
            <TypographyStyled>
                <LabelStyled>Kupac je ostavio napomenu:</LabelStyled> {props.porudzbina.note}
            </TypographyStyled>
            <TypographyStyled>
                <LabelStyled>Mobilni telefon kupca:</LabelStyled> {props.porudzbina.mobile}
            </TypographyStyled>
            <TypographyStyled>
                <LabelStyled>Ime kupca:</LabelStyled> {props.porudzbina.name} (profi / jednokratni)
            </TypographyStyled>
        </Grid>
    )
}