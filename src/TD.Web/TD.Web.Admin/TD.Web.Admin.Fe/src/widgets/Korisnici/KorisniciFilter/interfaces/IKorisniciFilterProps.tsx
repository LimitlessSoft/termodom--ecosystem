import { IKorisniciFilterData } from "./IKorisniciFilterData";

export interface IKorisniciFilterProps {
    onFilterChange: (filterData: IKorisniciFilterData) => void
}