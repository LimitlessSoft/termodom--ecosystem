import {
    CircularProgress,
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
    styled,
} from '@mui/material'
import { AzuriranjeCenaPrimeniUsloveDialog } from './AzuriranjeCenaPrimeniUsloveDialog'
import { HorizontalActionBar, HorizontalActionBarButton } from '@/widgets'
import { AzuriranjeCenaTableRow } from './AzuriranjeCenaTableRow'
import { asUtcString } from '@/helpers/dateHelpers'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { DataDto } from '../models/DataDto'
import { toast } from 'react-toastify'
import moment from 'moment'

export const AzuriranjeCena = () => {
    const [data, setData] = useState<DataDto[] | null>(null)
    const [
        isUpdatingCeneKomercijalnogPoslovanja,
        setIsUpdatingCeneKomercijalnogPoslovanja,
    ] = useState<boolean>(false)
    const [isPrimeniUsloveUToku, setIsPrimeniUsloveUToku] =
        useState<boolean>(false)
    const [isPrimeniUsloveDialogOpen, setIsPrimeniUsloveDialogOpen] =
        useState<boolean>(false)
    const [azuriraneKomercijalnoCeneTime, setAzuriraneKomercijalnoCeneTime] =
        useState<Date | null>(null)

    const AzuriranjeCenaStyled = styled(Grid)(
        ({ theme }) => `
            padding: 1rem;
        `
    )

    const reloadData = () => {
        setData(null)

        if (azuriraneKomercijalnoCeneTime != null) {
            loadBaseData()
            return
        }

        officeApi
            .post(`/web-azuriraj-cene-komercijalno-poslovanje`, {})
            .then(() => {
                // toast.success(
                //     `Uspešno ažurirane cene komercijalnog poslovanja!`
                // )
                setAzuriraneKomercijalnoCeneTime(new Date())

                loadBaseData()

                setIsUpdatingCeneKomercijalnogPoslovanja(true)
            })
            .catch((err) => handleApiError(err))
            .finally(() => {
                setIsUpdatingCeneKomercijalnogPoslovanja(false)
            })
    }

    const loadBaseData = () => {
        officeApi
            .get(`/web-azuriranje-cena`)
            .then((response: any) => {
                setData(response.data)
            })
            .catch((err) => handleApiError(err))
    }

    useEffect(() => {
        reloadData()
    }, [])

    const calculateMinWebOsnove = (data: DataDto) => {
        if (data.uslovFormiranjaWebCeneType == 1)
            return (
                (data.prodajnaCenaKomercijalno *
                    (100 + data.uslovFormiranjaWebCeneModifikator)) /
                100
            )

        return (
            (data.nabavnaCenaKomercijalno *
                (100 + data.uslovFormiranjaWebCeneModifikator)) /
            100
        )
    }

    return (
        <AzuriranjeCenaStyled container direction={`column`}>
            <AzuriranjeCenaPrimeniUsloveDialog
                isOpen={isPrimeniUsloveDialogOpen}
                handleClose={(nastaviAkciju: boolean) => {
                    if (nastaviAkciju) {
                        setIsPrimeniUsloveUToku(true)

                        const request: AzurirajCeneMaxWebOsnoveRequest = {
                            items: [],
                        }

                        data?.map((dto) => [
                            request.items.push({
                                maxWebOsnova:
                                    dto.uslovFormiranjaWebCeneType == 2
                                        ? 0
                                        : dto.prodajnaCenaKomercijalno,
                                productId: dto.id,
                            }),
                        ])

                        officeApi
                            .post(`/web-azuriraj-cene-max-web-osnove`, request)
                            .then(() => {
                                toast.success(`Uspešno ažurirane iron cene!`)
                                toast.info(`Sada ažuriram početne cene...`)

                                officeApi
                                    .post(
                                        `/web-azuriraj-cene-min-web-osnove`,
                                        {}
                                    )
                                    .then(() => {
                                        reloadData()
                                        toast.success(
                                            `Uspešno ažurirane početne cene prema definisanim uslovima!`
                                        )
                                    })
                                    .finally(() => {
                                        setIsPrimeniUsloveUToku(false)
                                    })
                            })
                            .catch((err) => handleApiError(err))
                    }

                    setIsPrimeniUsloveDialogOpen(false)
                }}
            />

            <Grid>
                <Typography variant={`h4`}>Ažuriranje Web Cena</Typography>
            </Grid>
            <Grid container>
                {data == null ? (
                    <CircularProgress />
                ) : (
                    <HorizontalActionBar>
                        <HorizontalActionBarButton
                            startIcon={
                                isPrimeniUsloveUToku ? (
                                    <CircularProgress size={`1em`} />
                                ) : null
                            }
                            disabled={
                                isPrimeniUsloveUToku ||
                                isUpdatingCeneKomercijalnogPoslovanja
                            }
                            text="Ažuriraj iron cene i primeni definisane uslove na početne cene"
                            onClick={() => {
                                setIsPrimeniUsloveDialogOpen(true)
                            }}
                        />
                    </HorizontalActionBar>
                )}
            </Grid>
            <Grid container>
                {data == null ? (
                    <CircularProgress />
                ) : (
                    <Typography variant={`body2`}>
                        Cene komercijalnog poslovanja trenutka:{' '}
                        {azuriraneKomercijalnoCeneTime == null
                            ? 'nikada'
                            : moment(
                                  asUtcString(azuriraneKomercijalnoCeneTime)
                              ).format('D.MMM.yyyy HH:mm:ss')}
                    </Typography>
                )}
            </Grid>
            <Grid sx={{ py: `1rem` }} container>
                {data == null ? (
                    <CircularProgress />
                ) : (
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="Proizvodi">
                            <TableHead>
                                <TableRow>
                                    <TableCell align="center">Naziv</TableCell>
                                    <TableCell align="center">
                                        Trenutna početna cena
                                    </TableCell>
                                    <TableCell align="center">
                                        Trenutna Iron cena
                                    </TableCell>
                                    <TableCell align="center">
                                        Nabavna Cena Komercijalno
                                    </TableCell>
                                    <TableCell align="center">
                                        Prodajna Cena Komercijalno
                                    </TableCell>
                                    <TableCell align="center">
                                        Uslov formiranja početne cene
                                    </TableCell>
                                    <TableCell align="center">
                                        Trenutna Platinum cena
                                    </TableCell>
                                    <TableCell align="center">
                                        Trenutna Gold cena
                                    </TableCell>
                                    <TableCell align="center">
                                        Trenutna Silver cena
                                    </TableCell>
                                    <TableCell align="center">
                                        Trenutna Iron cena
                                    </TableCell>
                                    <TableCell align="center">
                                        Trenutno Povezan RobaId
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {data.map((dto) => {
                                    return (
                                        <AzuriranjeCenaTableRow
                                            key={dto.id}
                                            data={dto}
                                        />
                                    )
                                })}
                            </TableBody>
                        </Table>
                    </TableContainer>
                )}
            </Grid>
        </AzuriranjeCenaStyled>
    )
}
