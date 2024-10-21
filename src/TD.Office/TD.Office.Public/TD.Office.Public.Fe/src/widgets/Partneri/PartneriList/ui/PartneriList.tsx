import {
    Box,
    Button,
    CircularProgress,
    Grid,
    IconButton,
    Pagination,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import {
    PARTNERI_DATA_DEFAULT_VALUE,
    PARTNERI_DEFAULT_CURRENT_PAGE,
    PARTNERI_PAGE_SIZE,
    PARTNERI_PAGINATION_DEFAULT_VALUE,
} from '@/widgets/Partneri/PartneriList/constants'
import { IPartnerDto } from '@/dtos/partneri/IPartnerDto'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS, PERMISSIONS_GROUPS, USER_PERMISSIONS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { PayloadPagination as PayloadPagination } from '@/types'
import { Add, Search } from '@mui/icons-material'
import { mainTheme } from '@/themes'
import { PartneriNewDialog } from '@/widgets/Partneri/PartneriList/ui/PartneriNewDialog'
import { Router, useRouter } from 'next/router'

export const PartneriList = () => {
    const [partneriPagination, setPartneriPagination] = useState<
        PayloadPagination | undefined
    >(PARTNERI_PAGINATION_DEFAULT_VALUE)
    const [partneriData, setPartneriData] = useState<IPartnerDto[] | undefined>(
        PARTNERI_DATA_DEFAULT_VALUE
    )
    const pagePermissions = usePermissions(PERMISSIONS_GROUPS.PARTNERI)
    const [currentPage, setCurrentPage] = useState<number>(
        PARTNERI_DEFAULT_CURRENT_PAGE
    )
    const [currentSearchKeyword, setCurrentSearchKeyword] = useState<string>(``)
    const [serachKeywordInput, setSearchKeywordInput] = useState<string>(``)

    const [isNewDialogOpen, setIsNewDialogOpen] = useState<boolean>(false)

    const router = useRouter()

    useEffect(() => {
        setPartneriPagination(PARTNERI_PAGINATION_DEFAULT_VALUE)
        setPartneriData(PARTNERI_DATA_DEFAULT_VALUE)

        const fetchPartneriData = async () => {
            return await officeApi
                .get(ENDPOINTS.PARTNERS.GET_MULTIPLE, {
                    params: {
                        currentPage,
                        pageSize: PARTNERI_PAGE_SIZE,
                        searchKeyword: currentSearchKeyword,
                    },
                })
                .catch(handleApiError)
        }

        fetchPartneriData().then((response: any) => {
            console.log(response)
            setPartneriData(response.data.payload)
            setPartneriPagination(response.data.pagination)
        })
    }, [currentPage, currentSearchKeyword])

    return (
        <Grid container gap={2}>
            <Grid item xs={12}>
                <Button variant={`contained`} onClick={() => router.push('as')}>
                    Finansijsko i Komercijalno
                </Button>
            </Grid>
            <Grid item xs={12}>
                <Grid container gap={2}>
                    <Grid item xs={12}>
                        <PartneriNewDialog
                            isOpen={isNewDialogOpen}
                            onClose={() => {
                                setIsNewDialogOpen(false)
                            }}
                        />
                        <IconButton
                            onClick={() => {
                                setIsNewDialogOpen(true)
                            }}
                            sx={{
                                backgroundColor: mainTheme.palette.primary.main,
                                color: mainTheme.palette.primary.contrastText,
                            }}
                        >
                            <Add />
                        </IconButton>
                    </Grid>
                    <Grid item xs={12}>
                        <Box>
                            <TextField
                                onKeyDown={(event) => {
                                    if (event.key === `Enter`) {
                                        setCurrentSearchKeyword(
                                            serachKeywordInput
                                        )
                                    }
                                }}
                                defaultValue={serachKeywordInput}
                                onChange={(event) => {
                                    setSearchKeywordInput(event.target.value)
                                }}
                                placeholder={`Unesi pojam za pretragu`}
                                sx={{
                                    minWidth: `300px`,
                                }}
                            />
                            <IconButton
                                size={`medium`}
                                sx={{
                                    m: 1,
                                    backgroundColor:
                                        mainTheme.palette.secondary.main,
                                    color: mainTheme.palette.secondary
                                        .contrastText,
                                }}
                                onClick={() => {
                                    setCurrentSearchKeyword(serachKeywordInput)
                                }}
                            >
                                <Search fontSize={`medium`} />
                            </IconButton>
                        </Box>
                        <Box>
                            {currentSearchKeyword !== `` && (
                                <Typography>
                                    Rezultati pretrage za:{' '}
                                    {currentSearchKeyword}
                                </Typography>
                            )}
                        </Box>
                    </Grid>
                    <Grid item xs={12}>
                        <Stack gap={2} alignItems={`center`}>
                            {(partneriData === undefined ||
                                partneriPagination === undefined) && (
                                <CircularProgress />
                            )}

                            {partneriData !== undefined &&
                                partneriPagination !== undefined &&
                                partneriData.length === 0 && (
                                    <Typography> Nema podataka </Typography>
                                )}

                            {partneriData !== undefined &&
                                partneriData.length > 0 && (
                                    <TableContainer component={Paper}>
                                        <Table>
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell>PPID</TableCell>
                                                    <TableCell>Naziv</TableCell>
                                                    <TableCell>PIB</TableCell>
                                                    <TableCell>
                                                        Adresa
                                                    </TableCell>
                                                    {hasPermission(
                                                        pagePermissions,
                                                        USER_PERMISSIONS
                                                            .PARTNERI
                                                            .VIDI_MOBILNI
                                                    ) && (
                                                        <TableCell>
                                                            Mobilni
                                                        </TableCell>
                                                    )}
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {partneriData.map(
                                                    (partner: IPartnerDto) => (
                                                        <TableRow
                                                            key={partner.ppid}
                                                        >
                                                            <TableCell>
                                                                {partner.ppid}
                                                            </TableCell>
                                                            <TableCell>
                                                                {partner.naziv}
                                                            </TableCell>
                                                            <TableCell>
                                                                {partner.pib}
                                                            </TableCell>
                                                            <TableCell>
                                                                {partner.adresa}
                                                            </TableCell>
                                                            {hasPermission(
                                                                pagePermissions,
                                                                USER_PERMISSIONS
                                                                    .PARTNERI
                                                                    .VIDI_MOBILNI
                                                            ) && (
                                                                <TableCell>
                                                                    {
                                                                        partner.mobilni
                                                                    }
                                                                </TableCell>
                                                            )}
                                                        </TableRow>
                                                    )
                                                )}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                )}
                            {partneriPagination !== undefined && (
                                <Pagination
                                    page={partneriPagination.page}
                                    size={`large`}
                                    count={partneriPagination.totalPages}
                                    variant={`outlined`}
                                    onChange={(event, page) => {
                                        setCurrentPage(page)
                                    }}
                                />
                            )}
                        </Stack>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}
