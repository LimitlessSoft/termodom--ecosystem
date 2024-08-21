export interface IEditProductDetails {
    name: string
    src: string
    image: string
    id: number
    unitId: number
    stockType: number
    status: number
    productPriceGroupId: number
    priorityIndex: number
    oneAlternatePackageEquals: number
    alternateUnitId: number
    shortDescription: string
    description: string
    catalogId: string
    classification: number
    minWebBase: number
    maxWebBase: number
    vat: number
    metaTitle: string
    metaDescription: string
    groups: number[]
    canEdit: boolean
}
