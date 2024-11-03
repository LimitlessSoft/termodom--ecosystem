import { useEffect, useState } from 'react'
import { DataGrid } from '@mui/x-data-grid'
import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
} from '@mui/material'

export const IzborRobeTable = (props) => {
    useEffect(() => {
        if (!props.filter || !props.filter.search) {
            setFilteredRoba(props.roba)
            return
        }

        // split on space or +
        const keywords = props.filter.search.split(/[\s+]/)

        setFilteredRoba(
            props.roba.filter(
                (roba) =>
                    keywords.every((keyword) =>
                        roba.naziv.toLowerCase().includes(keyword.toLowerCase())
                    ) ||
                    keywords.every((keyword) =>
                        roba.katBr.toLowerCase().includes(keyword.toLowerCase())
                    ) ||
                    keywords.every((keyword) =>
                        roba.robaId
                            .toString()
                            .toLowerCase()
                            .includes(keyword.toLowerCase())
                    )
            )
        )
    }, [props.filter, props.filter.search])

    const [pagination, setPagination] = useState({
        page: 0,
        pageSize: 10,
    })

    const [filteredRoba, setFilteredRoba] = useState(props.roba)

    const [inputKolicineOpen, setInputKolicineOpen] = useState(false)
    const [inputRobaId, setInputRobaId] = useState(0)
    const [inputKolicina, setInputKolicina] = useState(0)

    return (
        <Box>
            <Dialog open={inputKolicineOpen}>
                <DialogTitle>Unos kolicine</DialogTitle>
                <DialogContent>
                    <Box p={2}>
                        <TextField
                            label={`Kolicina`}
                            type={`number`}
                            value={props.inputKolicina}
                            onChange={(e) => {
                                setInputKolicina(e.target.value)
                            }}
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setInputKolicineOpen(false)}>
                        Odustani
                    </Button>
                    <Button
                        onClick={() => {
                            props.onSelectRoba(inputRobaId, inputKolicina)
                            setInputKolicineOpen(false)
                        }}
                        variant={`contained`}
                    >
                        Potvrdi
                    </Button>
                </DialogActions>
            </Dialog>
            <DataGrid
                onCellDoubleClick={(params) => {
                    if (props.inputKolicine) {
                        setInputRobaId((prev) => {
                            setInputKolicineOpen(true)
                            return params.row.robaId
                        })
                        return
                    }

                    props.onSelectRoba(params.row.robaId)
                }}
                getRowId={(row) => row.robaId}
                columns={[
                    { field: 'robaId', headerName: 'RobaID' },
                    { field: 'katBr', headerName: 'KatBr', width: 200 },
                    { field: 'naziv', headerName: 'Naziv', flex: 1 },
                    { field: 'jm', headerName: 'JM' },
                ]}
                initialState={{
                    sorting: {
                        sortModel: [{ field: 'naziv', sort: 'asc' }],
                    },
                }}
                paginationModel={pagination}
                onPaginationModelChange={setPagination}
                rows={filteredRoba}
            />
        </Box>
    )
}
