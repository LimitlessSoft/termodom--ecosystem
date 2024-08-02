import { IKorisniciFilterSearchProps } from '../interfaces/IKorisniciFilterSearchProps'
import { TextField } from '@mui/material'

export const KorisniciFilterSearch = ({
    onSearchUsers,
}: IKorisniciFilterSearchProps) => (
    <TextField
        label={`Pretraga korisnika`}
        variant={`outlined`}
        onChange={(e) => {
            onSearchUsers(e.target.value)
        }}
    />
)
