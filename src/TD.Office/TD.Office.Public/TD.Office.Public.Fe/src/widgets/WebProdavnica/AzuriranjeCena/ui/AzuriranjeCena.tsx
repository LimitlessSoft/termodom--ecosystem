
import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar";
import { Button, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography, styled } from "@mui/material"
import { useState } from "react";
import { toast } from "react-toastify";
import { AzurirajCeneKomercijalnoPoslovanjaDialog } from "./AzurirajCeneKomercijalnoPoslovanjaDialog";

export const AzuriranjeCena = (): JSX.Element => {

    const [isOpenAzurirajCeneKomercijalnoPoslovanjaDialog, setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog] = useState<boolean>(false);
    const AzuriranjeCenaStyled = styled(Grid)(
        ({ theme }) => `
            padding: 1rem;
        `
    );

    return (
        <AzuriranjeCenaStyled container direction={`column`}>
            <AzurirajCeneKomercijalnoPoslovanjaDialog isOpen={isOpenAzurirajCeneKomercijalnoPoslovanjaDialog} handleClose={(nastaviAkciju: boolean) => {
                if(nastaviAkciju) {
                    toast.warning(`Ova funkcionalnost još uvek nije implementirana.`)
                }

                setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog(false)
            }} />
            <Grid>
                <Typography variant={`h4`} >Ažuriranje cena</Typography>
            </Grid>
            <Grid container>
                <HorizontalActionBar>
                    <HorizontalActionBarButton text="Ažuriraj 'Max Web Cene'" onClick={() => {
                        toast.warning(`Ova funkcionalnost još uvek nije implementirana.`)
                    }} />
                    <HorizontalActionBarButton text="Azuriraj cene komercijalnog poslovanja" onClick={() => {
                        setIsOpenAzurirajCeneKomercijalnoPoslovanjaDialog(true)
                    }} />
                </HorizontalActionBar>
            </Grid>
            <Grid container>
                <Typography variant={`body2`}>Cene komercijalnog poslovanja ažurirane: 12.12.2024</Typography>
            </Grid>
            <Grid sx={{ py: `1rem` }} container>
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
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            <TableRow>
                                <TableCell align="center">Proizvod 1</TableCell>
                                <TableCell align="center">1000</TableCell>
                                <TableCell align="center">1500</TableCell>
                                <TableCell align="center">800</TableCell>
                                <TableCell align="center">1200</TableCell>
                                <TableCell align="center"><Button color={`info`} variant={`contained`} onClick={() => {
                                    toast.warning(`Ova funkcionalnost još uvek nije implementirana.`)
                                }}>Nabavna cena * 1.2</Button></TableCell>
                                <TableCell align="center">800</TableCell>
                                <TableCell align="center">1000</TableCell>
                                <TableCell align="center">1200</TableCell>
                                <TableCell align="center">14000</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
            </Grid>
        </AzuriranjeCenaStyled>
    )
}