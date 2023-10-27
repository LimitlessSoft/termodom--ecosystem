import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { AddCircle, Cancel, Delete, Edit, Save } from "@mui/icons-material"
import { Accordion, AccordionDetails, AccordionSummary, Box, Button, Grid, LinearProgress, Stack, TextField, Typography } from "@mui/material"
import { DataGrid, GridActionsCellItem, GridDeleteIcon, GridExpandMoreIcon, GridSaveAltIcon } from "@mui/x-data-grid"
import { useEffect, useState } from "react"
import { toast } from "react-toastify"

export const CGP = (): JSX.Element => {

    const [cenovneGrupeProizvoda, setCenovneGrupeProizvoda] = useState<any | undefined>(null)
    const [rowsInEditMode, setRowsInEditMode] = useState<any[]>([])
    const [novaCenovnaGrupa, setNovaCenovnaGrupa] = useState<any>({
        name: null
    })
    const textFieldVariant = 'standard'

    useEffect(() => {
        fetchApi(ApiBase.Main, "/products-prices-groups")
            .then((payload) => {
                setCenovneGrupeProizvoda(payload)
            })
    }, [])

    return (
        <Box sx={{ width: '100%' }}>
            <Box>
                <Typography
                    sx={{ m: 2 }}
                    variant='h6'>
                    Cenovne grupe proizvoda
                </Typography>
            </Box>
            <Box>
                {
                    cenovneGrupeProizvoda == null ?
                    <LinearProgress /> :
                    <DataGrid
                        autoHeight
                        editMode='cell'
                        sx={{ m: 2 }}
                        rows={cenovneGrupeProizvoda}
                        columns={[
                            { field: 'id', headerName: 'Id' },
                            { field: 'name', headerName: 'Naziv', flex: 1, editable: true },
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
                                                    fetchApi(ApiBase.Main, "/products-prices-groups", {
                                                        method: 'PUT',
                                                        body: { id: params.row.id, name: params.row.name },
                                                        contentType: ContentType.ApplicationJson
                                                    }).then(() => {
                                                        toast('Cenovna grupa uspešno ažurirana!', { type: 'success' })
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
                                                fetchApi(ApiBase.Main, `/products-prices-groups/${params.row.id}`, {
                                                    method: 'DELETE'
                                                }).then(() => {
                                                    toast('Cenovna grupa uspešno uklonjena!', { type: 'success' })
                                                    setCenovneGrupeProizvoda((prev: any) => [ ...prev.filter((x: any) => x.id !== params.row.id)])
                                                })
                                            }}
                                        />
                                    ]
                                }
                            }
                        ]}
                        initialState={{
                            pagination: {
                                paginationModel: { page: 0, pageSize: 10 }
                            }
                        }}
                        pageSizeOptions={[5, 10]}
                        onCellEditStart={(row) => {
                            setRowsInEditMode((old) => [ ...old, row.id ])
                        }}
                        onProcessRowUpdateError={(error) => {
                            console.log(error)
                        }}
                        processRowUpdate={(newRow) => {
                            const updatedRow = { ...newRow, isNew: false };
                            setCenovneGrupeProizvoda((old: any) => [
                                ...old.filter((x: any) => x.id !== updatedRow.id),
                                {
                                    id: updatedRow.id,
                                    name: updatedRow.name
                                }
                            ])
                            //handle send data to api
                            return updatedRow;
                        }}
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
                                Kreiraj novu cenovnu grupu
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
                                    setNovaCenovnaGrupa({ name: event.currentTarget.value })
                                }} />

                            <Button
                                variant="contained"
                                startIcon={<AddCircle />}
                                onClick={() => {
                                    fetchApi(ApiBase.Main, '/products-prices-groups', {
                                        method: "PUT",
                                        body: novaCenovnaGrupa,
                                        contentType: ContentType.ApplicationJson
                                    }).then((payload) => {
                                        setCenovneGrupeProizvoda((prev: any) => [
                                            ...prev,
                                            {
                                                id: payload,
                                                name: novaCenovnaGrupa.name
                                            }
                                        ])
                                        toast("Cenovna grupa uspešno kreirana!", { type: 'success' })
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