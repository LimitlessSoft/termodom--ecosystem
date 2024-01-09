import { useUser } from "@/app/hooks"
import { Box, CircularProgress } from "@mui/material"

const Home = (): JSX.Element => {

    const user = useUser()

    return (
        user?.isLogged == null || user.isLogged == false ?
            <CircularProgress /> :
            <Box>
                home
            </Box>
    )
}

export default Home