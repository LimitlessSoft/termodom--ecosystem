import { ILayoutLeftMenuProps } from "../interfaces/ILayoutLeftMenuProps";
import { Home, Language, LocalAtm, LocalShipping, Logout, Person } from "@mui/icons-material";
import { fetchMe } from "@/features/slices/userSlice/userSlice";
import { LayoutLeftMenuButton } from "./LayoutLeftMenuButton";
import { Box, Grid, styled } from "@mui/material";
import { useAppDispatch } from "@/app/hooks";
import useCookie from 'react-use-cookie'
import { useRouter } from "next/router";

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
                }}> <Home /> </LayoutLeftMenuButton>

                <LayoutLeftMenuButton onClick={() => {
                    router.push('/nalog-za-prevoz')
                }}> <LocalShipping /> </LayoutLeftMenuButton>

                <LayoutLeftMenuButton onClick={() => {
                    router.push('/specifikacija-novca')
                }}> <LocalAtm /> </LayoutLeftMenuButton>

                <LayoutLeftMenuButton onClick={() => {
                    router.push('/web-prodavnica')
                }}> <Language /> </LayoutLeftMenuButton>

                <LayoutLeftMenuButton onClick={() => {
                    router.push('/korisnici')
                }}> <Person /> </LayoutLeftMenuButton>

                <LayoutLeftMenuButton onClick={() => {
                    setUserToken('')
                    dispatch(fetchMe())
                }}> <Logout /> </LayoutLeftMenuButton>

            </Grid>
        </LayoutLeftMenuStyled>
    )
}