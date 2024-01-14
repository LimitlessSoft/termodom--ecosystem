import { Button, CircularProgress, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography, styled } from "@mui/material"
import { AzurirajCeneKomercijalnoPoslovanjaDialog } from "./AzurirajCeneKomercijalnoPoslovanjaDialog";
import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar";
import { AzuriranjeCenaPovezanCell } from "./AzuriranjeCenaPovezanCell";
import { ApiBase, ContentType, fetchApi } from "@/app/api";
import { useEffect, useState } from "react";
import { DataDto } from "../models/DataDto";
import { toast } from "react-toastify";
import { AzuriranjeCenaUslovFormiranjaCell } from "./AzuriranjeCenaUslovFormiranjaCell";
import { AzurirajMaxWebOsnoveDialog } from "./AzurirajMaxWebOsnoveDialog";
import { AzuriranjeCenaTableRow } from "./AzuriranjeCenaTableRow";

export const AzuriranjeCena = (): JSX.Element => {

    const [isOpenAzurirajCeneKomercijalnoPoslovanjaDialog, setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog] = useState<boolean>(false)
    const [data, setData] = useState<DataDto[] | null>(null)
    const [isUpdatingCeneKomercijalnogPoslovanja, setIsUpdatingCeneKomercijalnogPoslovanja] = useState<boolean>(false)
    const [isAzurirajMaxWebOsnoveDialogOpen, setIsAzurirajMaxWebOsnoveDialogOpen] = useState<boolean>(false)
    const [isAzuriranjeMaxWebOsnovaUToku, setIsAzuriranjeMaxWebOsnovaUToku] = useState<boolean>(false)

    const AzuriranjeCenaStyled = styled(Grid)(
        ({ theme }) => `
            padding: 1rem;
        `
    );

    const reloadData = () => {
        setData(null)
        fetchApi(ApiBase.Main, '/web-azuriranje-cena')
            .then((response) => {
                setData(response)
            })
    }

    useEffect(() => {
        reloadData()
    }, [])

    return (
        <AzuriranjeCenaStyled container direction={`column`}>
            <AzurirajCeneKomercijalnoPoslovanjaDialog isOpen={isOpenAzurirajCeneKomercijalnoPoslovanjaDialog} handleClose={(nastaviAkciju: boolean) => {
                if(nastaviAkciju) {
                    setIsUpdatingCeneKomercijalnogPoslovanja(true)
                    fetchApi(ApiBase.Main, '/web-azuriraj-cene-komercijalno-poslovanje', {
                        method: 'POST',
                    }).then(() => {
                        toast.success(`Uspešno ažurirane cene komercijalnog poslovanja!`)
                    }).finally(() => {
                        setIsUpdatingCeneKomercijalnogPoslovanja(false)
                    })
                }

                setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog(false)
            }} />

            <AzurirajMaxWebOsnoveDialog isOpen={isAzurirajMaxWebOsnoveDialogOpen} handleClose={(nastaviAkciju: boolean) => {
                if(nastaviAkciju) {
                    setIsAzuriranjeMaxWebOsnovaUToku(true)

                    const request: AzurirajCeneMaxWebOsnoveRequest = {
                        items: []
                    }

                    data?.map((dto) => [
                        request.items.push({
                            maxWebOsnova: dto.prodajnaCenaKomercijalno,
                            productId: dto.id
                        })
                    ])

                    fetchApi(ApiBase.Main, '/web-azuriraj-cene-max-web-osnove', {
                        method: 'POST',
                        body: request,
                        contentType: ContentType.ApplicationJson
                    }).then(() => {
                        reloadData()
                        toast.success(`Uspešno ažurirane cene max web osnova!`)
                    }).finally(() => {
                        setIsAzuriranjeMaxWebOsnovaUToku(false)
                    })
                }

                setIsAzurirajMaxWebOsnoveDialogOpen(false)
            }} />

            <Grid>
                <Typography variant={`h4`} >Ažuriranje cena</Typography>
            </Grid>
            <Grid container>
                {
                    data == null ?
                        <CircularProgress /> :
                        <HorizontalActionBar>
                            <HorizontalActionBarButton
                                startIcon={isAzuriranjeMaxWebOsnovaUToku ? <CircularProgress size={`1em`} /> : null}
                                disabled={isAzuriranjeMaxWebOsnovaUToku}
                                text="Ažuriraj 'Max Web Osnove'" onClick={() => {
                                setIsAzurirajMaxWebOsnoveDialogOpen(true)
                            }} />
                            <HorizontalActionBarButton
                                startIcon={isUpdatingCeneKomercijalnogPoslovanja ? <CircularProgress size={`1em`} /> : null}
                                disabled={isUpdatingCeneKomercijalnogPoslovanja}
                                text="Azuriraj cene komercijalnog poslovanja" onClick={() => {
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
                                        return <AzuriranjeCenaTableRow key={dto.id} data={dto} reloadData={reloadData} />
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