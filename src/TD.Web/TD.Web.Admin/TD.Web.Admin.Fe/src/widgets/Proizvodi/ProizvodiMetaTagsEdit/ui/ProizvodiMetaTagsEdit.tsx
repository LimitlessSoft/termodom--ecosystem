import { Paper, Stack, TextField, Typography } from '@mui/material'
import { IProizvodiMetaTagsEditProps } from '../interfaces/IProizvodiMetaTagsEditProps'

export const ProizvodiMetaTagsEdit = (props: IProizvodiMetaTagsEditProps) => {
    return (
        <Paper variant="outlined" sx={{ p: 3 }}>
            <Typography variant="subtitle1" fontWeight="bold" color="text.secondary" sx={{ mb: 2 }}>
                Meta tagovi
            </Typography>
            <Stack spacing={3}>
                <TextField
                    fullWidth
                    disabled={props.disabled}
                    label="Meta Title"
                    value={props.metaTagTitle || ''}
                    onChange={(e) => props.onMetaTagTitleChange(e.target.value)}
                    helperText="Naslov za pretraživače (max 60 karaktera)"
                />
                <TextField
                    fullWidth
                    disabled={props.disabled}
                    multiline
                    rows={3}
                    label="Meta Description"
                    value={props.metaTagDescription || ''}
                    onChange={(e) => props.onMetaTagDescriptionChange(e.target.value)}
                    helperText="Opis za pretraživače (max 160 karaktera)"
                />
            </Stack>
        </Paper>
    )
}
