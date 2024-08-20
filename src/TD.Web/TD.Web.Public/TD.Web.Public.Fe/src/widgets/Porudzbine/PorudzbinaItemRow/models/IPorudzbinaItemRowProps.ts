import { IPorudzbinaItem } from '../../interfaces/IPorudzbinaItem'
import { IStockType } from '../interfaces/IStockType'

export interface IPorudzbinaItemRowProps {
    item: IPorudzbinaItem
    stockTypes: IStockType[]
    isDelivery: boolean
}
