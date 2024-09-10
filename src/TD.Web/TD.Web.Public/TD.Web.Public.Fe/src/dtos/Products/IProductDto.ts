type oneTimePrice = {
    minPrice: number
    maxPrice: number
}

type userPrice = {
    priceWithoutVAT: number
    vat: number
    priceWithVAT: number
}

export interface IProductDto {
    id: number
    src: string
    title: string
    vat: number
    unit: string
    imageContentType: string
    imageData: string
    classification: number
    userPrice?: userPrice
    oneTimePrice?: oneTimePrice
    priorityIndex: number
    metaTitle?: string
    metaDescription?: string
    isWholesale: boolean
    stockType: number
}
