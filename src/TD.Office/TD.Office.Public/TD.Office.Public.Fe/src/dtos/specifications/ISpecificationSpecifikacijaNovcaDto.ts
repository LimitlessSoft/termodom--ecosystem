import { ISpecificationOstaloDto } from './ISpecificationOstaloDto'
import { ISpecificationSpecifikacijaNovcaNovcanicaDto } from './ISpecificationSpecifikacijaNovcaNovcanicaDto'

export interface ISpecificationSpecifikacijaNovca {
    eur1: {
        komada: number
        kurs: number
    }
    eur2: {
        komada: number
        kurs: number
    }
    novcanice: ISpecificationSpecifikacijaNovcaNovcanicaDto[]
    ostalo: ISpecificationOstaloDto[]
}
