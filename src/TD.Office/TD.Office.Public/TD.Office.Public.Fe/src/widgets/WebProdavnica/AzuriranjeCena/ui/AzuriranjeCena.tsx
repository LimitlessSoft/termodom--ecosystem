
import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar";
import { Button, CircularProgress, Grid, LinearProgress, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography, styled } from "@mui/material"
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { AzurirajCeneKomercijalnoPoslovanjaDialog } from "./AzurirajCeneKomercijalnoPoslovanjaDialog";
import { ApiBase, ContentType, fetchApi } from "@/app/api";
import { AzuriranjeCenaPovezanRobaIdDialog } from "./AzuriranjeCenaPovezanRobaIdDialog";

interface DataDto {
    id: number;
    naziv: string;
    minWebOsnova: number;
    maxWebOsnova: number;
    nabavnaCenaKomercijalno: number;
    prodajnaCenaKomercijalno: number;
    ironCena: number;
    silverCena: number;
    goldCena: number;
    platinumCena: number;
    linkRobaId: number | null;
    linkId?: number;
}

interface LinkRequest {
    id?: number;
    robaId?: number;
    webId: number;
}

export const AzuriranjeCena = (): JSX.Element => {

    const [isOpenAzurirajCeneKomercijalnoPoslovanjaDialog, setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog] = useState<boolean>(false);
    const [isOpenAzuriranjeLinkRobaId, setIsOpenAzuriranjeLinkRobaId] = useState<boolean>(false);
    const [currentLink, setCurrentLink] = useState<LinkRequest | null>(null)
    const [currentLinkNaziv, setCurrentLinkNaziv] = useState<string | null>(null)
    const [currentLinkRobaId, setCurrentLinkRobaId] = useState<number | null>(null)

    const AzuriranjeCenaStyled = styled(Grid)(
        ({ theme }) => `
            padding: 1rem;
        `
    );

    const [data, setData] = useState<DataDto[] | null>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, '/web-azuriranje-cena')
            .then((response) => {
                setData(response)
            })
    }, [])


    return (
        <AzuriranjeCenaStyled container direction={`column`}>
            <AzurirajCeneKomercijalnoPoslovanjaDialog isOpen={isOpenAzurirajCeneKomercijalnoPoslovanjaDialog} handleClose={(nastaviAkciju: boolean) => {
                if(nastaviAkciju) {
                    fetchApi(ApiBase.Main, '/web-azuriraj-cene-komercijalno-poslovanje', {
                        method: 'POST',
                    }).then(() => {
                        toast.success(`Uspešno ažurirane cene komercijalnog poslovanja!`)
                    })
                }

                setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog(false)
            }} />

            <AzuriranjeCenaPovezanRobaIdDialog
                currentRobaId={currentLinkRobaId ?? 0}
                isOpen={isOpenAzuriranjeLinkRobaId}
                naziv={currentLinkNaziv ?? 'undefined'}
                handleClose={(value: number | null) => {
                if(value != null) {
                    fetchApi(ApiBase.Main, '/web-azuriraj-cene-komercijalno-poslovanje-povezi-proizvode', {
                        method: 'PUT',
                        body: JSON.stringify(currentLink),
                        contentType: ContentType.ApplicationJson
                    })
                    .then(() => {
                        toast.success(`Uspešno ažuriran povezan RobaId!`)
                    })
                }

                setIsOpenAzuriranjeLinkRobaId(false)
            }} />

            <Grid>
                <Typography variant={`h4`} >Ažuriranje cena</Typography>
            </Grid>
            <Grid container>
                {
                    data == null ?
                        <CircularProgress /> :
                        <HorizontalActionBar>
                            <HorizontalActionBarButton text="Ažuriraj 'Max Web Cene'" onClick={() => {
                                toast.warning(`Ova funkcionalnost još uvek nije implementirana.`)
                            }} />
                            <HorizontalActionBarButton text="Azuriraj cene komercijalnog poslovanja" onClick={() => {
                                setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog(true)
                            }} />
                        </HorizontalActionBar>
                }
            </Grid>
            <Grid container>
                {
                    data == null ?
                        <CircularProgress /> :
                        <Typography variant={`body2`}>Cene komercijalnog poslovanja ažurirane: 12.12.2024</Typography>
                }
            </Grid>
            <Grid sx={{ py: `1rem` }} container>
                {
                    data == null ?
                        <CircularProgress /> :
                        <TableContainer component={Paper}>
                            <Table sx={{ minWidth: 650 }} aria-label="Proizvodi">
                                <TableHead>
                                    <TableRow>
                                        <TableCell align="center">Naziv</TableCell>
                                        <TableCell align="center">Min Web Osnova</TableCell>
                                        <TableCell align="center">Max Web Osnova</TableCell>
                                        <TableCell align="center">Nabavna Cena Komercijalno</TableCell>
                                        <TableCell align="center">Prodajna Cena Komercijalno</TableCell>
                                        <TableCell align="center">Uslov formiranja Min Web Cene</TableCell>
                                        <TableCell align="center">Platinum cena</TableCell>
                                        <TableCell align="center">Gold cena</TableCell>
                                        <TableCell align="center">Silver cena</TableCell>
                                        <TableCell align="center">Iron cena</TableCell>
                                        <TableCell align="center">Povezan RobaId</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                {
                                    data.map((dto) => {
                                        return (
                                            <TableRow key={dto.naziv}>
                                                <TableCell align="center">{dto.naziv}</TableCell>
                                                <TableCell align="center">{dto.minWebOsnova}</TableCell>
                                                <TableCell align="center">{dto.maxWebOsnova}</TableCell>
                                                <TableCell align="center">{dto.nabavnaCenaKomercijalno}</TableCell>
                                                <TableCell align="center">{dto.prodajnaCenaKomercijalno}</TableCell>
                                                <TableCell align="center"><Button color={`info`} variant={`contained`} onClick={() => {
                                                    toast.warning(`Ova funkcionalnost još uvek nije implementirana.`)
                                                }}>Nabavna cena * 1.2</Button></TableCell>
                                                <TableCell align="center">{dto.platinumCena}</TableCell>
                                                <TableCell align="center">{dto.goldCena}</TableCell>
                                                <TableCell align="center">{dto.silverCena}</TableCell>
                                                <TableCell align="center">{dto.ironCena}</TableCell>
                                                <TableCell align="center">
                                                    <Button color={`info`} variant={`contained`} onClick={() => {
                                                        setCurrentLinkNaziv(dto.naziv)
                                                        setCurrentLinkRobaId(dto.linkRobaId)
                                                        setIsOpenAzuriranjeLinkRobaId(true)
                                                    }}>
                                                        {
                                                            dto.linkRobaId == null ?
                                                                'Nije povezan' :
                                                                <Typography>RobaId: {dto.linkRobaId}</Typography>
                                                        }
                                                </Button></TableCell>
                                            </TableRow>
                                        )
                                    })
                                }
                                </TableBody>
                            </Table>
                        </TableContainer>
                }
            </Grid>
        </AzuriranjeCenaStyled>
    )
}