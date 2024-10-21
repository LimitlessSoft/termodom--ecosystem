import {
    Button,
    FormControl,
    Grid,
    InputLabel,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Paper,
    Select,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Checkbox,
    Typography,
} from '@mui/material'
import React, { useEffect, useMemo, useState } from 'react'
import { Search, SwapVert } from '@mui/icons-material'
import SubModuledLayout from '@/widgets/SubModuledLayout/ui/SubModuledLayout'
import { handleApiError, officeApi } from '@/apis/officeApi'
import qs from 'qs'
import { USER_PERMISSIONS, PERMISSIONS_GROUPS } from '@/constants'
import PartneriFinansijskoIKomercijalnoYearRow from '@/widgets/Partneri/PartneriFinansijskoIKomercijalno/ui/PartneriFinansijskoIKomercijalnoYearRow'
import { ALIGNMENT } from '@/widgets/Partneri/PartneriFinansijskoIKomercijalno/constants'
import { formatYear } from '@/widgets/Partneri/PartneriFinansijskoIKomercijalno/helpers/formatYear'
import {
    FINANSIJSKO,
    KOMERCIJALNO,
    SHOW_DATA_LABEL,
    TABLE_HEAD_FIELDS,
} from '../../widgets/Partneri/PartneriFinansijskoIKomercijalno/constants'
import { useModules } from '../../widgets/SubModuledLayout/ui/hooks/useModules'
import { usePermissions } from '../../hooks/usePermissionsHook'
import { hasPermission } from '../../helpers/permissionsHelpers'

