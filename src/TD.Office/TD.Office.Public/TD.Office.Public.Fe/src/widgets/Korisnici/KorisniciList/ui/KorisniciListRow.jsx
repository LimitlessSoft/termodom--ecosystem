import { useRouter } from 'next/router'
import { KorisniciListRowStyled } from '../styled/KorisniciListRowStyled'
import { TableCell } from '@mui/material'

export const KorisniciListRow = (props) => {
    const router = useRouter()

    return (
        <KorisniciListRowStyled
            onClick={() => {
                router.push(`/korisnici/${props.korisnik.id}`)
            }}
        >
            <TableCell>{props.korisnik.id}</TableCell>
            <TableCell>{props.korisnik.nickname}</TableCell>
            <TableCell>{props.korisnik.username}</TableCell>
        </KorisniciListRowStyled>
    )
}
