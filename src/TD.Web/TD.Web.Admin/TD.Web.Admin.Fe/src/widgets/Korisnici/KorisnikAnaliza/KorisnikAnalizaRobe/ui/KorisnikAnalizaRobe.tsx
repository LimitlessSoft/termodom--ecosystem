import {
    Grid,
    LinearProgress,
    MenuItem,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TableSortLabel,
    TextField,
    Typography,
} from '@mui/material'
import { KorisnikAnalizaPanel } from '../../ui/KorisnikAnalizaPanel'
import { useEffect, useState } from 'react'
import { formatNumber } from '@/helpers/numberHelpers'
import { adminApi, handleApiError } from '@/apis/adminApi'

const TableHeaderCell = (props: any): JSX.Element => {
    return (
        <TableCell sortDirection={props.sortDir === `asc` ? `asc` : `desc`}>
            <TableSortLabel
                active={props.sortBy === props.name}
                direction={props.sortDir === `asc` ? `asc` : `desc`}
                onClick={() => {
                    props.setSortBy(props.name)
                    props.setSortDir(props.sortDir === `asc` ? `desc` : `asc`)
                }}
            >
                {props.label}
            </TableSortLabel>
        </TableCell>
    )
}

export const KorisnikAnalizaRobe = (props: any): JSX.Element => {
    const [sortedData, setSortedData] = useState<any[] | undefined>(undefined)
    const [data, setData] = useState<any[] | undefined>(undefined)
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [range, setRange] = useState<number>(1)
    const [sortBy, setSortBy] = useState<string>(`valueSum`)
    const [sortDir, setSortDir] = useState<string>(`desc`)
    const [filterTerm, setFilterTerm] = useState<string | undefined>(undefined)

    useEffect(() => {
        setData(undefined)
        setIsLoading(true)
        if (props.username === undefined) return

        adminApi
            .get(
                `/users-analyze-ordered-products/${props.username}?range=${range}&username=${props.username}`
            )
            .then((response) => {
                setData(response.data.items)
                setSortBy(`quantitySum`)
            })
            .catch((err) => handleApiError(err))
            .finally(() => {
                setIsLoading(false)
            })
    }, [props.username, range])

    useEffect(() => {
        setSortedData(undefined)

        if (!data) return

        setSortedData(
            data
                .filter((row: any) => {
                    if (!filterTerm || filterTerm.length == 0) return true
                    return row.name
                        .toLowerCase()
                        .includes(filterTerm.toLowerCase())
                })
                .toSorted((a: any, b: any) => {
                    return (
                        (a[sortBy] < b[sortBy] ? -1 : 1) *
                        (sortDir === `asc` ? 1 : -1)
                    )
                })
        )
    }, [sortBy, sortDir, data, filterTerm])

    return (
        <KorisnikAnalizaPanel item sm={6}>
            <Stack>
                <Grid container spacing={1}>
                    <Grid item flex={1}>
                        <Typography m={2} variant={`h6`}>
                            Analiza robe u porudžbinama
                        </Typography>
                    </Grid>
                    <Grid item>
                        <TextField
                            disabled={isLoading}
                            placeholder={`Filter`}
                            onChange={(e) => {
                                setFilterTerm(e.target.value)
                            }}
                        />
                    </Grid>
                    <Grid item>
                        <TextField
                            select
                            defaultValue={1}
                            disabled={isLoading}
                            onChange={(e) => {
                                setRange(parseInt(e.target.value))
                            }}
                        >
                            <MenuItem value={0}>Poslednjih 30 dana</MenuItem>
                            <MenuItem value={1}>Ove godine</MenuItem>
                            <MenuItem value={2}>
                                Poslednjih godinu dana
                            </MenuItem>
                            <MenuItem value={3}>Od kreiranja naloga</MenuItem>
                        </TextField>
                    </Grid>
                </Grid>
                <Grid>{isLoading && <LinearProgress />}</Grid>
                {!isLoading && (
                    <TableContainer component={Paper}>
                        <Table size={`small`} stickyHeader>
                            <TableHead>
                                <TableRow>
                                    <TableHeaderCell
                                        sortDir={sortDir}
                                        sortBy={sortBy}
                                        setSortBy={setSortBy}
                                        setSortDir={setSortDir}
                                        name={`name`}
                                        label={`Proizvod`}
                                    />
                                    <TableHeaderCell
                                        sortDir={sortDir}
                                        sortBy={sortBy}
                                        setSortBy={setSortBy}
                                        setSortDir={setSortDir}
                                        name={`quantitySum`}
                                        label={`Zbir količine`}
                                    />
                                    <TableHeaderCell
                                        sortDir={sortDir}
                                        sortBy={sortBy}
                                        setSortBy={setSortBy}
                                        setSortDir={setSortDir}
                                        name={`valueSum`}
                                        label={`Zbir vrednosti`}
                                    />
                                    <TableHeaderCell
                                        sortDir={sortDir}
                                        sortBy={sortBy}
                                        setSortBy={setSortBy}
                                        setSortDir={setSortDir}
                                        name={`discountSum`}
                                        label={`Ostvarena ušteda`}
                                    />
                                    <TableCell></TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {sortedData?.map((row, index) => (
                                    <TableRow key={index}>
                                        <TableCell>{row.name}</TableCell>
                                        <TableCell>
                                            {formatNumber(row.quantitySum)}
                                        </TableCell>
                                        <TableCell>
                                            {formatNumber(row.valueSum)}
                                        </TableCell>
                                        <TableCell>
                                            {formatNumber(row.discountSum)}
                                        </TableCell>
                                        <TableCell></TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                )}
            </Stack>
        </KorisnikAnalizaPanel>
    )
}
