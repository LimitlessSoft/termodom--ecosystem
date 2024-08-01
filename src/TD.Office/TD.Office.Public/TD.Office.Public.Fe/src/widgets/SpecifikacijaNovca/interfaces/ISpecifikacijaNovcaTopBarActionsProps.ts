import { IStoreDto } from '@/dtos/stores/IStoreDto'
import { Dayjs } from 'dayjs'

export interface ISpecifikacijaNovcaTopBarActionsProps {
    stores: IStoreDto[] | undefined
    currentStore: IStoreDto | undefined
    date: Dayjs
    currentSpecificationNumber: number
    onChangeDate: (newDate: Dayjs) => void
    onChangeStore: (store: IStoreDto | undefined) => void
}
