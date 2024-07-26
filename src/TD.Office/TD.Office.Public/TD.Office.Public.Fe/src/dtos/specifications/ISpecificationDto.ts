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
        kartice: {
            vrednost: number
            komentar: string
        }
        cekovi: {
            vrednost: number
            komentar: string
        }
        papiri: {
            vrednost: number
            komentar: string
        }
        troskovi: {
            vrednost: number
            komentar: string
        }
        vozaci: {
            vrednost: number
            komentar: string
        }
        sasa: {
            vrednost: number
            komentar: string
        }
    }
    komentar: string
}
