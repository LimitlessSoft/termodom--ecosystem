import { IPorudzbinaItem } from "./IPorudzbinaItem";

export interface IPorudzbina {
    oneTimeHash: string,
    createdAt: Date,
    status: string,
    user: string,
    valueWithVAT: number,
    discountValue: number,
    referent: string,
    note: string,
    mobile: string,
    name: string,
    items: IPorudzbinaItem[]
}