import {
    Box,
    Button,
    Divider,
    MenuItem,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { MagaciniDropdown } from '@/widgets'
import React, { useState } from 'react'
import { ComboBoxInput } from '../../../ComboBoxInput/ui/ComboBoxInput'
import { IzvestajIzlazRobePoGodinamaTable } from './IzvestajIzlazRobePoGodinamaTable'
import { VrDoksDropdown } from '../../../VrDoksDropdown/ui/VrDoksDropdown'
import { handleApiError, officeApi } from '../../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../../constants'
import qs from 'qs'
import { InteractiveLoader } from '../../../InteractiveLoader/ui/InteractiveLoader'
import { DatePicker } from '@mui/x-date-pickers'

export const IzvestajIzlazaRobePoGodinama = () => {
    const [pageLoadData, setPageLoadData] = useState({
        years: [
            {
                key: 2025,
                value: 'TCMDZ 2025',
            },
            {
                key: 2024,
                value: 'TCMDZ 2024',
            },
            {
                key: 2023,
                value: 'TCMDZ 2023',
            },
        ],
    })

    const [izvestajRequest, setIzvestajRequest] = useState({
        godina: [],
        magacin: [],
        vrDok: [],
        odDatuma: new Date(),
        doDatuma: new Date(),
    })

    const meseci = [
        'Januar',
        'Februar',
        'Mart',
        'April',
        'Maj',
        'Jun',
        'Jul',
        'Avgust',
        'Septembar',
        'Oktobar',
        'Novembar',
        'Decembar',
    ]

    const [izvestajData, setIzvestajData] = useState(undefined)

    const [isLoading, setIsLoading] = useState(false)

    return (
        <Stack gap={2}>
            <MagaciniDropdown
                disabled={isLoading}
                onChange={(arr) => {
                    setIzvestajRequest((prev) => {
                        return {
                            ...prev,
                            magacin: arr,
                        }
                    })
                }}
                multiselect
            />
            <ComboBoxInput
                label={`Godine`}
                onSelectionChange={(e) => {
                    setIzvestajRequest((prev) => {
                        return {
                            ...prev,
                            godina: e.target.value,
                        }
                    })
                }}
                selectedValues={izvestajRequest.godina}
                options={pageLoadData.years}
                style={{ width: 300 }}
                disabled={isLoading}
            />
            <VrDoksDropdown
                disabled={isLoading}
                multiselect={true}
                onChange={(e) => {
                    setIzvestajRequest((prev) => {
                        return {
                            ...prev,
                            vrDok: e,
                        }
                    })
                }}
            />
            <Stack gap={2}>
                <Typography>Za period:</Typography>
                <Stack direction={`row`} gap={2} alignItems={`center`}>
                    <Typography>Od:</Typography>
                    <TextField
                        label={`Mesec`}
                        select
                        value={izvestajRequest.odDatuma.getMonth()}
                        onChange={(e) => {
                            setIzvestajRequest((prev) => {
                                return {
                                    ...prev,
                                    odDatuma: new Date(
                                        prev.odDatuma.getFullYear(),
                                        e.target.value,
                                        prev.odDatuma.getDate()
                                    ),
                                }
                            })
                        }}
                    >
                        {Array.from({ length: 12 }).map((_, i) => (
                            <MenuItem key={i} value={i}>
                                {meseci[i]}
                            </MenuItem>
                        ))}
                    </TextField>
                    <TextField
                        label={`Dan`}
                        select
                        value={izvestajRequest.odDatuma.getDate()}
                        onChange={(e) => {
                            setIzvestajRequest((prev) => {
                                return {
                                    ...prev,
                                    odDatuma: new Date(
                                        prev.odDatuma.getFullYear(),
                                        prev.odDatuma.getMonth(),
                                        e.target.value
                                    ),
                                }
                            })
                        }}
                    >
                        {Array.from({ length: 31 }).map((_, i) => (
                            <MenuItem key={i} value={i + 1}>
                                {i + 1}
                            </MenuItem>
                        ))}
                    </TextField>
                </Stack>
                <Stack direction={`row`} gap={2} alignItems={`center`}>
                    <Typography>Do:</Typography>
                    <TextField
                        label={`Mesec`}
                        select
                        value={izvestajRequest.doDatuma.getMonth()}
                        onChange={(e) => {
                            setIzvestajRequest((prev) => {
                                return {
                                    ...prev,
                                    doDatuma: new Date(
                                        prev.doDatuma.getFullYear(),
                                        e.target.value,
                                        prev.doDatuma.getDate()
                                    ),
                                }
                            })
                        }}
                    >
                        {Array.from({ length: 12 }).map((_, i) => (
                            <MenuItem key={i} value={i}>
                                {meseci[i]}
                            </MenuItem>
                        ))}
                    </TextField>
                    <TextField
                        label={`Dan`}
                        select
                        value={izvestajRequest.doDatuma.getDate()}
                        onChange={(e) => {
                            setIzvestajRequest((prev) => {
                                return {
                                    ...prev,
                                    doDatuma: new Date(
                                        prev.odDatuma.getFullYear(),
                                        prev.odDatuma.getMonth(),
                                        e.target.value
                                    ),
                                }
                            })
                        }}
                    >
                        {Array.from({ length: 31 }).map((_, i) => (
                            <MenuItem key={i} value={i + 1}>
                                {i + 1}
                            </MenuItem>
                        ))}
                    </TextField>
                </Stack>
            </Stack>
            <Box>
                <Button
                    disabled={isLoading}
                    variant={`contained`}
                    onClick={() => {
                        setIzvestajData(undefined)
                        setIsLoading(true)
                        officeApi
                            .get(
                                ENDPOINTS_CONSTANTS.IZVESTAJI
                                    .GET_IZVESTAJ_IZLAZA_ROBE_PO_GODINAMA,
                                {
                                    params: {
                                        ...izvestajRequest,
                                        odDatuma:
                                            izvestajRequest.odDatuma.toISOString(),
                                        doDatuma:
                                            izvestajRequest.doDatuma.toISOString(),
                                    },
                                    paramsSerializer: (params) =>
                                        qs.stringify(params, {
                                            arrayFormat: 'repeat',
                                        }),
                                }
                            )
                            .then((res) => {
                                setIzvestajData(res.data)
                            })
                            .catch(handleApiError)
                            .finally(() => {
                                setIsLoading(false)
                            })
                    }}
                >
                    Ucitaj
                </Button>
            </Box>

            <Divider />

            {isLoading && (
                <InteractiveLoader
                    messages={['Genersianje izveštaja je u toku']}
                />
            )}
            {izvestajData && (
                <IzvestajIzlazRobePoGodinamaTable data={izvestajData} />
            )}
        </Stack>
    )
}
