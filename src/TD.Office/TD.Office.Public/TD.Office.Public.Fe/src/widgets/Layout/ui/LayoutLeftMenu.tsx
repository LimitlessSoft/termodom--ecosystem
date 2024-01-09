import { Box, Grid, styled } from "@mui/material";
import { LayoutLeftMenuButton } from "./LayoutLeftMenuButton";
import { Home, Language, Logout } from "@mui/icons-material";
import { useRouter } from "next/router";
import useCookie from 'react-use-cookie'
import { useAppDispatch } from "@/app/hooks";
import { fetchMe } from "@/features/slices/userSlice/userSlice";

interface ILayoutLeftMenuProps {
    fixed?: boolean
}

export const LayoutLeftMenu = (props: ILayoutLeftMenuProps): JSX.Element => {

    const { fixed } = props
    const router = useRouter()
    const dispatch = useAppDispatch()
    const [userToken, setUserToken] = useCookie('token', undefined)

    const LayoutLeftMenuStyled = styled(Grid)(
        ({ theme }) => `
            background-color: ${theme.palette.primary.main};
            color: ${theme.palette.primary.contrastText};
            height: 100vh;
            ${fixed ? 'position: fixed; top: 0; left: 0;' : null}
            ${!fixed ? 'opacity: 0;' : null}
        `
    )

    return (
        <LayoutLeftMenuStyled>
            <Grid container
                direction={`column`}>
                <LayoutLeftMenuButton onClick={() => {
                    router.push('/')
                }}>
                    <Home />
                </LayoutLeftMenuButton>
                <LayoutLeftMenuButton onClick={() => {
                    router.push('/web-prodavnica')
                }}>
                    <Language />
                </LayoutLeftMenuButton>
                <LayoutLeftMenuButton onClick={() => {
                        setUserToken('')
                        dispatch(fetchMe())
                    }} >
                    <Logout />
                </LayoutLeftMenuButton>
            </Grid>
        </LayoutLeftMenuStyled>
    )
}