import { IKorisniciSearchFilterProps } from "../interfaces/IKorisniciSearchFilterProps"
import { TextField } from "@mui/material"

export const KorisniciSearchFilter = ({onSearchUsers}: IKorisniciSearchFilterProps) =>
    <TextField
        label={`Pretraga korisnika`}
        variant={`outlined`}
        onChange={(e) => { onSearchUsers(e.target.value) }} />