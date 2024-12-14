import { Box, Button, Divider, Stack } from '@mui/material'
import { MagaciniDropdown } from '@/widgets'
import React, { useState } from 'react'
import { ComboBoxInput } from '../../../ComboBoxInput/ui/ComboBoxInput'
import { IzvestajIzlazRobePoGodinamaTable } from './IzvestajIzlazRobePoGodinamaTable'

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
        years: [],
        magacini: [],
    })

    const [izvestajData, setIzvestajData] = useState({
        centar16: {
            godina2024: 100000,
            godina2023: 100000,
            magacin116: {
                naziv: 'Magacin 116',
                godina2024: {
                    vrednost: 100000,
                    dokumenti: {
                        v15: {
                            naziv: `Maloprodajni Racun`,
                            vrednost: 10000,
                        },
                        v32: {
                            naziv: `Proracun`,
                            vrednost: 20000,
                        },
                    },
                },
                godina2023: {
                    vrednost: 100000,
                    dokumenti: {
                        v15: {
                            naziv: `Maloprodajni Racun`,
                            vrednost: 10000,
                        },
                        v32: {
                            naziv: `Proracun`,
                            vrednost: 20000,
                        },
                    },
                },
            },
            magacin2116: {
                naziv: 'Magacin 2116',
                godina2024: {
                    vrednost: 100000,
                    dokumenti: {
                        v15: {
                            naziv: `Maloprodajni Racun`,
                            vrednost: 10000,
                        },
                        v32: {
                            naziv: `Proracun`,
                            vrednost: 20000,
                        },
                    },
                },
                godina2023: {
                    vrednost: 100000,
                    dokumenti: {
                        v15: {
                            naziv: `Maloprodajni Racun`,
                            vrednost: 10000,
                        },
                        v32: {
                            naziv: `Proracun`,
                            vrednost: 20000,
                        },
                    },
                },
            },
        },
        centar28: {
            godina2024: 100000,
            godina2023: 100000,
            magacin128: {
                naziv: 'Magacin 128',
                godina2024: {
                    vrednost: 100000,
                    dokumenti: {
                        v15: {
                            naziv: `Maloprodajni Racun`,
                            vrednost: 10000,
                        },
                        v32: {
                            naziv: `Proracun`,
                            vrednost: 20000,
                        },
                    },
                },
                godina2023: {
                    vrednost: 100000,
                    dokumenti: {
                        v15: {
                            naziv: `Maloprodajni Racun`,
                            vrednost: 10000,
                        },
                        v32: {
                            naziv: `Proracun`,
                            vrednost: 20000,
                        },
                    },
                },
            },
            magacin2128: {
                naziv: 'Magacin 2128',
                godina2024: {
                    vrednost: 100000,
                    dokumenti: {
                        v15: {
                            naziv: `Maloprodajni Racun`,
                            vrednost: 10000,
                        },
                        v32: {
                            naziv: `Proracun`,
                            vrednost: 20000,
                        },
                    },
                },
                godina2023: {
                    vrednost: 100000,
                    dokumenti: {
                        v15: {
                            naziv: `Maloprodajni Racun`,
                            vrednost: 10000,
                        },
                        v32: {
                            naziv: `Proracun`,
                            vrednost: 20000,
                        },
                    },
                },
            },
        },
    })

    return (
        <Stack gap={2}>
            <MagaciniDropdown
                onChange={(arr) => {
                    setIzvestajRequest((prev) => {
                        return {
                            ...prev,
                            magacini: arr,
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
                            years: e.target.value,
                        }
                    })
                }}
                selectedValues={izvestajRequest.years}
                options={pageLoadData.years}
                style={{ width: 300 }}
                disabled={false}
            />
            <Box>
                <Button variant={`contained`}>Ucitaj</Button>
            </Box>

            <Divider />

            {izvestajData && (
                <IzvestajIzlazRobePoGodinamaTable data={izvestajData} />
            )}
        </Stack>
    )
}
