import { Grid } from '@mui/material'
import { SpecifikacijaNovcaTopBarButton } from './SpecifikacijaNovcaTopBarButton'
import { ArrowBackIos, ArrowForwardIos, Help, Print } from '@mui/icons-material'
import { ISpecifikacijaNovcaHelperActionsProps } from '../interfaces/ISpecifikacijaNovcaHelperActionsProps'
import { EnchantedTextField } from '@/widgets'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { PERMISSIONS_CONSTANTS } from '@/constants'
import dayjs from 'dayjs'

export const SpecifikacijaNovcaHelperActions = ({
    onStoreButtonClick,
    isStoreButtonSelected,
    permissions,
    date,
}: ISpecifikacijaNovcaHelperActionsProps) => {
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
                        text={`Help`}
                        startIcon={<Help />}
                    />
                </Grid>
                <Grid item>
                    <SpecifikacijaNovcaTopBarButton
                        text={`Stampa`}
                        startIcon={<Print />}
                        disabled={
                            !hasPermission(
                                permissions,
                                PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                    .SPECIFIKACIJA_NOVCA.PRINT
                            )
                        }
                    />
                </Grid>
                <Grid item sm={1}></Grid>
                <Grid item>
                    <Grid container spacing={1}>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton
                                disabled={
                                    noDatePermissions ||
                                    (date.isBefore(
                                        dayjs().subtract(7, 'days')
                                    ) &&
                                        onlyPreviousWeekEnabled)
                                }
                            >
                                <ArrowBackIos
                                    style={{ transform: 'translateX(4px)' }}
                                />
                            </SpecifikacijaNovcaTopBarButton>
                        </Grid>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton
                                onClick={onStoreButtonClick}
                                isToggled={isStoreButtonSelected}
                                text={`M`}
                                typographySx={{
                                    fontWeight: `bold`,
                                }}
                                disabled={noDatePermissions}
                            />
                        </Grid>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton
                                disabled={
                                    noDatePermissions ||
                                    date.isSame(dayjs(), 'day')
                                }
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
