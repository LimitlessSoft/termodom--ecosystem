import { HorizontalActionBarButton } from '@/widgets/TopActionBar'
import { PorudzbinaPretvoriUInternuOtpremnicuDialog } from '@/widgets/Porudzbine/PorudzbinaPretvoriUInternuOtpremnicu/ui/PorudzbinaPretvoriUInternuOtpremnicuDialog'
import { useState } from 'react'
import { toast } from 'react-toastify'

export const PorudzbinaPretvoriUInternuOtpremnicu = (props) => {
    const [isOpen, setIsOpen] = useState(false)
    return (
        <>
            <PorudzbinaPretvoriUInternuOtpremnicuDialog
                isOpen={isOpen}
                onClose={() => setIsOpen(false)}
                {...props}
            />
            <HorizontalActionBarButton
                color={`success`}
                isDisabled={props.isDisabled || props.porudzbina.statusId == 5}
                onClick={() => {
                    if (props.porudzbina.storeId == -5) {
                        toast.error(`Morate izabrati validan magacin!`)
                        return
                    }
                    setIsOpen(true)
                }}
                text={`Pretvori u internu otpremnicu`}
            />
        </>
    )
}
