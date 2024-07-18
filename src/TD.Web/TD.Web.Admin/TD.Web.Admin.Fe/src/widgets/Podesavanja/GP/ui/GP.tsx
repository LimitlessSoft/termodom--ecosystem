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
    GridRenderCellParams,
    GridValidRowModel,
} from '@mui/x-data-grid'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { GPTypeBox } from './GPTypeBox'
import { adminApi } from '@/apis/adminApi'

export const GP = (): JSX.Element => {
    const [grupeProizvoda, setGrupeProizvoda] = useState<any | undefined>(null)
    const [rowsInEditMode, setRowsInEditMode] = useState<any[]>([])
    const [productGroupTypes, setProductGroupTypes] = useState<
        any[] | undefined
    >(undefined)

    const [novaGrupa, setNovaGrupa] = useState<any>({
        name: null,
    })
    const textFieldVariant = 'standard'

    useEffect(() => {
        adminApi.get(`/products-groups`).then((response) => {
            setGrupeProizvoda(response.data)
        })

        adminApi.get(`/product-group-types`).then((response) => {
            setProductGroupTypes(response.data)
        })
    }, [])

    return (
        <Box sx={{ width: '100%' }}>
            <Box>
                <Typography sx={{ m: 2 }} variant="h6">
                    Grupe proizvoda
                </Typography>
            </Box>
            <Box>
                {grupeProizvoda == null || productGroupTypes === undefined ? (
                    <LinearProgress />
                ) : (
                    <StripedDataGrid
                        autoHeight
                        editMode="cell"
                        sx={{ m: 2 }}
                        rows={grupeProizvoda}
                        columns={[
                            { field: 'id', headerName: 'Id' },
                            {
                                field: 'name',
                                headerName: 'Naziv',
                                flex: 1,
                                editable: true,
                            },
                            {
                                field: 'welcomeMessage',
                                headerName: 'Poruka dobrodošlice',
                                flex: 1,
                                editable: true,
                            },
                            {
                                field: 'typeId',
                                headerAlign: `center`,
                                headerName: 'Tip',
                                minWidth: 150,
                                renderCell: (
                                    params: GridRenderCellParams<any, number>
                                ) => (
                                    <GPTypeBox
                                        params={params}
                                        productGroupTypes={productGroupTypes}
                                    />
                                ),
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
                                                        .put(
                                                            `/products-groups`,
                                                            {
                                                                id: params.row
                                                                    .id,
                                                                name: params.row
                                                                    .name,
                                                                welcomeMessage:
                                                                    params.row
                                                                        .welcomeMessage,
                                                            }
                                                        )
                                                        .then(() => {
                                                            toast.success(
                                                                'Grupa uspešno ažurirana!'
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
                                                        `/products-groups/${params.row.id}`
                                                    )
                                                    .then(() => {
                                                        toast.success(
                                                            'Grupa uspešno obrisana!'
                                                        )
                                                        setGrupeProizvoda(
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
                            setGrupeProizvoda((old: any) => [
                                ...old.filter(
                                    (x: any) => x.id !== updatedRow.id
                                ),
                                {
                                    id: updatedRow.id,
                                    name: updatedRow.name,
                                    welcomeMessage: updatedRow.welcomeMessage,
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
                        <Typography>Kreiraj novu grupu</Typography>
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
                                    setNovaGrupa({
                                        name: event.currentTarget.value,
                                    })
                                }}
                            />

                            <Button
                                variant="contained"
                                startIcon={<AddCircle />}
                                onClick={() => {
                                    adminApi
                                        .put(`/products-groups`, novaGrupa)
                                        .then((response) => {
                                            setGrupeProizvoda((prev: any) => [
                                                ...prev,
                                                {
                                                    id: response.data,
                                                    name: novaGrupa.name,
                                                },
                                            ])

                                            toast('Grupa uspešno kreirana!', {
                                                type: 'success',
                                            })
                                        })
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
