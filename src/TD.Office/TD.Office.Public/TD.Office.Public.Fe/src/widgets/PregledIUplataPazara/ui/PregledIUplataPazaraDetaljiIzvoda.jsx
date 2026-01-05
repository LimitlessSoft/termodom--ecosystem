import { Badge, Dialog, DialogContent, IconButton } from '@mui/material'
import { ReadMore } from '@mui/icons-material'
import { useState } from 'react'
import { DataGrid } from '@mui/x-data-grid'
import moment from 'moment'
import { DATE_FORMAT } from '../../../constants'

export const PregledIUplataPazaraDetaljiIzvoda = ({ params }) => {
    const [isOpen, setIsOpen] = useState(false)
    const columns = [
        {
            field: 'vrDok',
            headerName: 'Vrsta Dok',
            width: 100,
        },
        {
            field: 'brojIzvoda',
            headerName: 'Broj Izvoda',
            width: 100,
        },
        {
            field: 'ziroRacun',
            headerName: 'Ziro Racun',
            width: 100,
        },
        {
            field: 'konto',
            headerName: 'Konto',
            width: 100,
        },
        {
            field: 'pozivNaBroj',
            headerName: 'Poziv Na Broj',
            width: 150,
        },
        {
            field: 'magacinId',
            headerName: 'Magacin Id',
            type: 'number',
            width: 100,
        },
        {
            field: 'potrazuje',
            headerName: 'Potrazuje',
            type: 'number',
            width: 150,
        },
        {
            field: 'duguje',
            headerName: 'Duguje',
            type: 'number',
            width: 150,
        },
    ]
    return (
        <>
            <Dialog
                open={isOpen}
                fullWidth
                maxWidth="xl"
                onClose={() => setIsOpen(false)}
            >
                <DialogContent>
                    <DataGrid
                        hideFooter={true}
                        columns={columns}
                        rows={params.row.izvodi}
                        getRowId={(row) => {
                            return `${row.vrDok}-${row.brojIzvoda}-${row.datum}-${row.potrazuje}-${row.duguje}`
                        }}
                    />
                </DialogContent>
            </Dialog>
            <IconButton
                color={`secondary`}
                disabled={params.row.izvodi.length === 0}
                onClick={() => {
                    setIsOpen(true)
                }}
            >
                <Badge
                    invisible={params.row.izvodi.length === 0}
                    badgeContent={params.row.izvodi.length}
                    variant={`standard`}
                    color={`secondary`}
                >
                    <ReadMore />
                </Badge>
            </IconButton>
        </>
    )
}
