import { Box, IconButton, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { MagaciniDropdown } from '../../MagaciniDropdown/ui/MagaciniDropdown'
import { toast } from 'react-toastify'
import { AddCircle } from '@mui/icons-material'
import { HorizontalActionBar } from '../../TopActionBar/ui/HorizontalActionBar'
import { OtpremnicaNoviDialog } from './OtpremnicaNoviDialog'
import { OtpremniceTable } from './OtpremniceTable'
import { useZMagacini } from '../../../zStore'

export const OtpremniceWrapper = ({ type }) => {
    const zMagacini = useZMagacini()

    const [isLoading, setIsLoading] = useState(false)
    const [novaOtpremnicaDialogOpen, setNovaOtpremnicaDialogOpen] =
        useState(false)

    const [data, setData] = useState(undefined)

    const [pagination, setPagination] = useState({
        pageSize: 10,
        page: 0,
    })
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
                            `primary`
                            // isLoading
                            // ||
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
                            //     ? `disabled`
                            //     : `primary`
                        }
                        fontSize={`large`}
                    />
                </IconButton>
            </HorizontalActionBar>

            {/*TODO: only MP/VP magacini should be visible in dropdown depending on type*/}
            <MagaciniDropdown />
            <OtpremniceTable
                data={data}
                magacini={zMagacini}
                pagination={pagination}
                setPagination={setPagination}
            />
        </Stack>
    )
}
