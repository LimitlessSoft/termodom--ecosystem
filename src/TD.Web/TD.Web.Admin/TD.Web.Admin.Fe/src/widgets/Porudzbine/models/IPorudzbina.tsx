export interface IPorudzbina {
    oneTimeHash: string,
    createdAt: Date,
    status: string,
    user: string,
    valueWithVAT: number,
    discountValue: number
}