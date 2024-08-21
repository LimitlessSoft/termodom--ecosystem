import { mainTheme } from '@/theme'
import { StripedDataGrid } from '@/widgets/StripedDataGrid'
import { AddCircle, Cancel, Delete, Save } from '@mui/icons-material'
import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    LinearProgress,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import {
    GridActionsCellItem,
    GridExpandMoreIcon,
    GridValidRowModel,
} from '@mui/x-data-grid'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'

export const JM = (): JSX.Element => {
    const [units, setUnits] = useState<any | undefined>(null)
    const [rowsInEditMode, setRowsInEditMode] = useState<any[]>([])
    const [noviUnit, setNoviUnit] = useState<any>({
        name: null,
    })
    const textFieldVariant = 'standard'

    const loadData = () => {
        adminApi
            .get(`/units`)
            .then((response) => {
                setUnits(response.data)
            })
            .catch((err) => handleApiError(err))
    }

    useEffect(() => {
        loadData()
    }, [])

    return (
        <Box sx={{ width: '100%' }}>
            <Box>
                <Typography sx={{ m: 2 }} variant="h6">
                    Jedinice mere
                </Typography>
            </Box>
            <Box>
                {units == null ? (
                    <LinearProgress />
                ) : (
                    <StripedDataGrid
                        autoHeight
                        editMode="cell"
                        sx={{ m: 2 }}
                        rows={units}
                        columns={[
                            { field: 'id', headerName: 'Id' },
                            {
                                field: 'name',
                                headerName: 'Naziv',
                                flex: 1,
                                editable: true,
                            },
                            {
                                field: 'actions',
                                headerName: 'Akcije',
                                type: 'actions',
                                getActions: (params) => {
                                    const isInEditMode =
                                        rowsInEditMode.find(
                                            (x) => x == params.id
                                        ) != null

                                    if (isInEditMode) {
                                        return [
                                            <GridActionsCellItem
                                                key={`save-icon-safg-${params.id}`}
                                                icon={<Save />}
                                                label="Sačuvaj"
                                                onClick={() => {
                                                    adminApi
                                                        .put(`/units`, {
                                                            id: params.row.id,
                                                            name: params.row
                                                                .name,
                                                        })
                                                        .then(() => {
                                                            toast(
                                                                'JM uspešno ažurirana!',
                                                                {
                                                                    type: 'success',
                                                                }
                                                            )
                                                            setRowsInEditMode(
                                                                (old) => [
                                                                    ...old.filter(
                                                                        (x) =>
                                                                            x !==
                                                                            params.id
                                                                    ),
                                                                ]
                                                            )
                                                        })
                                                        .catch((err) =>
                                                            handleApiError(err)
                                                        )
                                                }}
                                            />,
                                            <GridActionsCellItem
                                                key={`cancel-icon-safg-${params.id}`}
                                                icon={<Cancel />}
                                                label="Odbaci"
                                                onClick={() => {
                                                    setRowsInEditMode((old) => [
                                                        ...old.filter(
                                                            (x) =>
                                                                x !== params.id
                                                        ),
                                                    ])
                                                    loadData()
                                                }}
                                            />,
                                        ]
                                    }
                                    return [
                                        <GridActionsCellItem
                                            key={`delete-icon-vas${params.id}`}
                                            icon={<Delete />}
                                            label="Obriši"
                                            onClick={() => {
                                                adminApi
                                                    .delete(
                                                        `/units/${params.row.id}`
                                                    )
                                                    .then(() => {
                                                        toast.success(
                                                            'JM uspešno obrisana!'
                                                        )
                                                        setUnits(
                                                            (prev: any) => [
                                                                ...prev.filter(
                                                                    (x: any) =>
                                                                        x.id !==
                                                                        params
                                                                            .row
                                                                            .id
                                                                ),
                                                            ]
                                                        )
                                                    })
                                                    .catch((err) =>
                                                        handleApiError(err)
                                                    )
                                            }}
                                        />,
                                    ]
                                },
                            },
                        ]}
                        initialState={{
                            sorting: {
                                sortModel: [{ field: 'id', sort: 'asc' }],
                            },
                            pagination: {
                                paginationModel: {
                                    page: 0,
                                    pageSize:
                                        mainTheme.defaultPagination.default,
                                },
                            },
                        }}
                        pageSizeOptions={mainTheme.defaultPagination.options}
                        onCellEditStart={(row) => {
                            setRowsInEditMode((old) => [...old, row.id])
                        }}
                        onProcessRowUpdateError={() => {}}
                        processRowUpdate={(newRow) => {
                            const updatedRow: GridValidRowModel = {
                                ...newRow,
                                isNew: false,
                            }
                            setUnits((old: any) => [
                                ...old.filter(
                                    (x: any) => x.id !== updatedRow.id
                                ),
                                {
                                    id: updatedRow.id,
                                    name: updatedRow.name,
                                },
                            ])
                            //handle send data to api
                            return updatedRow
                        }}
                        getRowClassName={(params) =>
                            params.indexRelativeToCurrentPage % 2 === 0
                                ? 'even'
                                : 'odd'
                        }
                    />
                )}
            </Box>
            <Box sx={{ m: 2 }}>
                <Accordion>
                    <AccordionSummary
                        expandIcon={<GridExpandMoreIcon />}
                        aria-controls="panel1a-content"
                        id="panel1a-header"
                    >
                        <Typography>Kreiraj novu JM</Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Stack
                            direction={'row'}
                            spacing={2}
                            alignItems={'center'}
                        >
                            <TextField
                                required
                                id="name"
                                label="Naziv"
                                variant={textFieldVariant}
                                onChange={(event) => {
                                    setNoviUnit({
                                        name: event.currentTarget.value,
                                    })
                                }}
                            />

                            <Button
                                variant="contained"
                                startIcon={<AddCircle />}
                                onClick={() => {
                                    adminApi
                                        .put(`/units`, noviUnit)
                                        .then((response) => {
                                            setUnits((prev: any) => [
                                                ...prev,
                                                {
                                                    id: response.data,
                                                    name: noviUnit.name,
                                                },
                                            ])
                                            toast('JM uspešno kreirana!', {
                                                type: 'success',
                                            })
                                        })
                                        .catch((err) => handleApiError(err))
                                }}
                            >
                                <Typography>Kreiraj</Typography>
                            </Button>
                        </Stack>
                    </AccordionDetails>
                </Accordion>
            </Box>
        </Box>
    )
}
