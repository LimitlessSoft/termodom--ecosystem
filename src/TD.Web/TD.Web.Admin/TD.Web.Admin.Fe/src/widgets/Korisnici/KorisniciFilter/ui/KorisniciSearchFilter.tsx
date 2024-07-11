import { TextField } from "@mui/material"
import { IKorisniciSearchFilterProps } from "../interfaces/IKorisniciSearchFilterProps"
import { ChangeEvent } from "react"

export const KorisniciSearchFilter = ({onSearchUsers}: IKorisniciSearchFilterProps) => {
  const onChangeSearchTermHandler = (event:ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    onSearchUsers(event.target.value)
  }

  return (
    <TextField id={`outlined-basic`} label={`Pretraga korisnika`} variant={`outlined`} onChange={onChangeSearchTermHandler}/>
  )

}
