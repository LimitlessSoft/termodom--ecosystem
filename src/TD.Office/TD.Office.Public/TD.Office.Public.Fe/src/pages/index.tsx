import { Box, CircularProgress } from "@mui/material"
import {useUser} from "@/hooks/useUserHook"

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