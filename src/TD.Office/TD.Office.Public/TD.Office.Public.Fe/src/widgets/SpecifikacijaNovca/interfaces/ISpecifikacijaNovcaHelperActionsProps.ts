import { IPermissionDto } from '@/dtos/permissions/IPermissionDto'
import { Dayjs } from 'dayjs'

export interface ISpecifikacijaNovcaHelperActionsProps {
    onStoreButtonClick: () => void
    isStoreButtonSelected: boolean
    permissions: IPermissionDto[]
    date: Dayjs
}
