import { Box, IconButton } from '@mui/material'
import {
    HorizontalActionBar,
    HorizontalActionBarButton,
    ProracunNoviDialog,
    ProracunTable,
} from '@/widgets'
import { useRouter } from 'next/router'
import { AddCircle } from '@mui/icons-material'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { ProracunFilters } from '@/widgets/Proracun/ProracunFilters/ui/ProracunFilters'
import { useZMagacini } from '@/zStore'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { useUser } from '@/hooks/useUserHook'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import {
    PERMISSIONS_CONSTANTS,
    ENDPOINTS_CONSTANTS,
    DATE_CONSTANTS,
} from '@/constants'
import dayjs from 'dayjs'

const ProracunPage = () => {
    const router = useRouter()
    const magacini = useZMagacini()
    const user = useUser(false)

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PRORACUNI
    )

    const [isLoading, setIsLoading] = useState(false)

    const [noviProracunDialogOpen, setNoviProracunDialogOpen] = useState(false)

    const [pagination, setPagination] = useState({
        pageSize: 10,
        page: 0,
    })

    const [filters, setFilters] = useState({
        fromUtc: new Date().toISOString(),
        toUtc: new Date().toISOString(),
        magacinId: user.data?.storeId,
    })

    const [data, setData] = useState(undefined)

    const [triggerReload, setTriggerReload] = useState(false)

    const notAplliedAllFilters =
        !filters || !filters.magacinId || !filters.fromUtc || !filters.toUtc

    useEffect(() => {
        setFilters((prev) => ({
            ...prev,
            magacinId: user.data?.storeId,
        }))
    }, [user])

    useEffect(() => {
        if (notAplliedAllFilters) return

        setData(undefined)
        setIsLoading(true)

        officeApi
            .get(ENDPOINTS_CONSTANTS.PRORACUNI.GET_MULTIPLE, {
                params: {
                    ...filters,
                    currentPage: pagination.page + 1,
                    pageSize: pagination.pageSize,
                },
            })
            .then((response) => {
                setData(response.data)
            })
            .catch(handleApiError)
            .finally(() => {
                setIsLoading(false)
            })
    }, [pagination, triggerReload])

    useEffect(() => {
        setPagination((prev) => {
            return {
                ...prev,
                page: 0,
            }
        })
    }, [filters])

    const handleFiltersChange = (filters) => {
        setFilters((prev) => ({
            fromUtc: filters.fromUtc
                ? dayjs(filters.fromUtc, DATE_CONSTANTS.FORMAT).toISOString()
                : prev.fromUtc,
            toUtc: filters.toUtc
                ? dayjs(filters.toUtc, DATE_CONSTANTS.FORMAT).toISOString()
                : prev.toUtc,
            magacinId: filters.magacinId || prev.magacinId,
        }))
    }

    if (
        !hasPermission(
            permissions,
            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI.READ
        )
    )
        return

    return (
        <Box>
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text="Nazad"
                    onClick={() => router.push(`/`)}
                    disabled={isLoading}
                />
            </HorizontalActionBar>
            <HorizontalActionBar>
                <ProracunNoviDialog
                    open={noviProracunDialogOpen}
                    onClose={() => {
                        setNoviProracunDialogOpen(false)
                    }}
                    onCancel={() => {
                        setNoviProracunDialogOpen(false)
                    }}
                    onSuccess={() => {
                        toast.success('Proračun uspješno kreiran')
                        setNoviProracunDialogOpen(false)
                        setTriggerReload((prev) => !prev)
                    }}
                />
                <IconButton
                    disabled={
                        isLoading ||
                        (!hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .CREATE_MP
                        ) &&
                            !hasPermission(
                                permissions,
                                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                    .CREATE_VP
                            ))
                    }
                    onClick={() => {
                        setNoviProracunDialogOpen(true)
                    }}
                >
                    <AddCircle
                        color={
                            isLoading ||
                            (!hasPermission(
                                permissions,
                                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                    .CREATE_MP
                            ) &&
                                !hasPermission(
                                    permissions,
                                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                        .PRORACUNI.CREATE_VP
                                ))
                                ? `disabled`
                                : `primary`
                        }
                        fontSize={`large`}
                    />
                </IconButton>
            </HorizontalActionBar>
            {magacini && user && user.data && (
                <ProracunFilters
                    defaultMagacin={user.data.storeId}
                    disabled={isLoading}
                    filters={filters}
                    magacini={
                        hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .RAD_SA_SVIM_MAGACINIMA
                        )
                            ? magacini
                            : magacini.filter(
                                  (m) =>
                                      m.id === user.data.storeId ||
                                      m.id === user.data.vpStoreId
                              )
                    }
                    onFilterChange={(filters) => {
                        if (notAplliedAllFilters) return
                        handleFiltersChange(filters)
                    }}
                />
            )}
            {magacini && (
                <ProracunTable
                    data={data}
                    magacini={magacini}
                    pagination={pagination}
                    setPagination={setPagination}
                />
            )}
        </Box>
    )
}

export default ProracunPage
