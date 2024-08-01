import { Autocomplete, Grid, TextField } from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import { SpecifikacijaNovcaTopBarButton } from './SpecifikacijaNovcaTopBarButton'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { ISpecifikacijaNovcaTopBarActionsProps } from '../interfaces/ISpecifikacijaNovcaTopBarActionsProps'

export const SpecifikacijaNovcaTopBarActions = ({
    stores,
    currentStore,
    date,
    currentSpecificationNumber,
    onChangeDate,
    onChangeStore,
}: ISpecifikacijaNovcaTopBarActionsProps) => {
    return (
        <Grid item xs={12}>
            <Grid container spacing={2} alignItems={`center`}>
                <Grid item xs={4}>
                    {stores && stores.length > 0 && (
                        <Autocomplete
                            disableClearable
                            value={currentStore}
                            options={stores}
                            onChange={(event, store) =>
                                onChangeStore(store ?? undefined)
                            }
                            getOptionLabel={(option) => {
                                return `[ ${option.id} ] ${option.name}`
                            }}
                            renderInput={(params) => (
                                <TextField
                                    variant={`outlined`}
                                    {...params}
                                    label="Magacini"
                                />
                            )}
                        />
                    )}
                </Grid>
                <Grid item>
                    <DatePicker
                        label={`Datum`}
                        onChange={(newDate) => newDate && onChangeDate(newDate)}
                        value={date}
                    />
                </Grid>
                <Grid item>
                    <SpecifikacijaNovcaTopBarButton text={`Osvezi`} />
                </Grid>
                <Grid item flexGrow={1}></Grid>
                <Grid item>
                    <EnchantedTextField
                        label={`Pretraga po broju specifikacije`}
                        defaultValue={0}
                        inputType={`number`}
                    />
                </Grid>
                <Grid item>
                    <EnchantedTextField
                        label={`Broj specifikacije`}
                        readOnly
                        value={currentSpecificationNumber}
                    />
                </Grid>
            </Grid>
        </Grid>
    )
}
