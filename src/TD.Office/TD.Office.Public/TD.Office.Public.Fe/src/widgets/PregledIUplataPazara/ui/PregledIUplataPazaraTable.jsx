import { DataGrid } from '@mui/x-data-grid'
import moment from 'moment/moment'
import { DATE_FORMAT } from '../../../constants'
import { Badge, Box, IconButton, Stack, Tooltip } from '@mui/material'
import { ReadMore, Warning } from '@mui/icons-material'
import { PregledIUplataPazaraDetaljiIzvoda } from './PregledIUplataPazaraDetaljiIzvoda'

export const PregledIUplataPazaraTable = ({ data }) => {
    const columns = [
        {
            field: 'infos',
            headerName: '',
            headerAlign: 'center',
            minWidth: 120,
            renderCell: (params) => (
                <Box width={`100%`}>
                    <Stack
                        gap={2}
                        direction={`row`}
                        justifyContent={`center`}
                        width={`100%`}
                    >
                        {!params.row.konto && (
                            <Tooltip
                                title={'Fali konto na izvodu'}
                                arrow
                                placement={`right`}
                            >
                                <Warning color={`warning`} />
                            </Tooltip>
                        )}
                        {!params.row.pozNaBroj && (
                            <Tooltip
                                title={'Fali poziv na broj na izvodu'}
                                arrow
                                placement={`right`}
                            >
                                <Warning color={`warning`} />
                            </Tooltip>
                        )}
                    </Stack>
                </Box>
            ),
        },
        { field: 'konto', headerName: 'Konto', width: 100 },
        { field: 'pozNaBroj', headerName: 'Poziv Na Broj', width: 100 },
        {
            field: 'magacinId',
            headerName: 'Magacin Id',
            type: 'number',
            width: 100,
        },
        {
            field: 'datum',
            headerName: 'Datum',
            width: 100,
            valueGetter: (params) => {
                return moment(params.value).format(DATE_FORMAT)
            },
        },
        {
            field: 'mpRacuni',
            headerName: 'Mp. Racuni',
            type: 'number',
            width: 100,
        },
        {
            field: 'povratnice',
            headerName: 'Povratnice',
            type: 'number',
            width: 100,
        },
        {
            field: 'zaUplatu',
            headerName: 'Za Uplatu (Mp. Racuni - Povratnice)',
            type: 'number',
            width: 300,
        },
        {
            field: 'potrazuje',
            headerName: 'Potrazuje',
            type: 'number',
            width: 100,
        },
        { field: 'razlika', headerName: 'Razlika', type: 'number', width: 100 },
        {
            field: 'actions',
            headerName: 'Akcije',
            width: 100,
            headerAlign: 'center',
            renderCell: (params) => (
                <Box width={`100%`}>
                    <Stack
                        gap={2}
                        direction={`row`}
                        justifyContent={`center`}
                        width={`100%`}
                    >
                        <PregledIUplataPazaraDetaljiIzvoda params={params} />
                    </Stack>
                </Box>
            ),
        },
    ]
    return (
        <DataGrid
            hideFooter={true}
            columns={columns}
            rows={data.items}
            getRowId={(row) =>
                `${row.pozNaBroj}_${row.konto}_${row.magacinId}_${row.datum}`
            }
        />
    )
}
