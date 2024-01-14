import { DataDto } from "./DataDto";

export interface IAzuriranjeCenaTableRowProps {
    data: DataDto,
    reloadData: () => void
}