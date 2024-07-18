import {
    HorizontalActionBar,
    HorizontalActionBarButton,
} from '@/widgets/TopActionBar'
import { toast } from 'react-toastify'
import { IPorudzbinaActionBarProps } from '../models/IPorudzbinaActionBarProps'
import { LinearProgress } from '@mui/material'
import { adminApi } from '@/apis/adminApi'

export const PorudzbinaActionBar = (
    props: IPorudzbinaActionBarProps
): JSX.Element => {
    return props.porudzbina == null ? (
        <LinearProgress />
    ) : props.porudzbina.referent == null ? (
        <HorizontalActionBar>
            <HorizontalActionBarButton
                isDisabled={props.isDisabled || props.porudzbina.statusId == 5}
                onClick={() => {
                    props.onPreuzmiNaObraduStart()

                    adminApi
                        .put(
                            `/orders/${props.porudzbina?.oneTimeHash}/occupy-referent`
                        )
                        .then(() => {
                            toast.success(`Preuzeto na obradu`)
                        })
                        .finally(() => {
                            props.onPreuzmiNaObraduEnd()
                        })
                }}
                text={`Preuzmi na obradu`}
            />
        </HorizontalActionBar>
    ) : (
        <HorizontalActionBar>
            {props.porudzbina.komercijalnoBrDok != null ? null : (
                <HorizontalActionBarButton
                    isDisabled={
                        props.isDisabled || props.porudzbina.statusId == 5
                    }
                    onClick={() => {
                        if (props.porudzbina.storeId == -5) {
                            toast.error(`Morate izabrati validan magacin!`)
                            return
                        }
                        props.onPretvoriUProracunStart()

                        adminApi
                            .post(
                                `/orders/${props.porudzbina?.oneTimeHash}/forward-to-komercijalno`,
                                {
                                    oneTimeHash: props.porudzbina.oneTimeHash,
                                    isPonuda: false,
                                }
                            )
                            .then(() => {
                                props.onPretvoriUProracunSuccess()
                                toast.success(
                                    `Porudžbina prebačena u komercijalno poslovanje!`
                                )
                            })
                            .catch(() => {
                                props.onPretvoriUProracunFail()
                            })
                    }}
                    text={`Pretvori u proračun`}
                />
            )}
            {props.porudzbina.komercijalnoBrDok != null ? null : (
                <HorizontalActionBarButton
                    isDisabled={
                        props.isDisabled || props.porudzbina.statusId == 5
                    }
                    onClick={() => {
                        if (props.porudzbina.storeId == -5) {
                            toast.error(`Morate izabrati validan magacin!`)
                            return
                        }
                        props.onPretvoriUProracunStart()

                        adminApi
                            .post(
                                `/orders/${props.porudzbina?.oneTimeHash}/forward-to-komercijalno`,
                                {
                                    oneTimeHash: props.porudzbina.oneTimeHash,
                                    isPonuda: true,
                                }
                            )
                            .then(() => {
                                props.onPretvoriUProracunSuccess()
                                toast.success(
                                    `Porudžbina prebačena u komercijalno poslovanje!`
                                )
                            })
                            .catch(() => {
                                props.onPretvoriUProracunFail()
                            })
                    }}
                    text={`Pretvori u ponudu`}
                />
            )}
            {props.porudzbina.komercijalnoBrDok != null ? null : (
                <HorizontalActionBarButton
                    isDisabled={
                        props.isDisabled || props.porudzbina.statusId == 5
                    }
                    onClick={() => {
                        props.onStornirajStart()

                        adminApi
                            .put(
                                `/orders/${props.porudzbina?.oneTimeHash}/status/5`
                            )
                            .then(() => {
                                toast.success(`Porudžbina stornirana!`)
                                props.onStornirajSuccess()
                            })
                            .catch(() => {
                                props.onStornirajFail()
                            })
                    }}
                    text={`Storniraj porudžbinu`}
                />
            )}
            {props.porudzbina.komercijalnoBrDok == null ? null : (
                <HorizontalActionBarButton
                    isDisabled={props.isDisabled}
                    onClick={() => {
                        props.onRazveziOdProracunaStart()

                        adminApi
                            .post(
                                `/orders/${props.porudzbina?.oneTimeHash}/unlink-from-komercijalno`
                            )
                            .finally(() => {
                                props.onRazveziOdProracunaEnd()
                            })
                    }}
                    text={`Razveži od komercijalnog`}
                />
            )}
        </HorizontalActionBar>
    )
}
