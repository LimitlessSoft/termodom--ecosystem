import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { mainTheme } from "@/app/theme"
import { StripedDataGrid } from "@/widgets/StripedDataGrid"
import { AddCircle, Cancel, Delete, Edit, Save } from "@mui/icons-material"
import { Accordion, AccordionDetails, AccordionSummary, Box, Button, Grid, LinearProgress, Stack, TextField, Typography } from "@mui/material"
import { DataGrid, GridActionsCellItem, GridDeleteIcon, GridExpandMoreIcon, GridSaveAltIcon, GridValidRowModel } from "@mui/x-data-grid"
import { useEffect, useState } from "react"
import { toast } from "react-toastify"

export const GP = (): JSX.Element => {

    const [grupeProizvoda, setGrupeProizvoda] = useState<any | undefined>(null)
    const [rowsInEditMode, setRowsInEditMode] = useState<any[]>([])
    const [novaGrupa, setNovaGrupa] = useState<any>({
        name: null
    })
    const textFieldVariant = 'standard'

    useEffect(() => {
        fetchApi(ApiBase.Main, "/products-groups")
            .then((payload) => {
                setGrupeProizvoda(payload)
            })
    }, [])

    return (
        <Box sx={{ width: '100%' }}>
            <Box>
                <Typography
                    sx={{ m: 2 }}
                    variant='h6'>
                    Grupe proizvoda
                </Typography>
            </Box>
            <Box>
                {
                    grupeProizvoda == null ?
                    <LinearProgress /> :
                    <StripedDataGrid
                        autoHeight
                        editMode='cell'
                        sx={{ m: 2 }}
                        rows={grupeProizvoda}
                        columns={[
                            { field: 'id', headerName: 'Id' },
                            { field: 'name', headerName: 'Naziv', flex: 1, editable: true },
                            { field: 'welcomeMessage', headerName: 'Poruka dobrodošlice', flex: 1, editable: true},
                            {
                                field: 'actions',
                                headerName: 'Akcije',
                                type: 'actions',
                                getActions: (params) => {
                                    const isInEditMode = rowsInEditMode.find(x => x == params.id) != null

                                    if(isInEditMode) {
                                        return [
                                            <GridActionsCellItem
                                                key={`save-icon-safg-${params.id}`}
                                                icon={<Save />}
                                                label='Sačuvaj'
                                                onClick={() => {
                                                    fetchApi(ApiBase.Main, "/products-groups", {
                                                        method: 'PUT',
                                                        body: { id: params.row.id, name: params.row.name, welcomeMessage: params.row.welcomeMessage },
                                                        contentType: ContentType.ApplicationJson
                                                    }).then(() => {
                                                        toast('Grupa uspešno ažurirana!', { type: 'success' })
                                                        setRowsInEditMode((old) => [ ...old.filter(x => x !== params.id) ])
                                                    })
                                                }}
                                            />,
                                            <GridActionsCellItem
                                                key={`cancel-icon-safg-${params.id}`}
                                                icon={<Cancel />}
                                                label='Odbaci'
                                                onClick={() => {
                                                    setRowsInEditMode((old) => [ ...old.filter(x => x !== params.id) ])
                                                }}
                                            />,
                                        ]
                                    }
                                    return [
                                        <GridActionsCellItem
                                            key={`delete-icon-vas${params.id}`}
                                            icon={<Delete />}
                                            label='Obriši'
                                            onClick={() => {
                                                fetchApi(ApiBase.Main, `/products-groups/${params.row.id}`, {
                                                    method: 'DELETE'
                                                }).then(() => {
                                                    toast('Grupa uspešno uklonjena!', { type: 'success' })
                                                    grupeProizvoda((prev: any) => [ ...prev.filter((x: any) => x.id !== params.row.Id)])
                                                })
                                            }}
                                        />
                                    ]
                                }
                            }
                        ]}
                        initialState={{
                            sorting: {
                                sortModel: [{ field: 'id', sort: 'asc' }]
                            },
                            pagination: {
                                paginationModel: { page: 0, pageSize: mainTheme.defaultPagination.default }
                            }
                        }}
                        pageSizeOptions={mainTheme.defaultPagination.options}
                        onCellEditStart={(row) => {
                            setRowsInEditMode((old) => [ ...old, row.id ])
                        }}
                        onProcessRowUpdateError={(error) => {
                            console.log(error)
                        }}
                        processRowUpdate={(newRow) => {
                            const updatedRow: GridValidRowModel = { ...newRow, isNew: false };
                            setGrupeProizvoda((old: any) => [
                                ...old.filter((x: any) => x.id !== updatedRow.id),
                                {
                                    id: updatedRow.id,
                                    name: updatedRow.name,
                                    welcomeMessage: updatedRow.welcomeMessage
                                }
                            ])
                            //handle send data to api
                            return updatedRow;
                        }}
                        getRowClassName={(params) =>
                            params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd'
                        }
                        />
                }
            </Box>
            <Box
                sx={{ m: 2 }}>
                <Accordion>
                    <AccordionSummary
                        expandIcon={<GridExpandMoreIcon />}
                        aria-controls="panel1a-content"
                        id='panel1a-header'>
                            <Typography>
                                Kreiraj novu grupu
                            </Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Stack
                            direction={'row'}
                            spacing={2}
                            alignItems={'center'}>

                            <TextField
                                required
                                id='name'
                                label='Naziv'
                                variant={textFieldVariant}
                                onChange={(event) => {
                                    setNovaGrupa({ name: event.currentTarget.value })
                                }} />

                            <Button
                                variant="contained"
                                startIcon={<AddCircle />}
                                onClick={() => {
                                    fetchApi(ApiBase.Main, '/products-groups', {
                                        method: "PUT",
                                        body: novaGrupa,
                                        contentType: ContentType.ApplicationJson
                                    }).then((payload) => {
                                        setGrupeProizvoda((prev: any) => [
                                            ...prev,
                                            {
                                                id: payload,
                                                name: novaGrupa.name
                                            }
                                        ])
                                        toast("Grupa uspešno kreirana!", { type: 'success' })
                                    })
                                }}
                                >
                                <Typography>
                                    Kreiraj
                                </Typography>
                            </Button>

                        </Stack>
                    </AccordionDetails>
                </Accordion>
            </Box>
        </Box>
    )
}