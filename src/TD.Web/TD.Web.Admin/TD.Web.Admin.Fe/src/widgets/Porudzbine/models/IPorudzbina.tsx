import { IPorudzbinaItem } from "./IPorudzbinaItem";

export interface IPorudzbina {
    oneTimeHash: string,
    checkedOutAt: Date,
    status: string,
    storeId: number,
    statusId: number,
    userInformation: any,
    summary: any,
    referent: string,
    note: string,
    mobile: string,
    paymentTypeId: number,
    name: string,
    komercijalnoBrDok?: number,
    komercijalnoVrDok?: number,
    items: IPorudzbinaItem[]
}