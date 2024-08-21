export interface ICreateProductDetails {
    name: string
    src: string
    image: string
    unitId: number
    alternateUnitId?: number
    shortDescription: string
    description: string
    oneAlternatePackageEquals?: number
    catalogId: string
    classification: number
    vat: number
    productPriceGroupId: number
    stockType: number
}
