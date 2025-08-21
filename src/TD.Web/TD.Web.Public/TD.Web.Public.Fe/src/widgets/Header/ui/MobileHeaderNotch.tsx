import { IClickable } from '@/interfaces/IClickable'
import { MobileHeaderNotchStyled } from './MobileHeaderNotchStyled'
import { Grid, Typography } from '@mui/material'
import { useAppSelector } from '@/app/hooks'
import { selectUser } from '@/features/userSlice/userSlice'

export const MobileHeaderNotch = (props: IClickable): JSX.Element => {
    const user = useAppSelector(selectUser)
    return (
        <MobileHeaderNotchStyled
            user={user}
            display={{
                xs: `block`,
                md: `none`,
            }}
            onClick={() => {
                props.onClick()
            }}
        >
            <Grid container spacing={1} alignItems={`center`}>
                <Grid item>
                    <span></span>
                    <span></span>
                    <span></span>
                </Grid>
                <Grid item>
                    <Typography
                        fontSize={{
                            xs: `32px`,
                        }}
                        component={`p`}
                        variant={`subtitle1`}
                    >
                        Meni
                    </Typography>
                </Grid>
            </Grid>
        </MobileHeaderNotchStyled>
    )
}
