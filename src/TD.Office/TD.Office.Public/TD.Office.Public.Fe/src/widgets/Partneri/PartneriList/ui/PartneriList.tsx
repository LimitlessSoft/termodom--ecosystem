import {
    CircularProgress,
    Grid,
    Pagination,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
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

    useEffect(() => {
        setPartneriPagination(PARTNERI_PAGINATION_DEFAULT_VALUE)
        setPartneriData(PARTNERI_DATA_DEFAULT_VALUE)

        const fetchPartneriData = async () => {
            return await officeApi
                .get(ENDPOINTS.PARTNERS.GET_MULTIPLE, {
                    params: {
                        currentPage,
                        pageSize: PARTNERI_PAGE_SIZE,
                    },
                })
                .catch(handleApiError)
        }

        fetchPartneriData().then((response: any) => {
            console.log(response)
            setPartneriData(response.data.payload)
            setPartneriPagination(response.data.pagination)
        })
    }, [currentPage])

    return (
        <Grid item xs={12}>
            <Stack gap={2} alignItems={`center`}>
                {(partneriData === undefined ||
                    partneriPagination === undefined) && <CircularProgress />}

                {partneriData !== undefined &&
                    partneriPagination !== undefined &&
                    partneriData.length === 0 && (
                        <Typography> Nema podataka </Typography>
                    )}

                {partneriData !== undefined && partneriData.length > 0 && (
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>PPID</TableCell>
                                    <TableCell>Naziv</TableCell>
                                    <TableCell>PIB</TableCell>
                                    <TableCell>Adresa</TableCell>
                                    {hasPermission(
                                        pagePermissions,
                                        USER_PERMISSIONS.PARTNERI.VIDI_MOBILNI
                                    ) && <TableCell>Mobilni</TableCell>}
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {partneriData.map((partner: IPartnerDto) => (
                                    <TableRow key={partner.ppid}>
                                        <TableCell>{partner.ppid}</TableCell>
                                        <TableCell>{partner.naziv}</TableCell>
                                        <TableCell>{partner.pib}</TableCell>
                                        <TableCell>{partner.adresa}</TableCell>
                                        {hasPermission(
                                            pagePermissions,
                                            USER_PERMISSIONS.PARTNERI
                                                .VIDI_MOBILNI
                                        ) && (
                                            <TableCell>
                                                {partner.mobilni}
                                            </TableCell>
                                        )}
                                    </TableRow>
                                ))}
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
    )
}
