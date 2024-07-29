import { ISpecificationOstaloDto } from './ISpecificationOstaloDto'

export interface ISpecificationDto {
    magacinId: number
    datumUTC: string
    racunar: {
        gotovinskiRacuni: string
        virmanskiRacuni: string
        kartice: string
        ukupnoRacunar: string
        gotovinskePovratnice: string
        virmanskePovratnice: string
        ostalePovratnice: string
    }
    poreska: {
        fiskalizovaniRacuni: string
        fiskalizovanePovratnice: string
    }
    specifikacijaNovca: {
        eur1: {
            komada: number
            kurs: number
        }
        eur2: {
            komada: number
            kurs: number
        }
        novcanice: {
            key: number
            value: number
        }[]
        ostalo: ISpecificationOstaloDto[]
    }
    komentar: string,
    racunarTrazi: {
        value: number,
        label: string
    }
}
