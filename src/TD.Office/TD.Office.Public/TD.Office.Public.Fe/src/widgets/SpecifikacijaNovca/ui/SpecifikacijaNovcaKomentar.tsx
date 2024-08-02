import { ISpecifikacijaNovcaKomentarProps } from '../interfaces/ISpecifikacijaNovcaKomentarProps'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { SpecifikacijaNovcaDataField } from './SpecifikacijaNovcaDataField'

export const SpecifikacijaNovcaKomentar = ({
    komentar,
    onChange,
}: ISpecifikacijaNovcaKomentarProps) => {
    return (
        <SpecifikacijaNovcaBox title={`Komentar`}>
            <SpecifikacijaNovcaDataField
                onChange={onChange}
                multiline
                value={komentar}
            />
        </SpecifikacijaNovcaBox>
    )
}
