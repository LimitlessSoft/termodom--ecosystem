import { Grid } from '@mui/material'
import { SpecifikacijaNovcaTopBarButton } from './SpecifikacijaNovcaTopBarButton'
import { ArrowBackIos, ArrowForwardIos, Help, Print } from '@mui/icons-material'
import { ISpecifikacijaNovcaHelperActionsProps } from '../interfaces/ISpecifikacijaNovcaHelperActionsProps'

export const SpecifikacijaNovcaHelperActions = ({
    onStoreButtonClick,
    isStoreButtonSelected,
}: ISpecifikacijaNovcaHelperActionsProps) => {
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
                    />
                </Grid>
                <Grid item sm={1}></Grid>
                <Grid item>
                    <Grid container spacing={1}>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton>
                                <ArrowBackIos
                                    style={{ transform: 'translateX(4px)' }}
                                />
                            </SpecifikacijaNovcaTopBarButton>
                        </Grid>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton
                                onClick={onStoreButtonClick}
                                isSelected={isStoreButtonSelected}
                                text={`M`}
                                typographySx={{
                                    fontWeight: `bold`,
                                }}
                            />
                        </Grid>
                        <Grid item>
                            <SpecifikacijaNovcaTopBarButton>
                                <ArrowForwardIos />
                            </SpecifikacijaNovcaTopBarButton>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}
