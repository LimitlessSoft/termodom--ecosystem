import { useUser } from "@/app/hooks"
import { NalogZaPrevozWrapper } from "@/widgets"
import { CircularProgress } from "@mui/material"

const NalogZaPrevoz = (): JSX.Element => {

    const user = useUser(true)

    return user.isLoading || user.isLogged === false
        ? <CircularProgress />
        : <NalogZaPrevozWrapper />
}

export default NalogZaPrevoz