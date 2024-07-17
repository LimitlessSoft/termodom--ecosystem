import { DataDto } from './DataDto'

export interface IAzuriranjeCenaPovezanCellProps {
    data: DataDto
    disabled: boolean
    onSuccessUpdate: () => void
    onErrorUpdate: () => void
}
