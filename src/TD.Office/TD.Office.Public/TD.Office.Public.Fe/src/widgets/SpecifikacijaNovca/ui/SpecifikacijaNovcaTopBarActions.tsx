import { Autocomplete, Button, Grid, TextField } from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import { SpecifikacijaNovcaTopBarButton } from './SpecifikacijaNovcaTopBarButton'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { ISpecifikacijaNovcaTopBarActionsProps } from '../interfaces/ISpecifikacijaNovcaTopBarActionsProps'
import { useState } from 'react'
import { IStoreDto } from '@/dtos/stores/IStoreDto'
import { Search } from '@mui/icons-material'
import { hasPermission } from '@/helpers/permissionsHelpers'
import dayjs from 'dayjs'
import { PERMISSIONS_CONSTANTS } from '@/constants'

export const SpecifikacijaNovcaTopBarActions = ({
    permissions,
    stores,
    currentStore,
    date,
    currentSpecificationNumber,
    onChangeDate,
    onChangeStore,
}: ISpecifikacijaNovcaTopBarActionsProps) => {
    if (stores.length === 0) throw new Error(`Neuspesno ucitavanje magacina`)

    const [options, setOptions] = useState<IStoreDto[]>(
        stores.toSorted((a, b) => b.id - a.id)
    )

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

    return (
        <Grid item xs={12}>
            <Grid container spacing={2} alignItems={`center`}>
                <Grid item xs={4}>
                    <Autocomplete
                        disableClearable
                        value={currentStore}
                        options={options}
                        onChange={(event, store) => onChangeStore(store)}
                        getOptionLabel={(option) =>
                            `[ ${option.id} ] ${option.name}`
                        }
                        renderInput={(params) => (
                            <TextField
                                variant={`outlined`}
                                {...params}
                                label={`Magacini`}
                            />
                        )}
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
                        onChange={(newDate) => newDate && onChangeDate(newDate)}
                        value={date}
                        disabled={noDatePermissions}
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
                    <SpecifikacijaNovcaTopBarButton text={`Osvezi`} />
                </Grid>
                <Grid item flexGrow={1}></Grid>
                <Grid item>
                    <EnchantedTextField
                        label={`Broj specifikacije`}
                        readOnly
                        value={currentSpecificationNumber}
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
