import { Grid } from '@mui/material'
import { SpecifikacijaNovcaTopBarButton } from './SpecifikacijaNovcaTopBarButton'
import { ArrowBackIos, ArrowForwardIos, Help, Print } from '@mui/icons-material'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { PERMISSIONS_CONSTANTS } from '@/constants'
import dayjs from 'dayjs'
import { toast } from 'react-toastify'
import { useState } from 'react'
import { encryptSpecifikacijaNovcaId } from '../helpers/SpecifikacijaHelpers'

export const SpecifikacijaNovcaHelperActions = ({
    onPreviousClick,
    onNextClick,
    permissions,
    date,
    disabled,
    specifikacijaId,
}) => {
    const [isMagacinFixed, setIsMagacinFixed] = useState(false)
    const onlyPreviousWeekEnabled = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.SPECIFIKACIJA_NOVCA.PREVIOUS_WEEK
    )

    const noDatePermissions =
        !hasPermission(
            permissions,
            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.SPECIFIKACIJA_NOVCA.ALL_DATES
        ) && !onlyPreviousWeekEnabled

    return (
        <Grid item xs={12}>
            <Grid container justifyContent={`end`} gap={2}>
                <Grid item>
                    <SpecifikacijaNovcaTopBarButton
                        text={`Stampa`}
                        startIcon={<Print />}
                        disabled={
                            disabled ||
                            !hasPermission(
                                permissions,
                                PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                    .SPECIFIKACIJA_NOVCA.PRINT
                            )
                        }
                        href={`/print/specifikacija-novca/${encryptSpecifikacijaNovcaId(specifikacijaId)}?noLayout=true`}
                        target={`_blank`}
                    />
                </Grid>
                <Grid item sm={1}></Grid>
                <Grid item>
                    <Grid container spacing={1}>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton
                                disabled={
                                    disabled ||
                                    noDatePermissions ||
                                    (date.isBefore(
                                        dayjs().subtract(7, 'days')
                                    ) &&
                                        onlyPreviousWeekEnabled)
                                }
                                onClick={() => {
                                    onPreviousClick(isMagacinFixed)
                                }}
                            >
                                <ArrowBackIos
                                    style={{ transform: 'translateX(4px)' }}
                                />
                            </SpecifikacijaNovcaTopBarButton>
                        </Grid>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton
                                onClick={() => {
                                    setIsMagacinFixed((prev) => !prev)
                                }}
                                isToggled={isMagacinFixed}
                                text={`M`}
                                typographySx={{
                                    fontWeight: `bold`,
                                }}
                                disabled={noDatePermissions || disabled}
                            />
                        </Grid>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton
                                disabled={
                                    disabled ||
                                    noDatePermissions ||
                                    date.isSame(dayjs(), 'day')
                                }
                                onClick={() => {
                                    onNextClick(isMagacinFixed)
                                }}
                            >
                                <ArrowForwardIos />
                            </SpecifikacijaNovcaTopBarButton>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}
