import {
    Autocomplete,
    Button,
    CircularProgress,
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Typography,
} from '@mui/material'
import { NalogZaPrevozNoviDialog } from './NalogZaPrevozNoviDialog'
import { PERMISSIONS_CONSTANTS, PRINT_CONSTANTS } from '@/constants'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { NalogZaPrevozTable } from './NalogZaPrevozTable'
import { Add, Print } from '@mui/icons-material'
import { DatePicker } from '@mui/x-date-pickers'
import { useUser } from '@/hooks/useUserHook'
import { useEffect, useState } from 'react'
import dayjs from 'dayjs'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ORDER_DTO_FIELDS } from '@/dtoFields/orderDtoFields'
import { formatNumber } from '@/helpers/numberHelpers'
import { useZMagacini } from '@/zStore'

export const NalogZaPrevozWrapper = () => {
    const [reload, setReload] = useState(false)
    const [isLoadingData, setIsLoadingData] = useState(false)
    const [data, setData] = useState(undefined)
    const [selectedFromDate, setSelectedFromDate] = useState(new Date())
    const [selectedToDate, setSelectedToDate] = useState(new Date())
    const [selectedStore, setSelectedStore] = useState(null)
    const [stores, setStores] = useState(undefined)
    const user = useUser(false)

    const [newDialogOpened, setNewDialogOpened] = useState(false)

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.NALOG_ZA_PREVOZ
    )

    const sumOrderProperty = (property, condition) => {
        if (!data || data.length === 0) return 0

        const filteredData = condition ? data.filter(condition) : data

        return formatNumber(
            filteredData.reduce(
                (prev, current) => prev + (current[property] || 0),
                0
            )
        )
    }

    const magacini = useZMagacini()

    useEffect(() => {
        if (magacini === undefined) return

        setStores(magacini)
        setSelectedStore(
            magacini.find((store) => store.id === user.data?.storeId)
        )
    }, [magacini])

    useEffect(() => {
        if (selectedStore === null) return

        setIsLoadingData(true)

        officeApi
            .get(
                `/nalog-za-prevoz?storeId=${selectedStore.id}&dateFrom=${dayjs(selectedFromDate).format('YYYY-MM-DD')}&dateTo=${dayjs(selectedToDate).format('YYYY-MM-DD')}`
            )
            .then((response) => {
                setData(response.data)
            })
            .catch((err) => handleApiError(err))
            .finally(() => {
                setIsLoadingData(false)
            })
    }, [selectedStore, selectedFromDate, selectedToDate, reload])

    return (
        <Grid container spacing={2} p={2}>
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
                    <Grid item>
                        <Typography variant={`h4`}>Nalog za prevoz</Typography>
                    </Grid>
                    <Grid
                        item
                        className={PRINT_CONSTANTS.PRINT_CLASSNAMES.NO_PRINT}
                    >
                        <Button
                            variant={`contained`}
                            startIcon={<Add />}
                            disabled={
                                selectedStore === null ||
                                isLoadingData ||
                                !hasPermission(
                                    permissions,
                                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                        .NALOG_ZA_PREVOZ.NEW
                                )
                            }
                            onClick={() => {
                                setNewDialogOpened(true)
                            }}
                        >
                            Novi
                        </Button>
                    </Grid>
                    <Grid
                        item
                        className={PRINT_CONSTANTS.PRINT_CLASSNAMES.NO_PRINT}
                    >
                        <Button
                            variant={`outlined`}
                            startIcon={<Print />}
                            disabled={
                                selectedStore === null ||
                                isLoadingData ||
                                !hasPermission(
                                    permissions,
                                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                        .NALOG_ZA_PREVOZ.REPORT_PRINT
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
                                        PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                            .NALOG_ZA_PREVOZ.ALL_WAREHOUSES
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
                                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                        .NALOG_ZA_PREVOZ.PREVIOUS_DATES
                                )
                            }
                            label="Od datuma"
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e) => {
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
                                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                        .NALOG_ZA_PREVOZ.PREVIOUS_DATES
                                )
                            }
                            label="Do datuma"
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e) => {
                                setSelectedToDate(e)
                            }}
                        />
                    </Grid>
                    <Grid container m={1}>
                        <TableContainer component={Paper}>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell sx={{ textAlign: 'center' }}>
                                            Broj dokumenata
                                        </TableCell>
                                        <TableCell sx={{ textAlign: 'center' }}>
                                            Ukupna cena prevoznika bez pdv
                                        </TableCell>
                                        <TableCell sx={{ textAlign: 'center' }}>
                                            Ukupno mi naplatili gotovinom
                                        </TableCell>
                                        <TableCell sx={{ textAlign: 'center' }}>
                                            Ukupno mi naplatili virmanom
                                        </TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TableRow>
                                        <TableCell sx={{ textAlign: 'center' }}>
                                            {data?.length}
                                        </TableCell>
                                        <TableCell sx={{ textAlign: 'center' }}>
                                            {sumOrderProperty(
                                                ORDER_DTO_FIELDS.CENA_PREVOZA_BEZ_PDV,
                                                (order) => order.status === 0
                                            )}
                                        </TableCell>
                                        <TableCell sx={{ textAlign: 'center' }}>
                                            {sumOrderProperty(
                                                ORDER_DTO_FIELDS.MI_NAPLATILI_KUPCU_BEZ_PDV,
                                                (order) => order.status === 0 && !order.placenVirmanom
                                            )}
                                        </TableCell>
                                        <TableCell sx={{ textAlign: 'center' }}>
                                            {sumOrderProperty(
                                                ORDER_DTO_FIELDS.MI_NAPLATILI_KUPCU_BEZ_PDV,
                                                (order) => order.status === 0 && order.placenVirmanom
                                            )}
                                        </TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </Grid>
                </Grid>
            </Grid>
            <NalogZaPrevozTable
                data={data}
                permissions={permissions}
                onReload={() => setReload(!reload)}
            />
        </Grid>
    )
}
