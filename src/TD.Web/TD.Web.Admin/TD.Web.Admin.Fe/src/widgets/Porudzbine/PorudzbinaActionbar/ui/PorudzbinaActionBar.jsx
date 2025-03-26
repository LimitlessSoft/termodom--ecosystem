import {
    HorizontalActionBar,
    HorizontalActionBarButton,
} from '@/widgets/TopActionBar'
import { toast } from 'react-toastify'
import { LinearProgress } from '@mui/material'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { PorudzbinaPretvoriUInternuOtpremnicu } from '@/widgets/Porudzbine/PorudzbinaPretvoriUInternuOtpremnicu/ui/PorudzbinaPretvoriUInternuOtpremnicu'

export const PorudzbinaActionBar = (props) => {
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
                        .catch((err) => handleApiError(err))
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
                    color={`success`}
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
                                    type: 0,
                                }
                            )
                            .then(() => {
                                props.onPretvoriUProracunSuccess()
                                toast.success(
                                    `Porudžbina prebačena u komercijalno poslovanje!`
                                )
                            })
                            .catch((err) => {
                                props.onPretvoriUProracunFail()
                                handleApiError(err)
                            })
                    }}
                    text={`Pretvori u proračun`}
                />
            )}
            {props.porudzbina.komercijalnoBrDok != null ? null : (
                <HorizontalActionBarButton
                    color={`success`}
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
                                    type: 1,
                                }
                            )
                            .then(() => {
                                props.onPretvoriUProracunSuccess()
                                toast.success(
                                    `Porudžbina prebačena u komercijalno poslovanje!`
                                )
                            })
                            .catch((err) => {
                                props.onPretvoriUProracunFail()
                                handleApiError(err)
                            })
                    }}
                    text={`Pretvori u ponudu`}
                />
            )}
            {props.porudzbina.komercijalnoBrDok != null ? null : (
                <HorizontalActionBarButton
                    color={`success`}
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
                                    type: 2,
                                }
                            )
                            .then(() => {
                                props.onPretvoriUProracunSuccess()
                                toast.success(
                                    `Porudžbina prebačena u komercijalno poslovanje!`
                                )
                            })
                            .catch((err) => {
                                props.onPretvoriUProracunFail()
                                handleApiError(err)
                            })
                    }}
                    text={`Pretvori u profakturu`}
                />
            )}
            {props.porudzbina.komercijalnoBrDok != null ? null : (
                <PorudzbinaPretvoriUInternuOtpremnicu
                    isDisabled={props.isDisabled}
                    porudzbina={props.porudzbina}
                    onPretvoriUProracunStart={props.onPretvoriUProracunStart}
                    onPretvoriUProracunSuccess={
                        props.onPretvoriUProracunSuccess
                    }
                    onPretvoriUProracunFail={props.onPretvoriUProracunFail}
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
                            .catch((err) => {
                                props.onStornirajFail()
                                handleApiError(err)
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
                            .catch((err) => handleApiError(err))
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
