import { Grid } from '@mui/material'
import KomentarInput from '@/widgets/KomentarInput'

const PorudzbinaComment = ({ label, defaultValue, onSave }) => {
    return (
        <Grid item md={6} xs={12}>
            <KomentarInput
                label={label}
                defaultValue={defaultValue || ''}
                onSave={onSave}
            />
        </Grid>
    )
}

export default PorudzbinaComment
