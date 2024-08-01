import { ISpecificationSpecifikacijaNovcaNovcanicaDto } from '@/dtos/specifications/ISpecificationSpecifikacijaNovcaNovcanicaDto'

export interface ISpecifikacijaNovcaGotovinaProps {
    gotovina: ISpecificationSpecifikacijaNovcaNovcanicaDto[]
    ukupnoGotovine: number
    onChange: (note: number, value: number) => void
}
