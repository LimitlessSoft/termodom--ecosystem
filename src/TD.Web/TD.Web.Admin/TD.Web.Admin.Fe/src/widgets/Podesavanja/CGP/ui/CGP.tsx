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
import { IProductsPricesGroupsDto } from '@/dtos/responses/products-prices-groups/IProductsPricesGroupsDto'
import {
    GridActionsCellItem,
    GridExpandMoreIcon,
    GridValidRowModel,
} from '@mui/x-data-grid'
import { PodesavanjaTitle } from '@/widgets/Podesavanja/ui/PodesavanjaTitle'
import { AddCircle, Cancel, Delete, Save } from '@mui/icons-material'
import { StripedDataGrid } from '@/widgets/StripedDataGrid'
import { useEffect, useState } from 'react'
import { mainTheme } from '@/theme'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'

export const CGP = (): JSX.Element => {
    const [cenovneGrupeProizvoda, setCenovneGrupeProizvoda] = useState<
        IProductsPricesGroupsDto[] | undefined
    >(undefined)
    const [rowsInEditMode, setRowsInEditMode] = useState<any[]>([])
    const [novaCenovnaGrupa, setNovaCenovnaGrupa] = useState<any>({
        name: null,
    })
    const textFieldVariant = 'standard'

    useEffect(() => {
        adminApi
            .get(`/products-prices-groups`)
            .then((response) => {
                setCenovneGrupeProizvoda(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [])

    return (
        <Box width={`100%`}>
            <PodesavanjaTitle title={`Cenovne grupe proizvoda`} />
            <Box>
                {cenovneGrupeProizvoda === undefined && <LinearProgress />}
                {cenovneGrupeProizvoda !== undefined &&
                    cenovneGrupeProizvoda.length == 0 && (
                        <Typography variant="h6" sx={{ m: 2 }}>
                            Trenutno nema cenovnih grupa proizvoda
                        </Typography>
                    )}
                {cenovneGrupeProizvoda !== undefined &&
                    cenovneGrupeProizvoda.length > 0 && (
                        <StripedDataGrid
                            autoHeight
                            editMode="cell"
                            sx={{ m: 2 }}
                            rows={cenovneGrupeProizvoda}
                            columns={[
                                { field: 'id', headerName: 'Id' },
                                {
                                    field: 'name',
                                    headerName:
                                        'Naziv (dupli klik na grupu za izmenu)',
                                    flex: 1,
                                    editable: true,
                                },
                                {
                                    field: 'actions',
                                    headerName: 'Akcije',
                                    type: 'actions',
                                    getActions: (params: any) => {
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
                                                                `/products-prices-groups`,
                                                                {
                                                                    id: params
                                                                        .row.id,
                                                                    name: params
                                                                        .row
                                                                        .name,
                                                                }
                                                            )
                                                            .then(() => {
                                                                toast(
                                                                    'Cenovna grupa uspešno ažurirana!',
                                                                    {
                                                                        type: 'success',
                                                                    }
                                                                )
                                                                setRowsInEditMode(
                                                                    (old) => [
                                                                        ...old.filter(
                                                                            (
                                                                                x
                                                                            ) =>
                                                                                x !==
                                                                                params.id
                                                                        ),
                                                                    ]
                                                                )
                                                            })
                                                            .catch((err) =>
                                                                handleApiError(
                                                                    err
                                                                )
                                                            )
                                                    }}
                                                />,
                                                <GridActionsCellItem
                                                    key={`cancel-icon-safg-${params.id}`}
                                                    icon={<Cancel />}
                                                    label="Odbaci"
                                                    onClick={() => {
                                                        setRowsInEditMode(
                                                            (old) => [
                                                                ...old.filter(
                                                                    (x) =>
                                                                        x !==
                                                                        params.id
                                                                ),
                                                            ]
                                                        )
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
                                                            `/products-prices-groups/${params.row.id}`
                                                        )
                                                        .then(() => {
                                                            toast(
                                                                'Cenovna grupa uspešno uklonjena!',
                                                                {
                                                                    type: 'success',
                                                                }
                                                            )
                                                            setCenovneGrupeProizvoda(
                                                                (prev: any) => [
                                                                    ...prev.filter(
                                                                        (
                                                                            x: any
                                                                        ) =>
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
                            pageSizeOptions={
                                mainTheme.defaultPagination.options
                            }
                            onCellEditStart={(row: any) => {
                                setRowsInEditMode((old) => [...old, row.id])
                            }}
                            onProcessRowUpdateError={() => {}}
                            processRowUpdate={(newRow: any) => {
                                const updatedRow: GridValidRowModel = {
                                    ...newRow,
                                    isNew: false,
                                }
                                setCenovneGrupeProizvoda((old: any) => [
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
                            getRowClassName={(params: any) =>
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
                        <Typography>Kreiraj novu cenovnu grupu</Typography>
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
                                    setNovaCenovnaGrupa({
                                        name: event.currentTarget.value,
                                    })
                                }}
                            />

                            <Button
                                variant="contained"
                                startIcon={<AddCircle />}
                                onClick={() => {
                                    adminApi
                                        .put(
                                            `/products-prices-groups`,
                                            novaCenovnaGrupa
                                        )
                                        .then((response) => {
                                            setCenovneGrupeProizvoda(
                                                (prev: any) => [
                                                    ...prev,
                                                    {
                                                        id: response.data,
                                                        name: novaCenovnaGrupa.name,
                                                    },
                                                ]
                                            )
                                            toast(
                                                'Cenovna grupa uspešno kreirana!',
                                                {
                                                    type: 'success',
                                                }
                                            )
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
