import { useRouter } from 'next/router'
import { TableCell, TableRow } from '@mui/material'

export const KorisniciListRow = (props) => {
    const router = useRouter()
    const { korisnik } = props

    // Use user type color for row background, with lighter opacity
    const rowColor = korisnik.tipKorisnikaBoja
        ? `${korisnik.tipKorisnikaBoja}20`  // 20 = 12% opacity in hex
        : undefined

    return (
        <TableRow
            onClick={() => {
                router.push(`/korisnici/lista/${korisnik.id}`)
            }}
            sx={{
                backgroundColor: rowColor,
                '&:hover': {
                    backgroundColor: korisnik.tipKorisnikaBoja
                        ? `${korisnik.tipKorisnikaBoja}40`  // Darker on hover
                        : 'action.hover',
                    cursor: 'pointer',
                },
            }}
        >
            <TableCell>{korisnik.id}</TableCell>
            <TableCell>{korisnik.nickname}</TableCell>
            <TableCell>{korisnik.username}</TableCell>
            <TableCell>{korisnik.tipKorisnikaNaziv || '-'}</TableCell>
        </TableRow>
    )
}
