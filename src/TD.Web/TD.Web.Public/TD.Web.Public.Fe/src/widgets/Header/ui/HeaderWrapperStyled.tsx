import { User } from "@/features/userSlice/userSlice";
import { Stack, styled } from "@mui/material";

export const HeaderWrapperStyled = styled(Stack)<{ user: User }>(
    ({ theme, user }) => `
        flex-direction: row;
        align-items: center;
        padding-left: 10px;
        padding-right: 10px;
        transition-duration: 0.5s;
        border-bottom: ${ user.isLogged ? `15px solid #fa0` : ``};

        @media only
            screen and (max-width: 260px),
            screen and (max-width: 360px),
            screen and (max-width: 520px),
            screen and (max-width: 720px),
        {
            transform: translateX(-100%);
            flex-direction: column;
            min-height: 100vh;
            z-index: 1000;
            width: 100vw;
            top: 0px;
            padding: 0;
            left: 0;
            position: fixed;
            padding-top: 10vh;
            padding-bottom: 20px;
            background-color: var(--td-red);
            border-bottom: none;
        }
    `)