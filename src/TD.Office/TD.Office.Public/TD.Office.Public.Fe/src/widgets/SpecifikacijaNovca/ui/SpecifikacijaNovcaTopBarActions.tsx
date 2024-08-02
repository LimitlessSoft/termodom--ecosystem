import {
    Accordion,
    AccordionDetails,
    Autocomplete,
    Button,
    Grid,
    TextField,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import { SpecifikacijaNovcaTopBarButton } from './SpecifikacijaNovcaTopBarButton'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { ISpecifikacijaNovcaTopBarActionsProps } from '../interfaces/ISpecifikacijaNovcaTopBarActionsProps'
import { useState } from 'react'
import { IStoreDto } from '@/dtos/stores/IStoreDto'
import { Search } from '@mui/icons-material'

export const SpecifikacijaNovcaTopBarActions = ({
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
                    />
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
                        />
                        <Grid item>
                            <Button
                                variant={`outlined`}
                                color={`secondary`}
                                sx={{
                                    py: 2,
                                }}
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
