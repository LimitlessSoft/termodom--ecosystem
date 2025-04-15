import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
} from '@mui/material'
import { useState } from 'react'
import { KolicineInput } from '@/widgets/Proizvodi/ProizvodiSrc/KolicineInput/KolicineInput'
import { useEffect } from 'react'

export const KorpaIzmenaKolicineDialog = ({
    isOpen,
    oneAlternatePackageEquals,
    baseUnit,
    alternateUnit,
    quantity,
    onConfirm,
    onClose,
}) => {
    const [baseQuantity, setBaseQuantity] = useState(0)
    const [alternateQuantity, setAlternateQuantity] = useState(0)

    useEffect(() => {
        if (isOpen) {
            setBaseQuantity(
                oneAlternatePackageEquals
                    ? quantity / oneAlternatePackageEquals
                    : quantity
            )
            setAlternateQuantity(oneAlternatePackageEquals ? quantity : null)
        }
    }, [isOpen, quantity, oneAlternatePackageEquals])

    const handleQuantityChange = (value) => {
        setBaseQuantity(parseFloat(value.toFixed(3)))

        if (oneAlternatePackageEquals) {
            setAlternateQuantity(
                parseFloat((value * oneAlternatePackageEquals).toFixed(3))
            )
        }
    }

    return (
        <Dialog
            open={isOpen}
            onClose={onClose}
            TransitionProps={{
                onExited: () => {
                    setBaseQuantity(0)
                    setAlternateQuantity(0)
                },
            }}
        >
            <DialogTitle>Izmena količine</DialogTitle>
            <DialogContent>
                <KolicineInput
                    baseQuantity={baseQuantity}
                    alternateQuantity={alternateQuantity}
                    baseUnit={baseUnit}
                    alternateUnit={alternateUnit}
                    oneAlternatePackageEquals={oneAlternatePackageEquals}
                    onQuantityChange={handleQuantityChange}
                />
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        onConfirm(alternateQuantity || baseQuantity)
                        onClose()
                    }}
                >
                    Ažuriraj količinu
                </Button>
                <Button onClick={onClose}>Odustani</Button>
            </DialogActions>
        </Dialog>
    )
}
