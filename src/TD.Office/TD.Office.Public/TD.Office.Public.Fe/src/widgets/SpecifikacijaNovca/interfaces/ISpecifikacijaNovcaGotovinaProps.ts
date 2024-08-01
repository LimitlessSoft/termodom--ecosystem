import { ISpecificationDto } from '@/dtos/specifications/ISpecificationDto'

export interface ISpecifikacijaNovcaGotovinaProps {
    specifikacija: ISpecificationDto
    onChange: (note: number, value: number) => void
}
