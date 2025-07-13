import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { SpecifikacijaNovcaDataField } from './SpecifikacijaNovcaDataField'

export const SpecifikacijaNovcaKomentar = ({
    komentar,
    onChange,
    disabled,
}) => {
    return (
        <SpecifikacijaNovcaBox title={`Komentar`}>
            <SpecifikacijaNovcaDataField
                disabled={disabled}
                onChange={onChange}
                multiline
                value={komentar}
            />
        </SpecifikacijaNovcaBox>
    )
}
