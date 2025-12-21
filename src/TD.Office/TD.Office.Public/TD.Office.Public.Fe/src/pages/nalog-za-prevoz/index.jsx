import { NalogZaPrevozWrapper } from '@/widgets'
import { CircularProgress } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'

const NalogZaPrevoz = () => {
    const user = useUser(true)

    return user.isLoading || user.isLogged === false ? (
        <CircularProgress />
    ) : (
        <NalogZaPrevozWrapper />
    )
}

export default NalogZaPrevoz
