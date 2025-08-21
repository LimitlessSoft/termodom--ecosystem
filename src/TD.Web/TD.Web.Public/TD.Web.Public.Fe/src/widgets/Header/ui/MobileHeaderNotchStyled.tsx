import { Grid, styled } from '@mui/material'
import { User } from '@/features/userSlice/userSlice'

export const MobileHeaderNotchStyled = styled(Grid)<{ user: User }>(
    ({ theme, user }) => `
        background-color: var(--td-red);
        border-bottom: ${user.isLogged ? `15px solid #fa0` : ``};
        top: 0;
        left: 0;
        padding: ${theme.spacing(0.75)};
        z-index: 10000;
        
        span {
            display: block;
            width: 50px;
            border-radius: 4px;
            height: 10px;
            margin: 5px;
            background-color: #222;
        }
    `
)
