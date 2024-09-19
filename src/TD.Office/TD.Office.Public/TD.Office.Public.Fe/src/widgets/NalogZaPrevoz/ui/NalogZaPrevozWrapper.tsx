import {
    Autocomplete,
    Button,
    CircularProgress,
    Grid,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TextField,
    Typography,
} from '@mui/material'
import { NalogZaPrevozNoviDialog } from './NalogZaPrevozNoviDialog'
import { PERMISSIONS_GROUPS, USER_PERMISSIONS } from '@/constants'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { NalogZaPrevozTable } from './NalogZaPrevozTable'
import { Add, Print } from '@mui/icons-material'
import { DatePicker } from '@mui/x-date-pickers'
import { useUser } from '@/hooks/useUserHook'
import { useEffect, useState } from 'react'
import dayjs from 'dayjs'
import { handleApiError, officeApi } from '@/apis/officeApi'

export const NalogZaPrevozWrapper = () => {
    const [reload, setReload] = useState<boolean>(false)
    const [isLoadingData, setIsLoadingData] = useState<boolean>(false)
    const [data, setData] = useState<any[] | undefined>(undefined)
    const [selectedFromDate, setSelectedFromDate] = useState<Date>(new Date())
    const [selectedToDate, setSelectedToDate] = useState<Date>(new Date())
    const [selectedStore, setSelectedStore] = useState<any | null>(null)
    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const user = useUser(false)

    const [newDialogOpened, setNewDialogOpened] = useState<boolean>(false)

    const permissions = usePermissions(PERMISSIONS_GROUPS.NALOG_ZA_PREVOZ)

    console.log(data)

    const sumOrderProperty = (
        property: string,
        condition?: (order: any) => boolean
    ) => {
        if (!data || data.length === 0) return 0

        const filteredData = condition ? data.filter(condition) : data

        return filteredData
            .reduce((prev, current) => prev + (current[property] || 0), 0)
            .toFixed(2)
    }

    useEffect(() => {
        officeApi
            .get(`/stores`)
            .then((response: any) => {
                setStores(response.data)
                setSelectedStore(
                    response.data.find(
                        (store: any) => store.id === user.data?.storeId
                    )
                )
            })
            .catch((err) => handleApiError(err))
    }, [])

    useEffect(() => {
        if (selectedStore === null) return

        setIsLoadingData(true)

        officeApi
            .get(
                `/nalog-za-prevoz?storeId=${selectedStore.id}&dateFrom=${dayjs(selectedFromDate).format('YYYY-MM-DD')}&dateTo=${dayjs(selectedToDate).format('YYYY-MM-DD')}`
            )
            .then((response: any) => {
                setData(response.data)
            })
            .catch((err) => handleApiError(err))
            .finally(() => {
                setIsLoadingData(false)
            })
    }, [selectedStore, selectedFromDate, selectedToDate, reload])

    return (
        <Grid container spacing={2} p={2} className={`with-print-header`}>
            <NalogZaPrevozNoviDialog
                open={newDialogOpened}
                store={selectedStore}
                onClose={() => {
                    setNewDialogOpened(false)
                    setReload(!reload)
                }}
            />
            <Grid item xs={12}>
                <Grid container spacing={2} alignItems={`center`}>
                    <Grid item className={`print-header`}>
                        <Typography variant={`h4`}>Nalog za prevoz</Typography>
                    </Grid>
                    <Grid item className={`no-print`}>
                        <Button
                            variant={`contained`}
                            startIcon={<Add />}
                            disabled={
                                selectedStore === null ||
                                isLoadingData ||
                                !hasPermission(
                                    permissions,
                                    USER_PERMISSIONS.NALOG_ZA_PREVOZ.NEW
                                )
                            }
                            onClick={() => {
                                setNewDialogOpened(true)
                            }}
                        >
                            Novi
                        </Button>
                    </Grid>
                    <Grid item className={`no-print`}>
                        <Button
                            variant={`outlined`}
                            startIcon={<Print />}
                            disabled={
                                selectedStore === null ||
                                isLoadingData ||
                                !hasPermission(
                                    permissions,
                                    USER_PERMISSIONS.NALOG_ZA_PREVOZ
                                        .REPORT_PRINT
                                )
                            }
                            onClick={() => {
                                window.print()
                            }}
                        >
                            Print
                        </Button>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <Grid container spacing={1}>
                    <Grid item xs={12} sm={6}>
                        {stores === undefined && <CircularProgress />}
                        {stores !== undefined && stores.length === 0 && (
                            <h2>Nema dostupnih prodavnica</h2>
                        )}
                        {stores !== undefined && stores.length > 0 && (
                            <Autocomplete
                                defaultValue={stores!.find(
                                    (store) => store.id === user.data?.storeId
                                )}
                                options={stores!}
                                onChange={(event, value) => {
                                    setSelectedStore(value)
                                }}
                                disabled={
                                    !hasPermission(
                                        permissions,
                                        USER_PERMISSIONS.NALOG_ZA_PREVOZ
                                            .ALL_WAREHOUSES
                                    )
                                }
                                getOptionLabel={(option) => {
                                    return `[ ${option.id} ] ${option.name}`
                                }}
                                renderInput={(params) => (
                                    <TextField {...params} label={`magacin`} />
                                )}
                            />
                        )}
                    </Grid>
                    <Grid item>
                        <DatePicker
                            disabled={
                                selectedStore === null ||
                                isLoadingData ||
                                !hasPermission(
                                    permissions,
                                    USER_PERMISSIONS.NALOG_ZA_PREVOZ
                                        .PREVIOUS_DATES
                                )
                            }
                            label="Od datuma"
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e: any) => {
                                setSelectedFromDate(e)
                            }}
                        />
                    </Grid>
                    <Grid item>
                        <DatePicker
                            disabled={
                                selectedStore === null ||
                                isLoadingData ||
                                !hasPermission(
                                    permissions,
                                    USER_PERMISSIONS.NALOG_ZA_PREVOZ
                                        .PREVIOUS_DATES
                                )
                            }
                            label="Do datuma"
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e: any) => {
                                setSelectedToDate(e)
                            }}
                        />
                    </Grid>
                    <Grid container>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>Broj dokumenata</TableCell>
                                    <TableCell>
                                        Ukupna cena prevoznika bez pdv
                                    </TableCell>
                                    <TableCell>
                                        Ukupno mi naplatili gotovinom
                                    </TableCell>
                                    <TableCell>
                                        Ukupno mi naplatili virmanom
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                <TableRow>
                                    <TableCell>{data?.length}</TableCell>
                                    <TableCell>
                                        {sumOrderProperty('cenaPrevozaBezPdv')}
                                    </TableCell>
                                    <TableCell>
                                        {sumOrderProperty(
                                            'miNaplatiliKupcuBezPdv',
                                            (order) => !order.placenVirmanom
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {sumOrderProperty(
                                            'miNaplatiliKupcuBezPdv',
                                            (order) => order.placenVirmanom
                                        )}
                                    </TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </Grid>
                </Grid>
            </Grid>
            <NalogZaPrevozTable data={data} permissions={permissions} />
        </Grid>
    )
}
