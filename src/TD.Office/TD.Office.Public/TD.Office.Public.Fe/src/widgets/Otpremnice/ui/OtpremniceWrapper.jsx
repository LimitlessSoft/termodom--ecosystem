import { CircularProgress, IconButton, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { MagaciniDropdown } from '../../MagaciniDropdown/ui/MagaciniDropdown'
import { toast } from 'react-toastify'
import { AddCircle } from '@mui/icons-material'
import { HorizontalActionBar } from '../../TopActionBar/ui/HorizontalActionBar'
import { OtpremnicaNoviDialog } from './OtpremnicaNoviDialog'
import { OtpremniceTable } from './OtpremniceTable'
import { useZMagacini } from '../../../zStore'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { useUser } from '../../../hooks/useUserHook'
import { otpremniceHelpers } from '../../../helpers/otpremniceHelpers'

export const OtpremniceWrapper = ({ type }) => {
    const zMagacini = useZMagacini()
    const currentUser = useUser(true)

    const [isLoading, setIsLoading] = useState(false)
    const [novaOtpremnicaDialogOpen, setNovaOtpremnicaDialogOpen] =
        useState(false)

    const [data, setData] = useState(undefined)

    const [filters, setFilters] = useState({
        vrsta: type.split(' ')[1],
    })

    const [pagination, setPagination] = useState({
        pageSize: 10,
        page: 0,
    })
    
    useEffect(() => {
        if (!currentUser.data) return
        setData(undefined)
        setIsLoading(true)
        officeApi
            .get(ENDPOINTS_CONSTANTS.OTPREMNICE.GET_MULTIPLE, {
                params: {
                    ...filters,
                },
            })
            .then((res) => {
                setData(res.data)
            })
            .catch(handleApiError)
            .finally(() => {
                setIsLoading(false)
            })
    }, [pagination, currentUser.data])

    useEffect(() => {
        setPagination((prev) => ({ ...prev, page: 0 }))
    }, [filters])
    
    if (!zMagacini) return <CircularProgress />

    return (
        <Stack gap={2}>
            <Typography variant={`h5`}>{type} otpremnice</Typography>
            <HorizontalActionBar>
                <OtpremnicaNoviDialog
                    type={type}
                    open={novaOtpremnicaDialogOpen}
                    onClose={() => {
                        setNovaOtpremnicaDialogOpen(false)
                    }}
                    onCancel={() => {
                        setNovaOtpremnicaDialogOpen(false)
                    }}
                    onSuccess={() => {
                        toast.success('Otpremnica uspešno kreirana')
                        setNovaOtpremnicaDialogOpen(false)
                        // setTriggerReload((prev) => !prev)
                    }}
                />
                <IconButton
                    disabled={
                        isLoading
                        // ||
                        // (!hasPermission(
                        //     permissions,
                        //     PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                        //         .CREATE_MP
                        // ) &&
                        //     !hasPermission(
                        //         permissions,
                        //         PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                        //             .CREATE_VP
                        //     ))
                    }
                    onClick={() => {
                        setNovaOtpremnicaDialogOpen(true)
                    }}
                >
                    <AddCircle
                        color={
                            isLoading
                                ? // ||
                                  // (!hasPermission(
                                  //     permissions,
                                  //     PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                  //         .CREATE_MP
                                  // ) &&
                                  //     !hasPermission(
                                  //         permissions,
                                  //         PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                  //             .PRORACUNI.CREATE_VP
                                  //     ))
                                  `disabled`
                                : `primary`
                        }
                        fontSize={`large`}
                    />
                </IconButton>
            </HorizontalActionBar>

            <MagaciniDropdown
                excluteContainingStar
                disabled={isLoading}
                types={otpremniceHelpers.magaciniVrste(type)}
                onChange={(e) => setFilters({ ...filters, magacinId: e })}
            />
            <OtpremniceTable
                type={type}
                data={data}
                magacini={zMagacini}
                pagination={pagination}
                setPagination={setPagination}
            />
        </Stack>
    )
}
