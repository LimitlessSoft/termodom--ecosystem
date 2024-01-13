import { Button, CircularProgress, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography, styled } from "@mui/material"
import { AzurirajCeneKomercijalnoPoslovanjaDialog } from "./AzurirajCeneKomercijalnoPoslovanjaDialog";
import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar";
import { AzuriranjeCenaPovezanCell } from "./AzuriranjeCenaPovezanCell";
import { ApiBase, fetchApi } from "@/app/api";
import { useEffect, useState } from "react";
import { DataDto } from "../models/DataDto";
import { toast } from "react-toastify";

export const AzuriranjeCena = (): JSX.Element => {

    const [isOpenAzurirajCeneKomercijalnoPoslovanjaDialog, setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog] = useState<boolean>(false)

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

            <Grid>
                <Typography variant={`h4`} >Ažuriranje cena</Typography>
            </Grid>
            <Grid container>
                {
                    data == null ?
                        <CircularProgress /> :
                        <HorizontalActionBar>
                            <HorizontalActionBarButton text="Ažuriraj 'Max Web Osnove'" onClick={() => {
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
                                        <TableCell align="center">Trenutna Min Web Osnova</TableCell>
                                        <TableCell align="center">Trenutna Max Web Osnova</TableCell>
                                        <TableCell align="center">Nabavna Cena Komercijalno</TableCell>
                                        <TableCell align="center">Prodajna Cena Komercijalno</TableCell>
                                        <TableCell align="center">Uslov formiranja Min Web Osnove</TableCell>
                                        <TableCell align="center">Trenutna Platinum cena</TableCell>
                                        <TableCell align="center">Trenutna Gold cena</TableCell>
                                        <TableCell align="center">Trenutna Silver cena</TableCell>
                                        <TableCell align="center">Trenutna Iron cena</TableCell>
                                        <TableCell align="center">Trenutno Povezan RobaId</TableCell>
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
                                                    <AzuriranjeCenaPovezanCell data={dto} />
                                                </TableCell>
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