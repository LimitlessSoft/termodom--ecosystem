import { ISpecificationOstaloDto } from '@/dtos/specifications/ISpecificationOstaloDto'

export interface ISpecifikacijaNovcaOstalo {
    ostalo: ISpecificationOstaloDto[]
    onChange: (key: string, value: number) => void
}
