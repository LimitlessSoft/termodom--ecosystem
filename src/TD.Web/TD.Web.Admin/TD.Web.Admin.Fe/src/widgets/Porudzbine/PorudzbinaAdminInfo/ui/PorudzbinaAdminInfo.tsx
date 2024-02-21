import { IPorudzbinaAdminInfoProps } from "../models/IPorudzbinaAdminInfoProps"
import { Grid, Typography, styled } from "@mui/material"

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
                {
                    props.porudzbina.referent == null ? null :
                    <TypographyStyled
                        sx={{
                            my: 2
                        }}>
                        <LabelStyled>Referent obrade:</LabelStyled> {props.porudzbina.referent.name}
                    </TypographyStyled>
                }
                <TypographyStyled>
                    <LabelStyled>Kupac je ostavio napomenu:</LabelStyled> {props.porudzbina.note}
                </TypographyStyled>
                <TypographyStyled>
                    <LabelStyled>Mobilni telefon kupca:</LabelStyled> {props.porudzbina.userInformation.mobile}
                </TypographyStyled>
                <TypographyStyled>
                    <LabelStyled>Ime kupca:</LabelStyled> {props.porudzbina.userInformation.name} ({ props.porudzbina.userInformation.id == null ? "jednokratni" : "profi"})
                </TypographyStyled>
        </Grid>
    )
}