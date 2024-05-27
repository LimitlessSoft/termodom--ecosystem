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
                    isDisabled={props.isDisabled || props.porudzbina.statusId == 5}
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
                            isDisabled={props.isDisabled || props.porudzbina.statusId == 5}
                            onClick={() => {
                                if(props.porudzbina.storeId == -5) {
                                    toast.error(`Morate izabrati validan magacin!`)
                                    return
                                }
                                props.onPretvoriUProracunStart()
                                fetchApi(ApiBase.Main, `/orders/${props.porudzbina?.oneTimeHash}/forward-to-komercijalno`, {
                                    method: `POST`,
                                    body: {
                                        oneTimeHash: props.porudzbina.oneTimeHash,
                                        isPonuda: false
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
                            }} text={`Pretvori u proračun`} />
                    }
                    {
                        props.porudzbina.komercijalnoBrDok != null  ? null :
                            <HorizontalActionBarButton
                            isDisabled={props.isDisabled || props.porudzbina.statusId == 5}
                            onClick={() => {
                                if(props.porudzbina.storeId == -5) {
                                    toast.error(`Morate izabrati validan magacin!`)
                                    return
                                }
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
                            isDisabled={props.isDisabled || props.porudzbina.statusId == 5}
                            onClick={() => {
                                props.onStornirajStart()
                                fetchApi(ApiBase.Main, `/orders/${props.porudzbina?.oneTimeHash}/status/5`, {
                                    method: `PUT`,
                                })
                                .then(() => {
                                    toast.success(`Porudžbina stornirana!`)
                                    props.onStornirajSuccess()
                                }).catch(() => {
                                    props.onStornirajFail()
                                })
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