import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar"
import { toast } from "react-toastify"
import { IPorudzbinaActionBarProps } from "../models/IPorudzbinaActionBarProps"
import { LinearProgress } from "@mui/material"
import { ApiBase, ContentType, fetchApi } from "@/app/api"

export const PorudzbinaActionBar = (props: IPorudzbinaActionBarProps): JSX.Element => {
    return (
        props.porudzbina == null ?
            <LinearProgress /> :
            props.porudzbina.referent == null ?
                <HorizontalActionBar>
                    <HorizontalActionBarButton
                    isDisabled={props.isDisabled}
                    onClick={() => {
                        props.onPreuzmiNaObraduStart()
                        fetchApi(ApiBase.Main, `/orders/${props.porudzbina?.oneTimeHash}/occupy-referent`, {
                            method: `PUT`
                        })
                        .then((r) => {
                            toast.success(`Preuzeto na obradu`)
                            props.onPreuzmiNaObraduEnd()
                        })
                    }} text={`Preuzmi na obradu`} />
                </HorizontalActionBar> :
                <HorizontalActionBar>
                    {
                        props.porudzbina.komercijalnoBrDok != null  ? null :
                            <HorizontalActionBarButton
                            isDisabled={props.isDisabled}
                            onClick={() => {
                                props.onPretvoriUProracunStart()
                                fetchApi(ApiBase.Main, `/orders/${props.porudzbina?.oneTimeHash}/forward-to-komercijalno`, {
                                    method: `POST`,
                                    body: {
                                        oneTimeHash: props.porudzbina.oneTimeHash,
                                        isPonuda: false
                                    },
                                })
                                .then((r: number) => {
                                    props.onPretvoriUProracunSuccess()
                                    toast.success(`Porudžbina prebačena u komercijalno poslovanje!`)
                                })
                                .catch(() => {
                                    props.onPretvoriUProracunFail()
                                })
                            }} text={`Pretvori u proračun`} />
                    }
                    {
                        props.porudzbina.komercijalnoBrDok != null  ? null :
                            <HorizontalActionBarButton
                            isDisabled={props.isDisabled}
                            onClick={() => {
                                props.onPretvoriUProracunStart()
                                fetchApi(ApiBase.Main, `/orders/${props.porudzbina?.oneTimeHash}/forward-to-komercijalno`, {
                                    method: `POST`,
                                    body: {
                                        oneTimeHash: props.porudzbina.oneTimeHash,
                                        isPonuda: true
                                    },
                                    contentType: ContentType.ApplicationJson
                                })
                                .then((r: number) => {
                                    props.onPretvoriUProracunSuccess()
                                    toast.success(`Porudžbina prebačena u komercijalno poslovanje!`)
                                })
                                .catch(() => {
                                    props.onPretvoriUProracunFail()
                                })
                            }} text={`Pretvori u ponudu`} />
                    }
                    {
                        props.porudzbina.komercijalnoBrDok != null  ? null :
                            <HorizontalActionBarButton
                            isDisabled={props.isDisabled}
                            onClick={() => {
                                toast.warning(`Not implemented yet`)
                            }} text={`Storniraj porudžbinu`} />
                    }
                    {
                        props.porudzbina.komercijalnoBrDok == null ? null :
                            <HorizontalActionBarButton
                            isDisabled={props.isDisabled}
                            onClick={() => {
                                props.onRazveziOdProracunaStart()
                                fetchApi(ApiBase.Main, `/orders/${props.porudzbina?.oneTimeHash}/unlink-from-komercijalno`, {
                                    method: `POST`
                                })
                                .then((r: number) => {
                                    props.onRazveziOdProracunaEnd()
                                })
                                .catch(() => {
                                    props.onRazveziOdProracunaEnd()
                                })
                            }} text={`Razveži od komercijalnog`} />
                    }
                </HorizontalActionBar>
    )
}