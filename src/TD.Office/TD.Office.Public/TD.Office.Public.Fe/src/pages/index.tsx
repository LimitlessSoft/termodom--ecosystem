import { useUser } from "@/app/hooks"
import { Box } from "@mui/material"

const Home = (): JSX.Element => {

    const user = useUser()

    return (
        <Box>
            home
        </Box>
    )
}

export default Home