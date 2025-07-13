import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Button,
    Grid,
    Stack,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { ArrowDownward, Search } from '@mui/icons-material'
import { hasPermission } from '@/helpers/permissionsHelpers'
import dayjs from 'dayjs'
import { PERMISSIONS_CONSTANTS } from '@/constants'
import { MagaciniDropdown } from '../../MagaciniDropdown/ui/MagaciniDropdown'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { DATE_FORMAT, ENDPOINTS_CONSTANTS } from '../../../constants'
import { toast } from 'react-toastify'
import moment from 'moment'
import { SpecifikacijaNovcaRacunar } from './SpecifikacijaNovcaRacunar'
import { SpecifikacijaNovcaPoreska } from './SpecifikacijaNovcaPoreska'
import { SpecifikacijaNovcaHelperActions } from './SpecifkacijaNovcaHelperActions'
import { current } from '@reduxjs/toolkit'

export const SpecifikacijaNovcaTopBarActions = ({
    permissions,
    onDataChange,
    disabled,
}) => {
    const [expandedSearch, setExpandedSearch] = useState(0)
    const [fetching, setFetching] = useState(false)
    const [data, setData] = useState(undefined)
    const [date, setDate] = useState(dayjs(new Date()))
    const [initialStore, setInitialStore] = useState()
    const [store, setStore] = useState(undefined)
    const [searchByNumberInput, setSearchByNumberInput] = useState()
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

    const handleOsveziClick = (s, d) => {
        if (!s) {
            toast.error(`Molimo odaberite magacin!`)
            return
        }
        setFetching(true)
        setData(undefined)
        onDataChange(undefined)
        setSearchByNumberInput(undefined)
        console.log(d)
        officeApi
            .get(ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.GET_BY_DATE, {
                params: {
                    magacinId: s,
                    date: d.format(),
                },
            })
            .then((response) => {
                setData(response.data)
                onDataChange(response.data)
                setExpandedSearch(-1)
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    }

    const handleSearchByNumber = (number) => {
        if (!number) return toast.error(`Molimo unesite broj specifikacije!`)
        setData(undefined)
        onDataChange(undefined)
        setFetching(true)
        officeApi
            .get(ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.GET(number))
            .then((response) => {
                setData(response.data)
                onDataChange(response.data)
                setExpandedSearch(-1)
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    }
    const handleGetNextSpecification = (isFixedMagacin) => {
        if (!data) return toast.error(`Nema trenutne specifikacije!`)
        setData(undefined)
        onDataChange(undefined)
        setFetching(true)
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.NEXT(
                    data.id,
                    isFixedMagacin
                )
            )
            .then((response) => {
                setData(response.data)
                onDataChange(response.data)
            })
            .catch((err) => {
                if (err.response?.status === 404) {
                    if (isFixedMagacin) {
                        toast.error(
                            `Nema sledeće specifikacije za ovaj magacin!`
                        )
                    } else {
                        toast.error(`Nema sledeće specifikacije!`)
                    }
                    return
                }
                handleApiError(err)
            })
            .finally(() => {
                setFetching(false)
            })
    }
    const handleGetPreviousSpecification = (isFixedMagacin) => {
        if (!data) return toast.error(`Nema trenutne specifikacije!`)
        setData(undefined)
        onDataChange(undefined)
        setFetching(true)
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.PREVIOUS(
                    data.id,
                    isFixedMagacin
                )
            )
            .then((response) => {
                setData(response.data)
                onDataChange(response.data)
            })
            .catch((err) => {
                if (err.response?.status === 404) {
                    if (isFixedMagacin) {
                        toast.error(
                            `Nema prethodne specifikacije za ovaj magacin!`
                        )
                    } else {
                        toast.error(`Nema prethodne specifikacije!`)
                    }
                    return
                }
                handleApiError(err)
            })
            .finally(() => {
                setFetching(false)
            })
    }

    const handleExpand = (panel) => (event, isExpanded) => {
        setExpandedSearch(isExpanded ? panel : false)
    }

    useEffect(() => {
        setFetching(true)
        officeApi
            .get(ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.GET_DEFAULT)
            .then((response) => {
                setInitialStore(response.data.magacinId)
                setStore(response.data.magacinId)
                setData(response.data)
                onDataChange(response.data)
                setExpandedSearch(-1)
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    }, [])

    if (!initialStore) return
    return (
        <>
            <Grid item xs={12}>
                <Grid container spacing={2} alignItems={`center`}>
                    <Grid item xs={8}>
                        <Accordion
                            expanded={expandedSearch === 0}
                            onChange={handleExpand(0)}
                        >
                            <AccordionSummary expandIcon={<ArrowDownward />}>
                                Izbor specifikacije
                            </AccordionSummary>
                            <AccordionDetails>
                                <Stack gap={2}>
                                    <MagaciniDropdown
                                        defaultValue={initialStore}
                                        excluteContainingStar={true}
                                        onChange={setStore}
                                        disabled={
                                            fetching ||
                                            disabled ||
                                            !hasPermission(
                                                permissions,
                                                PERMISSIONS_CONSTANTS
                                                    .USER_PERMISSIONS
                                                    .SPECIFIKACIJA_NOVCA
                                                    .ALL_WAREHOUSES
                                            )
                                        }
                                    />
                                    <DatePicker
                                        label={`Datum`}
                                        onChange={(newDate) => {
                                            newDate && setDate(newDate)
                                            setData(undefined)
                                        }}
                                        value={date}
                                        disabled={
                                            noDatePermissions ||
                                            disabled ||
                                            fetching
                                        }
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
                                    <Button
                                        variant={`contained`}
                                        disabled={disabled || fetching}
                                        onClick={() => {
                                            handleOsveziClick(store, date)
                                        }}
                                    >
                                        Osveži
                                    </Button>
                                </Stack>
                            </AccordionDetails>
                        </Accordion>
                    </Grid>
                    <Grid item xs={4}>
                        <Accordion
                            expanded={expandedSearch === 1}
                            onChange={handleExpand(1)}
                        >
                            <AccordionSummary expandIcon={<ArrowDownward />}>
                                Izbor specifikacije po broju
                            </AccordionSummary>
                            <AccordionDetails>
                                <Grid container alignItems={`center`} gap={2}>
                                    <EnchantedTextField
                                        disabled={disabled || fetching}
                                        label={`Pretraga po broju specifikacije`}
                                        value={searchByNumberInput || 0}
                                        inputType={`number`}
                                        readOnly={searchByNumberDisabled}
                                        onChange={setSearchByNumberInput}
                                    />
                                    <Grid item>
                                        <Button
                                            variant={`outlined`}
                                            color={`secondary`}
                                            sx={{
                                                py: 2,
                                            }}
                                            disabled={
                                                searchByNumberDisabled ||
                                                disabled ||
                                                fetching
                                            }
                                            onClick={() => {
                                                handleSearchByNumber(
                                                    searchByNumberInput
                                                )
                                            }}
                                        >
                                            <Search />
                                        </Button>
                                    </Grid>
                                </Grid>
                            </AccordionDetails>
                        </Accordion>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <Stack gap={2} direction={`row`}>
                    <EnchantedTextField
                        label={`Broj trenutne specifikacije`}
                        readOnly
                        value={
                            data?.id ||
                            (fetching
                                ? `Ucitavanje...`
                                : `Neuspesno ucitavanje`)
                        }
                    />
                    <EnchantedTextField
                        label={`Magacin trenutne specifikacije`}
                        readOnly
                        value={
                            data?.magacinId ||
                            (fetching
                                ? `Ucitavanje...`
                                : `Neuspesno ucitavanje`)
                        }
                    />
                    <EnchantedTextField
                        label={`Datum trenutne specifikacije`}
                        readOnly
                        value={
                            data
                                ? moment(data.datumUTC).format(DATE_FORMAT)
                                : fetching
                                  ? `Ucitavanje...`
                                  : `Neuspesno ucitavanje`
                        }
                    />
                </Stack>
            </Grid>
            {data && (
                <SpecifikacijaNovcaHelperActions
                    disabled={disabled}
                    permissions={permissions}
                    date={dayjs(data.datumUTC)}
                    onPreviousClick={(isFixedMagacin) => {
                        handleGetPreviousSpecification(isFixedMagacin)
                    }}
                    onNextClick={(isFixedMagacin) => {
                        handleGetNextSpecification(isFixedMagacin)
                    }}
                />
            )}
        </>
    )
}
