import { IPorudzbina } from '../../models/IPorudzbina'

export interface IPorudzbinaActionBarProps {
    porudzbina: IPorudzbina
    isDisabled: boolean
    onPretvoriUProracunStart: () => void
    onPretvoriUPonuduStart: () => void
    onRazveziOdProracunaStart: () => void
    onPretvoriUProracunSuccess: () => void
    onPretvoriUProracunFail: () => void
    onPretvoriUPonuduEnd: () => void
    onRazveziOdProracunaEnd: () => void
    onPreuzmiNaObraduEnd: () => void
    onPreuzmiNaObraduStart: () => void
    onStornirajStart: () => void
    onStornirajSuccess: () => void
    onStornirajFail: () => void
}
