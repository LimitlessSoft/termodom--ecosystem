import { IDividerProps } from '../models/IDividerProps'
import { Typography } from '@mui/material'
import { DividerStyled } from './DividerStyled'

export const Divider = (props: IDividerProps): JSX.Element => {
    return (
        <>
            {props.user.isLogged && (
                <DividerStyled user={props.user}>
                    <Typography
                        component={`span`}
                        style={{
                            fontFamily: 'GothamProMedium',
                        }}
                    >
                        {props.user.data?.nickname}
                    </Typography>
                </DividerStyled>
            )}
        </>
    )
}
