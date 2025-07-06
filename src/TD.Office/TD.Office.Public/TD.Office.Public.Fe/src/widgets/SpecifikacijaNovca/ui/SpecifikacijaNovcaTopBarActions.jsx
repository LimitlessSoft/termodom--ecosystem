import { Button, CircularProgress, Grid } from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import { SpecifikacijaNovcaTopBarButton } from './SpecifikacijaNovcaTopBarButton'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { Search } from '@mui/icons-material'
import { hasPermission } from '@/helpers/permissionsHelpers'
import dayjs from 'dayjs'
import { PERMISSIONS_CONSTANTS } from '@/constants'
import { MagaciniDropdown } from '../../MagaciniDropdown/ui/MagaciniDropdown'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { DATE_FORMAT, ENDPOINTS_CONSTANTS } from '../../../constants'
import { toast } from 'react-toastify'

export const SpecifikacijaNovcaTopBarActions = ({
    permissions,
    onDataChange,
}) => {
    const [data, setData] = useState(undefined)
    const [date, setDate] = useState(dayjs(new Date()))
    const [initialStore, setInitialStore] = useState()
    const [store, setStore] = useState(undefined)
    const searchByNumberDisabled = !hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.SPECIFIKACIJA_NOVCA
            .SEARCH_BY_NUMBER
    )

    const onlyPreviousWeekEnabled = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.SPECIFIKACIJA_NOVCA.PREVIOUS_WEEK
    )

    const noDatePermissions =
        !hasPermission(
            permissions,
            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.SPECIFIKACIJA_NOVCA.ALL_DATES
        ) && !onlyPreviousWeekEnabled

    const handleOsveziClick = () => {
        if (!store) {
            toast.error(`Molimo odaberite magacin!`)
            return
        }
        setData(undefined)
        officeApi
            .get(ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.GET_BY_DATE, {
                params: {
                    magacinId: store,
                    date: date.format(),
                },
            })
            .then((response) => {
                setData(response.data)
            })
            .catch(handleApiError)
    }

    useEffect(() => {
        officeApi
            .get(ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.GET_DEFAULT)
            .then((response) => {
                setInitialStore(response.data.magacinId)
                setStore(response.data.magacinId)
                setData(response.data)
            })
            .catch(handleApiError)
    }, [])

    useEffect(() => {
        onDataChange(data)
    }, [data])

    if (!initialStore) return
    return (
        <Grid item xs={12}>
            <Grid container spacing={2} alignItems={`center`}>
                <Grid item xs={4}>
                    <MagaciniDropdown
                        defaultValue={initialStore}
                        excluteContainingStar={true}
                        onChange={setStore}
                        disabled={
                            !hasPermission(
                                permissions,
                                PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                    .SPECIFIKACIJA_NOVCA.ALL_WAREHOUSES
                            )
                        }
                    />
                </Grid>
                <Grid item>
                    <DatePicker
                        label={`Datum`}
                        onChange={(newDate) => newDate && setDate(newDate)}
                        value={date}
                        disabled={noDatePermissions}
                        format={DATE_FORMAT}
                        minDate={
                            onlyPreviousWeekEnabled
                                ? dayjs().subtract(7, 'days')
                                : noDatePermissions
                                  ? dayjs()
                                  : undefined
                        }
                        maxDate={dayjs()}
                    />
                </Grid>
                <Grid item>
                    <SpecifikacijaNovcaTopBarButton
                        text={`Osvezi`}
                        onClick={handleOsveziClick}
                    />
                </Grid>
                <Grid item flexGrow={1}></Grid>
                <Grid item>
                    <EnchantedTextField
                        label={`Broj trenutne specifikacije`}
                        readOnly
                        value={data?.id || `Ucitavanje...`}
                    />
                </Grid>
                <Grid item>
                    <Grid container alignItems={`center`} gap={2}>
                        <EnchantedTextField
                            label={`Pretraga po broju specifikacije`}
                            defaultValue={0}
                            inputType={`number`}
                            readOnly={searchByNumberDisabled}
                        />
                        <Grid item>
                            <Button
                                variant={`outlined`}
                                color={`secondary`}
                                sx={{
                                    py: 2,
                                }}
                                disabled={searchByNumberDisabled}
                            >
                                <Search />
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}
