import { IPorudzbinaItem } from '../../interfaces/IPorudzbinaItem'
import { IStockType } from '../../PorudzbinaItemRow/interfaces/IStockType'

export interface IPorudzbinaItemsProps {
    items: IPorudzbinaItem[]
    stockTypes: IStockType[]
    isDelivery: boolean
}
