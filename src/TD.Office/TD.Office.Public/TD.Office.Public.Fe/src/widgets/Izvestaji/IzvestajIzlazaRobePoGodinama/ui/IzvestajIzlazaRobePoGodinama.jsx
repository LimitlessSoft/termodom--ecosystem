import {
    Button,
    Collapse,
    Divider,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@mui/material'
import { MagaciniDropdown } from '@/widgets'
import React, { useState } from 'react'
import { ComboBoxInput } from '../../../ComboBoxInput/ui/ComboBoxInput'
import { IzvestajIzlazaRobePoGodinamaRow } from './IzvestajIzlazaRobePoGodinamaRow'

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
        years: [2024],
    })

    const [izvestajData, setIzvestajData] = useState({
        r15: {
            naziv: 'Neki naziv stavke',
            magaciniData: {
                m112: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m113: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m116: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
            },
        },
        r16: {
            naziv: 'Neki naziv stavke',
            magaciniData: {
                m112: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m113: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m116: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
            },
        },
        r17: {
            naziv: 'Neki naziv stavke',
            magaciniData: {
                m112: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m113: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m116: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
            },
        },
        r18: {
            naziv: 'Neki naziv stavke',
            magaciniData: {
                m112: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m113: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m116: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
            },
        },
        r19: {
            naziv: 'Neki naziv stavke',
            magaciniData: {
                m112: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m113: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m116: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
            },
        },
        r20: {
            naziv: 'Neki naziv stavke',
            magaciniData: {
                m112: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m113: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
                m116: {
                    y2024: {
                        units: 20000,
                        value: 1000000,
                    },
                    y2023: {
                        units: 20000,
                        value: 1000000,
                    },
                },
            },
        },
    })

    return (
        <Stack gap={2}>
            <MagaciniDropdown />
            <ComboBoxInput
                label={`Godine`}
                onSelectionChange={() => {}}
                selectedValues={izvestajRequest.years}
                options={pageLoadData.years}
                style={{ width: 300 }}
                disabled={false}
            />
            <Button>Ucitaj</Button>

            <Divider />

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell width={50}>RobaID</TableCell>
                            <TableCell>Naziv</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <IzvestajIzlazaRobePoGodinamaRow />
                        <IzvestajIzlazaRobePoGodinamaRow />
                    </TableBody>
                </Table>
            </TableContainer>
        </Stack>
    )
}
