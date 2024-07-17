import { DataDto } from './DataDto'

export interface IAzuriranjeCenaUslovFormiranjaCellProps {
    data: DataDto
    disabled: boolean
    onSuccessUpdate: () => void
    onErrorUpdate: () => void
}
