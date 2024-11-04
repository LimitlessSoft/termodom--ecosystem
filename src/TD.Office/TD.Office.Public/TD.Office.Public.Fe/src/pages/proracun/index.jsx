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
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { useUser } from '@/hooks/useUserHook'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'

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

    const [filters, setFilters] = useState(undefined)
    const [data, setData] = useState(undefined)

    const [triggerReload, setTriggerReload] = useState(false)

    useEffect(() => {
        if (
            filters === undefined ||
            filters.magacinId === undefined ||
            filters.fromUtc === undefined ||
            filters.toUtc === undefined
        )
            return

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

    if (
        hasPermission(
            permissions,
            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI.READ
        ) === false
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
                            USER_PERMISSIONS.PRORACUNI.CREATE_MP
                        ) &&
                            !hasPermission(
                                permissions,
                                USER_PERMISSIONS.PRORACUNI.CREATE_VP
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
                                USER_PERMISSIONS.PRORACUNI.CREATE_MP
                            ) &&
                                !hasPermission(
                                    permissions,
                                    USER_PERMISSIONS.PRORACUNI.CREATE_VP
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
                    magacini={
                        hasPermission(
                            permissions,
                            USER_PERMISSIONS.PRORACUNI.RAD_SA_SVIM_MAGACINIMA
                        )
                            ? magacini
                            : magacini.filter(
                                  (m) =>
                                      m.id === user.data.storeId ||
                                      m.id === user.data.vpStoreId
                              )
                    }
                    onChange={(val) => {
                        if (
                            val === undefined ||
                            val.fromLocal === undefined ||
                            val.toLocal === undefined
                        )
                            return
                        setFilters((prev) => {
                            return {
                                ...prev,
                                fromUtc: val.fromLocal.toISOString(),
                                toUtc: val.toLocal.toISOString(),
                                magacinId: val.magacinId,
                            }
                        })
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
