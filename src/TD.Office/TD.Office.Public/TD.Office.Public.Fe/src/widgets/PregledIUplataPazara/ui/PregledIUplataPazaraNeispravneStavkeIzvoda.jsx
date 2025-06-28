import Grid2 from '@mui/material/Unstable_Grid2'
import { Button, CircularProgress, Dialog, DialogContent } from '@mui/material'
import { officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { useState } from 'react'
import { DataGrid } from '@mui/x-data-grid'
import { CloseIcon } from 'next/dist/client/components/react-dev-overlay/internal/icons/CloseIcon'

export const PregledIUplataPazaraNeispravneStavkeIzvoda = () => {
    const [loading, setLoading] = useState(false)
    const [data, setData] = useState()
    const [isOpen, setIsOpen] = useState(false)
    const columns = [
        {
            field: 'firma',
            headerName: 'Firma',
            width: 100,
        },
        {
            field: 'brojIzvoda',
            headerName: 'Broj izvoda',
            width: 100,
        },
        {
            field: 'ppid',
            headerName: 'PPID',
            width: 100,
        },
        {
            field: 'opis',
            headerName: 'Opis',
            flex: 1,
        },
    ]
    return (
        <Grid2 container gap={2} alignItems={`center`}>
            <Dialog
                open={isOpen}
                onClose={() => setIsOpen(false)}
                maxWidth={`lg`}
                fullWidth={true}
            >
                <DialogContent>
                    {data && (
                        <DataGrid
                            hideFooter={true}
                            columns={columns}
                            rows={data}
                            getRowId={(param) => {
                                return `${param.firma}-${param.brojIzvoda}-${param.ppid}-${param.opis}`
                            }}
                        />
                    )}
                </DialogContent>
            </Dialog>
            <Grid2>
                <Button
                    endIcon={
                        loading && (
                            <CircularProgress color={`primary`} size={`1em`} />
                        )
                    }
                    disabled={loading}
                    variant="contained"
                    color="secondary"
                    onClick={() => {
                        setLoading(true)
                        officeApi
                            .get(
                                ENDPOINTS_CONSTANTS.PREGLED_I_UPLATA_PAZARA
                                    .GET_NEISPRAVO_UNETE_STAVKE_IZVODA
                            )
                            .then((response) => {
                                setData(response.data.items)
                                setIsOpen(true)
                            })
                            .catch((error) => {
                                console.error('Error fetching data:', error)
                            })
                            .finally(() => {
                                setLoading(false)
                            })
                    }}
                >
                    Proveri da li ima neispravno unetih stavki izvoda uplate
                    pazara
                </Button>
            </Grid2>
        </Grid2>
    )
}
