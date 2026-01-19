import {
    CircularProgress,
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
} from '@mui/material'
import { KorisniciListRow } from './KorisniciListRow'
import { KorisniciNovi } from './KorisniciNovi'
import { useEffect, useMemo, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '@/constants'

export const KorisniciList = () => {
    const [data, setData] = useState(undefined)
    const [tipoviKorisnika, setTipoviKorisnika] = useState([])
    const [query, setQuery] = useState('')
    const [selectedTipKorisnikaId, setSelectedTipKorisnikaId] = useState('')

    useEffect(() => {
        officeApi
            .get(`/users?pageSize=100`)
            .then((response) => {
                setData(response.data.payload)
            })
            .catch((err) => handleApiError(err))

        officeApi
            .get(ENDPOINTS_CONSTANTS.TIP_KORISNIKA.GET_MULTIPLE)
            .then((response) => {
                setTipoviKorisnika(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [])

    const filtered = useMemo(() => {
        if (!data) return []
        let result = data

        // Filter by search query
        const q = query.trim().toLowerCase()
        if (q) {
            result = result.filter((k) => {
                const id = String(k.id ?? '').toLowerCase()
                const nickname = String(k.nickname ?? '').toLowerCase()
                const username = String(k.username ?? '').toLowerCase()
                return (
                    id.includes(q) || nickname.includes(q) || username.includes(q)
                )
            })
        }

        // Filter by user type
        if (selectedTipKorisnikaId) {
            result = result.filter((k) => k.tipKorisnikaId === selectedTipKorisnikaId)
        }

        return result
    }, [data, query, selectedTipKorisnikaId])

    return (
        <Grid p={2} container>
            <Grid item sm={12}>
                <KorisniciNovi tipoviKorisnika={tipoviKorisnika} />
            </Grid>
            <Grid item sm={12} mt={1} mb={1}>
                <Grid container spacing={2}>
                    <Grid item sm={8}>
                        <TextField
                            fullWidth
                            size="small"
                            label="Pretraga"
                            placeholder="Id, nadimak ili korisničko ime"
                            value={query}
                            onChange={(e) => setQuery(e.target.value)}
                        />
                    </Grid>
                    <Grid item sm={4}>
                        <FormControl fullWidth size="small">
                            <InputLabel>Tip korisnika</InputLabel>
                            <Select
                                value={selectedTipKorisnikaId}
                                label="Tip korisnika"
                                onChange={(e) => setSelectedTipKorisnikaId(e.target.value)}
                            >
                                <MenuItem value="">Svi tipovi</MenuItem>
                                {tipoviKorisnika.map((tip) => (
                                    <MenuItem key={tip.id} value={tip.id}>
                                        {tip.naziv}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </Grid>
                </Grid>
            </Grid>
            {!data && <CircularProgress />}
            {data && data.length === 0 && <Grid>Nema podataka</Grid>}
            {data && data.length > 0 && (
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Id</TableCell>
                                <TableCell>Nadimak</TableCell>
                                <TableCell>Korisničko ime</TableCell>
                                <TableCell>Tip</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {filtered?.map((korisnik, index) => (
                                <KorisniciListRow
                                    key={index}
                                    korisnik={korisnik}
                                />
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            )}
        </Grid>
    )
}
