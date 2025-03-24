import {
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    Divider,
    Stack,
    Typography,
} from '@mui/material'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { MagaciniDropdown } from '@/widgets/MagaciniDropdown/ui/MagaciniDropdown'
import { useZMagacini } from '@/zStore'
import { useEffect, useState } from 'react'

export const PorudzbinaPretvoriUInternuOtpremnicuDialog = (props) => {
    const zMagacini = useZMagacini()
    const [destinacioniMagacinId, setDestinacioniMagacinId] =
        useState(undefined)

    useEffect(() => {
        if (!zMagacini) {
            setDestinacioniMagacinId(undefined)
            return
        }
        setDestinacioniMagacinId(zMagacini[0]?.magacinId)
    }, [zMagacini])
    if (!zMagacini) return <CircularProgress />
    return (
        <Dialog open={props.isOpen} onClose={props.onClose}>
            <DialogContent>
                <Stack gap={2}>
                    <Typography variant={`h5`}>
                        Realizovanje porudzbine kroz Internu otpremnicu
                    </Typography>
                    <Divider />
                    <Typography variant={`h6`}>
                        Iz magacina:{' '}
                        {
                            zMagacini.find(
                                (x) => x.magacinId === props.porudzbina.storeId
                            ).naziv
                        }
                    </Typography>
                    <Typography variant={`h6`}>U Magacin:</Typography>
                    <MagaciniDropdown
                        multiselect={false}
                        onChange={(magacinId) => {
                            setDestinacioniMagacinId(magacinId)
                        }}
                        value={destinacioniMagacinId}
                    />
                </Stack>
            </DialogContent>
            <DialogActions>
                <Button
                    color={`success`}
                    onClick={() => {
                        props.onPretvoriUProracunStart()

                        adminApi
                            .post(
                                `/orders/${props.porudzbina?.oneTimeHash}/forward-to-komercijalno`,
                                {
                                    oneTimeHash: props.porudzbina.oneTimeHash,
                                    type: 3,
                                    destinacioniMagacinId:
                                        destinacioniMagacinId,
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
                    variant={`contained`}
                >
                    Realizuj kreiranjem Interne Otpremncie
                </Button>
            </DialogActions>
        </Dialog>
    )
}
