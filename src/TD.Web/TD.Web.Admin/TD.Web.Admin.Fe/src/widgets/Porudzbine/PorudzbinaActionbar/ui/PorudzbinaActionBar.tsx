import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar"
import { toast } from "react-toastify"

export const PorudzbinaActionBar = (): JSX.Element => {
    return (
        <HorizontalActionBar>
            <HorizontalActionBarButton onClick={() => {
                toast.warning(`Not implemented yet`)
            }} text={`Preuzmi na obradu`} />
            <HorizontalActionBarButton onClick={() => {
                toast.warning(`Not implemented yet`)
            }} text={`Pretvori u proračun`} />
            <HorizontalActionBarButton onClick={() => {
                toast.warning(`Not implemented yet`)
            }} text={`Pretvori u ponudu`} />
            <HorizontalActionBarButton onClick={() => {
                toast.warning(`Not implemented yet`)
            }} text={`Razveži od proračuna`} />
        </HorizontalActionBar>
    )
}