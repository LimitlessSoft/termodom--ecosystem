import { useRouter } from 'next/router'
import {
    Autocomplete,
    Box,
    Button,
    CircularProgress,
    LinearProgress,
    MenuItem,
    Stack,
    TextField,
    Tooltip,
    Typography,
} from '@mui/material'
import { toast } from 'react-toastify'
import { useEffect, useState } from 'react'
import { KeyboardDoubleArrowRightRounded, Lock } from '@mui/icons-material'
import { DataGrid } from '@mui/x-data-grid'
import { useZMagacini } from '@/zStore'
import { handleApiError, officeApi } from '@/apis/officeApi'

const ProracunPage = () => {
    const router = useRouter()

    const magacini = useZMagacini()

    const [partneri, setPartneri] = useState<any>([])
    const [partneriSearch, setPartneriSearch] = useState<string>('')
    const [partneriLoading, setPartneriLoading] = useState<boolean>(false)

    const [currentDocument, setCurrentDocument] = useState<any>(undefined)

    useEffect(() => {
        if (router === undefined) return
        if (router.query === undefined) return
        if (router.query.id === undefined) return

        setCurrentDocument({
            id: router.query.id,
            magacinId: 112,
            magacinName: 'Magacin 1',
            datum: '2022-01-01',
        })
    }, [router])

    if (!currentDocument) return <CircularProgress />

    return (
        <Box p={1}>
            <Stack direction={`row`} gap={1} my={2}>
                <TextField
                    aria-readonly={true}
                    value={currentDocument?.id}
                    sx={{
                        maxWidth: 100,
                        '& .MuiInputBase-input': {
                            textAlign: 'right',
                        },
                    }}
                    label="Broj"
                />

                <TextField
                    aria-readonly={true}
                    value={currentDocument?.magacinName}
                    sx={{
                        maxWidth: 400,
                    }}
                    label="Magacin"
                />

                <TextField
                    aria-readonly={true}
                    value={currentDocument?.datum}
                    sx={{
                        maxWidth: 200,
                        '& .MuiInputBase-input': {
                            textAlign: 'center',
                        },
                    }}
                    label="Datum zaključavanja"
                />

                <Tooltip title={`Zaključaj`} arrow>
                    <Button variant={`contained`}>
                        <Lock />
                    </Button>
                </Tooltip>

                <TextField
                    aria-readonly={true}
                    sx={{
                        maxWidth: 150,
                        '& .MuiInputBase-input': {
                            textAlign: 'center',
                        },
                    }}
                    defaultValue={`32 - 12384`}
                    label={`Komercijalno`}
                />
                <Tooltip title={`Pošalji u komercijalno`} arrow>
                    <Button variant={`contained`}>
                        <KeyboardDoubleArrowRightRounded />
                    </Button>
                </Tooltip>
            </Stack>
            <Stack direction={`row`} gap={1}>
                {partneri === undefined ? (
                    <CircularProgress />
                ) : (
                    <Autocomplete
                        sx={{
                            minWidth: 300,
                            maxWidth: 500,
                        }}
                        options={partneri}
                        noOptionsText={`Unesi pretragu i lupi enter...`}
                        onInputChange={(event, value) => {
                            setPartneriSearch(value ?? '')
                        }}
                        onKeyDown={(event) => {
                            if (event.key === 'Enter') {
                                if (partneriSearch.length < 5) {
                                    toast.error(
                                        'Pretraga mora imati bar 5 karaktera'
                                    )
                                    return
                                }

                                setPartneriLoading(true)
                                officeApi
                                    .get(
                                        `/partners?searchKeyword=${partneriSearch}`
                                    )
                                    .then((response) => {
                                        setPartneri(response.data.payload)
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setPartneriLoading(false)
                                    })
                            }
                        }}
                        loading={partneriLoading}
                        loadingText={`Pretraga partnera...`}
                        onChange={(event, value) => {
                            toast.error('Partner promenjen')
                        }}
                        getOptionLabel={(option: any) => {
                            return `${option.naziv}`
                        }}
                        renderInput={(params) => <TextField {...params} />}
                    />
                )}
                <TextField
                    select
                    label="Način plaćanja"
                    sx={{
                        width: 200,
                    }}
                >
                    <MenuItem>
                        <Typography>Gotovina</Typography>
                    </MenuItem>
                    <MenuItem>
                        <Typography>Kartica</Typography>
                    </MenuItem>
                    <MenuItem>
                        <Typography>Virman</Typography>
                    </MenuItem>
                    <MenuItem>
                        <Typography>Ček</Typography>
                    </MenuItem>
                </TextField>
            </Stack>
            <DataGrid
                sx={{
                    my: 2,
                }}
                columns={[
                    {
                        field: 'id',
                        headerName: 'ID',
                        width: 100,
                    },
                    {
                        field: 'proizvodIme',
                        headerName: 'Proizvod',
                        width: 200,
                    },
                    {
                        field: 'kolicina',
                        headerName: 'Količina',
                        width: 150,
                    },
                    {
                        field: 'jedinicaMere',
                        headerName: 'JM',
                        width: 150,
                    },
                    {
                        field: 'rabat',
                        headerName: 'Rabat',
                        width: 150,
                    },
                    {
                        field: 'cena',
                        headerName: 'Cena',
                        width: 150,
                    },
                    {
                        field: 'vrednostBezPdv',
                        headerName: 'Vrednost bez PDV',
                        width: 150,
                    },
                    {
                        field: 'vrednostSaPdv',
                        headerName: 'Vrednost sa PDV',
                        width: 150,
                    },
                ]}
                rows={[
                    {
                        id: 1,
                        proizvodIme: 'Proizvod 1',
                        kolicina: 1,
                        jedinicaMere: 'kom',
                        rabat: 0,
                        cena: 1000,
                        vrednostBezPdv: 1000,
                        vrednostSaPdv: 1200,
                    },
                    {
                        id: 2,
                        proizvodIme: 'Proizvod 2',
                        kolicina: 2,
                        jedinicaMere: 'kom',
                        rabat: 0,
                        cena: 2000,
                        vrednostBezPdv: 4000,
                        vrednostSaPdv: 4800,
                    },
                ]}
            />
        </Box>
    )
}

export default ProracunPage
