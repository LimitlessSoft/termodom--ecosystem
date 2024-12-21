import { Box, Button, Divider, Stack } from '@mui/material'
import { MagaciniDropdown } from '@/widgets'
import React, { useState } from 'react'
import { ComboBoxInput } from '../../../ComboBoxInput/ui/ComboBoxInput'
import { IzvestajIzlazRobePoGodinamaTable } from './IzvestajIzlazRobePoGodinamaTable'
import { VrDoksDropdown } from '../../../VrDoksDropdown/ui/VrDoksDropdown'
import { handleApiError, officeApi } from '../../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../../constants'
import qs from 'qs'
import { InteractiveLoader } from '../../../InteractiveLoader/ui/InteractiveLoader'

export const IzvestajIzlazaRobePoGodinama = () => {
    const [pageLoadData, setPageLoadData] = useState({
        years: [
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
    })

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
                                    params: izvestajRequest,
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
