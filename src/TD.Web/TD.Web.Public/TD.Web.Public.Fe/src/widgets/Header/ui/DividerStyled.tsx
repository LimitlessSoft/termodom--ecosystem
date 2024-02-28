import { User } from "@/features/userSlice/userSlice"
import { Typography, styled } from "@mui/material"

export const DividerStyled = styled(Typography)<{ user: User }>
    (({ theme, user }) => (
    `
        flex-grow: 1;
        text-decoration: none;
        padding-top: 20px;
        padding-bottom: 20px;
        padding-left: 15px;
        padding-right: 15px;
        color: ${user.isLogged ? theme.palette.secondary.contrastText : theme.palette.primary.contrastText};

        @media only
        screen and (max-width: 260px),
        screen and (max-width: 360px),
        screen and (max-width: 520px),
        screen and (max-width: 720px),
        {
            flex-grow: initial;
        }
    `
    ))