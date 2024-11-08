import { IPermissionDto } from '@/dtos/permissions/IPermissionDto'
import { IStoreDto } from '@/dtos/stores/IStoreDto'
import { Dayjs } from 'dayjs'

export interface ISpecifikacijaNovcaTopBarActionsProps {
    permissions: IPermissionDto[]
    stores: IStoreDto[]
    currentStore: IStoreDto | undefined
    date: Dayjs
    currentSpecificationNumber: number
    onChangeDate: (newDate: Dayjs) => void
    onChangeStore: (store: IStoreDto | undefined) => void
}