const Partneri = () => {
    const [partners, setPartners] = useState([
        {
            ppid: 111,
            naziv: 'Something',
            komercijalno: [
                {
                    year: 2023,
                    pocetak: 50000,
                    kraj: 150000,
                },
                {
                    year: 2024,
                    pocetak: 75000,
                    kraj: 200000,
                },
            ],
            finansijsko: [
                {
                    year: 2024,
                    pocetak: 100000,
                    kraj: 200000,
                },
            ],
        },
        {
            ppid: 112,
            naziv: 'Something Else',
            komercijalno: [
                {
                    year: 2024,
                    pocetak: 60000,
                    kraj: 180000,
                },
            ],
            finansijsko: [
                {
                    year: 2023,
                    pocetak: 30000,
                    kraj: 120000,
                },
                {
                    year: 2024,
                    pocetak: 140000,
                    kraj: 250000,
                },
            ],
        },
    ])
    const [defaultData, setDefaultData] = useState({
        years: [
            { key: 'TCMDZ 2023', value: 3 },
            { key: 'TCMDZ 2024', value: 4 },
        ],
        defaultTolerancija: 10000,
    })
    const [currentFilter, setCurrentFilter] = useState({
        search: '',
        years: [3, 4],
    })
    const [sortConfig, setSortConfig] = useState({ key: '', direction: '' })

    const modules = [
        {
            href: '/finansijsko&komercijalno',
            label: 'Finansijsko',
            hasPermission: hasPermission(
                usePermissions(PERMISSIONS_GROUPS.PARTNERI),
                USER_PERMISSIONS.PARTNERI.READ
            ),
        },
        {
            href: '/nesto-drugo',
            label: 'Nesto drugo',
            hasPermission: hasPermission(
                usePermissions(
                    PERMISSIONS_GROUPS.PARTNERI_FINANSIJSKO_I_KOMERCIJALNO
                ),
                USER_PERMISSIONS.PARTNERI_FINANSIJSKO_I_KOMERCIJALNO.READ
            ),
        },
    ]

    useEffect(() => {
        officeApi
            .get('/partneri-po-godinama-komercijalno-finansijsko-data', {
                params: {
                    search: currentFilter.search,
                    year: currentFilter.years,
                },
                paramsSerializer: (params) =>
                    qs.stringify(params, { arrayFormat: 'repeat' }),
            })
            .then((res) => res.data)
            .then((data) => setPartners(data))
            .catch(handleApiError)
    }, [currentFilter.search, currentFilter.years])

    useEffect(() => {
        officeApi
            .get('/partneri-po-godinama-komercijalno-finansijsko-data')
            .then((res) => res.data)
            .then((data) => setDefaultData(data))
            .catch(handleApiError)
    }, [])

    const sortData = (key) => {
        let newDirection

        if (sortConfig.key === key) {
            newDirection = sortConfig.direction === 'asc' ? 'desc' : 'asc'
        } else {
            newDirection = 'asc'
        }

        const lowerCaseKey = key.toLowerCase()

        const sortedPartners = [...partners].sort((a, b) => {
            if (key === TABLE_HEAD_FIELDS.PPID) {
                return newDirection === 'asc'
                    ? a[lowerCaseKey] - b[lowerCaseKey]
                    : b[lowerCaseKey] - a[lowerCaseKey]
            } else {
                return newDirection === 'asc'
                    ? a[lowerCaseKey].localeCompare(b[lowerCaseKey])
                    : b[lowerCaseKey].localeCompare(a[lowerCaseKey])
            }
        })

        setPartners(sortedPartners)
        setSortConfig({ key, direction: newDirection })
    }

    return (
        <SubModuledLayout modules={modules}>
            <Stack gap={2}>
                <Grid container gap={4}>
                    <Grid item xs={12}>
                        <Grid container alignItems={`center`} gap={2}>
                            <Grid item xs={6}>
                                <FormControl fullWidth>
                                    <InputLabel>Godine:</InputLabel>
                                    <Select
                                        label={`Godine:`}
                                        value={defaultData.years
                                            .filter((year) =>
                                                currentFilter.years.includes(
                                                    year.value
                                                )
                                            )
                                            .map((year) => year.value)}
                                        multiple
                                        renderValue={(selected) => {
                                            const selectedYears =
                                                defaultData.years
                                                    .filter((year) =>
                                                        selected.includes(
                                                            year.value
                                                        )
                                                    )
                                                    .map((year) =>
                                                        formatYear(year.key)
                                                    )
                                            return selectedYears.join(', ')
                                        }}
                                    >
                                        {defaultData.years.map((year) => {
                                            const isChecked =
                                                currentFilter.years.some(
                                                    (selectedYear) =>
                                                        selectedYear ===
                                                        year.value
                                                )
                                            return (
                                                <ListItem
                                                    key={year.value}
                                                    disablePadding
                                                >
                                                    <ListItemButton
                                                        onChange={(e) =>
                                                            setCurrentFilter(
                                                                (prev) => ({
                                                                    ...prev,
                                                                    search: e
                                                                        .target
                                                                        .value,
                                                                })
                                                            )
                                                        }
                                                    >
                                                        <ListItemIcon>
                                                            <Checkbox
                                                                checked={
                                                                    isChecked
                                                                }
                                                            />
                                                        </ListItemIcon>
                                                        <ListItemText
                                                            primary={formatYear(
                                                                year.key
                                                            )}
                                                        />
                                                    </ListItemButton>
                                                </ListItem>
                                            )
                                        })}
                                    </Select>
                                </FormControl>
                            </Grid>
                            <Grid item>
                                <Button
                                    variant={`contained`}
                                    sx={{
                                        height: '100%',
                                    }}
                                >
                                    <Search />
                                    <Typography>{SHOW_DATA_LABEL}</Typography>
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container justifyContent={`space-between`}>
                            <Grid item>
                                <TextField
                                    variant={`outlined`}
                                    label={`Pretraži:`}
                                    color={`secondary`}
                                    onChange={(e) =>
                                        setCurrentFilter((prev) => ({
                                            ...prev,
                                            search: e.target.value,
                                        }))
                                    }
                                />
                            </Grid>
                            <Grid item>
                                <TextField
                                    variant={`outlined`}
                                    label={`Tolerancija RSD:`}
                                    color={`secondary`}
                                    defaultValue={
                                        defaultData.defaultTolerancija
                                    }
                                    onChange={(e) => {
                                        if (e.target.value !== '') {
                                            setDefaultData((prev) => ({
                                                ...prev,
                                                defaultTolerancija:
                                                    e.target.value || 0,
                                            }))
                                        }
                                    }}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell
                                    align={ALIGNMENT}
                                    onClick={() =>
                                        sortData(TABLE_HEAD_FIELDS.PPID)
                                    }
                                >
                                    <Grid
                                        container
                                        alignItems={ALIGNMENT}
                                        justifyContent={ALIGNMENT}
                                    >
                                        <Grid item>
                                            <Typography>
                                                {TABLE_HEAD_FIELDS.PPID}
                                            </Typography>
                                        </Grid>
                                        <Grid item>
                                            <SwapVert />
                                        </Grid>
                                    </Grid>
                                </TableCell>
                                <TableCell
                                    align={ALIGNMENT}
                                    onClick={() =>
                                        sortData(TABLE_HEAD_FIELDS.NAZIV)
                                    }
                                >
                                    <Grid
                                        container
                                        alignItems={ALIGNMENT}
                                        justifyContent={ALIGNMENT}
                                    >
                                        <Grid item>
                                            <Typography>
                                                {TABLE_HEAD_FIELDS.NAZIV}
                                            </Typography>
                                        </Grid>
                                        <Grid item>
                                            <SwapVert />
                                        </Grid>
                                    </Grid>
                                </TableCell>
                                {defaultData.years.map((year) => (
                                    <React.Fragment key={year.value}>
                                        <TableCell align={ALIGNMENT}>
                                            {`${formatYear(year.key)}_${TABLE_HEAD_FIELDS.POCETAK_SUFFIX}`}
                                        </TableCell>
                                        <TableCell align={ALIGNMENT}>
                                            {`${formatYear(year.key)}_${TABLE_HEAD_FIELDS.KRAJ_SUFFIX}`}
                                        </TableCell>
                                    </React.Fragment>
                                ))}
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {partners.map((partner) => (
                                <React.Fragment key={partner.ppid}>
                                    <PartneriFinansijskoIKomercijalnoYearRow
                                        defaultData={defaultData}
                                        partner={partner}
                                        type={KOMERCIJALNO}
                                    />
                                    <PartneriFinansijskoIKomercijalnoYearRow
                                        defaultData={defaultData}
                                        partner={partner}
                                        type={FINANSIJSKO}
                                    />
                                </React.Fragment>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Stack>
        </SubModuledLayout>
    )
}

export default Partneri
