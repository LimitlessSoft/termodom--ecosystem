export interface IKorisniciSelectFilterData {
    filteredCities: number[]
    filteredProfessions: number[]
    filteredStatuses: number[]
    filteredStores: number[]
    filteredTypes: number[]
}

export interface IKorisniciFilterData extends IKorisniciSelectFilterData {
    search: string
}
