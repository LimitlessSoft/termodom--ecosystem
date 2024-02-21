import { IPorudzbina } from "../../models/IPorudzbina";

export interface IPorudzbinaHeaderProps {
    porudzbina: IPorudzbina,
    isDisabled: boolean,
    isTDNumberUpdating: boolean
}