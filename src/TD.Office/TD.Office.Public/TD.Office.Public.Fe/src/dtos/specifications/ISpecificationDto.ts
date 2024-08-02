import { ISpecificationOstaloDto } from './ISpecificationOstaloDto'
import { ISpecificationPoreskaDto } from './ISpecificationPoreskaDto'
import { ISpecificationRacunarDto } from './ISpecificationRacunarDto'
import { ISpecificationSpecifikacijaNovca } from './ISpecificationSpecifikacijaNovcaDto'

export interface ISpecificationDto {
    id: number
    magacinId: number
    datumUTC: string
    racunar: ISpecificationRacunarDto
    poreska: ISpecificationPoreskaDto
    specifikacijaNovca: ISpecificationSpecifikacijaNovca
    komentar: string
    racunarTrazi: {
        value: number
        label: string
    }
}
