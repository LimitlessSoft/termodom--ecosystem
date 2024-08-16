import { IPorudzbina } from '../../models/IPorudzbina'
import { IStockType } from '../../PorudzbinaItemRow/interfaces/IStockType'

export interface IPorudzbinaSummaryProps {
    porudzbina: IPorudzbina
    stockTypes: IStockType[]
    isDelivery: boolean
}
