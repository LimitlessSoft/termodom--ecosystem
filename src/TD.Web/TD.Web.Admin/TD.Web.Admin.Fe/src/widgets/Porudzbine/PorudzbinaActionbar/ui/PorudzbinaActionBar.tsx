import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar"
import { toast } from "react-toastify"
import { IPorudzbinaActionBarProps } from "../models/IPorudzbinaActionBarProps"
import { LinearProgress } from "@mui/material"

export const PorudzbinaActionBar = (props: IPorudzbinaActionBarProps): JSX.Element => {
    return (
        props.porudzbina == null ?
            <LinearProgress /> :
            props.porudzbina.referent == null ?
                <HorizontalActionBar>
                    <HorizontalActionBarButton onClick={() => {
                        toast.warning(`Not implemented yet`)
                    }} text={`Preuzmi na obradu`} />
                </HorizontalActionBar> :
                <HorizontalActionBar>
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