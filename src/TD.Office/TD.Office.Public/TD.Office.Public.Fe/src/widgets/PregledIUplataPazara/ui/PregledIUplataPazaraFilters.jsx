import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { MagaciniDropdown } from '../../MagaciniDropdown/ui/MagaciniDropdown'
import { DatePicker } from '@mui/x-date-pickers'
import { useMountedState } from '@/hooks'
import dayjs from 'dayjs'
import { DATE_FORMAT } from '@/constants'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { ArrowDownward } from '@mui/icons-material'
import { useEffect, useState } from 'react'

export const PregledIUplataPazaraFilters = ({
    onLoadingChanged,
    onDisabledChanged,
    onData,
}) => {
    const initialFilters = {
        magacin: [],
        odDatumaInclusive: new Date(),
        doDatumaInclusive: new Date(),
        tolerancija: 30,
    }
    const [disabled, setDisabled] = useState(false)
    const [loading, setLoading] = useState(false)
    const [expanded, setExpanded] = useState(true)
    const [filters, setFilters] = useMountedState({
        initialValue: initialFilters,
        onChange: (newFilters) => {},
    })

    useEffect(() => {
        onDisabledChanged?.(disabled)
    }, [disabled, onDisabledChanged])

    useEffect(() => {
        onLoadingChanged?.(loading)
    }, [loading, onLoadingChanged])
    return (
        <Accordion
            expanded={expanded}
            onChange={() => setExpanded((prev) => !prev)}
        >
            <AccordionSummary expandIcon={<ArrowDownward />}>
                <Typography>Filteri</Typography>
            </AccordionSummary>
            <AccordionDetails>
                <Stack gap={2}>
                    <DatePicker
                        disabled={disabled || loading}
                        label={`Od datuma`}
                        format={DATE_FORMAT}
                        value={dayjs(filters.odDatumaInclusive)}
                        onChange={(e) => {
                            setFilters((prev) => {
                                return {
                                    ...prev,
                                    odDatumaInclusive: dayjs(e ?? new Date())
                                        .startOf('day')
                                        .toDate(),
                                }
                            })
                        }}
                    />
                    <DatePicker
                        disabled={disabled || loading}
                        label={`Do datuma`}
                        format={DATE_FORMAT}
                        value={dayjs(filters.doDatumaInclusive)}
                        onChange={(e) => {
                            setFilters((prev) => {
                                return {
                                    ...prev,
                                    doDatumaInclusive: dayjs(e ?? new Date())
                                        .startOf('day')
                                        .toDate(),
                                }
                            })
                        }}
                    />
                    <TextField
                        disabled={disabled || loading}
                        label={`Tolerancija`}
                        value={filters.tolerancija}
                        onChange={(e) => {
                            if (e.target.value.length === 0) {
                                setFilters((prev) => {
                                    return {
                                        ...prev,
                                        tolerancija: 0,
                                    }
                                })
                                return
                            }
                            const parsed = parseInt(e.target.value)
                            const initi = isNaN(parsed)
                            console.log(initi)
                            if (initi) return
                            setFilters((prev) => {
                                return {
                                    ...prev,
                                    tolerancija: +parsed,
                                }
                            })
                        }}
                    />
                    <MagaciniDropdown
                        disabled={disabled || loading}
                        types={[2]}
                        excluteContainingStar={true}
                        multiselect
                        onChange={(e) => {
                            setFilters((prev) => {
                                return {
                                    ...prev,
                                    magacin: e,
                                }
                            })
                        }}
                        selectedValues={filters.magacin}
                    />
                    <Box>
                        <Button
                            variant={`contained`}
                            disabled={disabled || loading}
                            onClick={() => {
                                onData(undefined)
                                setLoading(true)
                                officeApi
                                    .get(
                                        ENDPOINTS_CONSTANTS
                                            .PREGLED_I_UPLATA_PAZARA
                                            .GET_MULTIPLE,
                                        {
                                            params: {
                                                ...filters,
                                            },
                                        }
                                    )
                                    .then((response) => {
                                        onData(response.data)
                                        setExpanded(false)
                                    })
                                    .catch((err) => handleApiError(err))
                                    .finally(() => {
                                        setLoading(false)
                                    })
                            }}
                        >
                            Prikazi
                        </Button>
                    </Box>
                </Stack>
            </AccordionDetails>
        </Accordion>
    )
}
