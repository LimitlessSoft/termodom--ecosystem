import { useRouter } from 'next/router'
import { IKorisniciListRowProps } from '../interfaces/IKorisniciListRowProps'
import { KorisniciListRowStyled } from '../styled/KorisniciListRowStyled'
import { TableCell } from '@mui/material'

export const KorisniciListRow = (props: IKorisniciListRowProps) => {
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
