import { Box, Grid, TextField, Typography } from "@mui/material"
import { IProizvodiMetaTagsEditProps } from "../interfaces/IProizvodiMetaTagsEditProps"

export const ProizvodiMetaTagsEdit = (props: IProizvodiMetaTagsEditProps) => {
    return (
        <Box sx={{ backgroundColor: `#8dd`, textAlign: `center`}}>
            <Typography p={2} variant="h6">Meta tags</Typography>
            <TextField
                label="Meta tag title"
                defaultValue={props.metaTagTitle}
                onChange={(e) => props.onMetaTagTitleChange(e.target.value)}
            />
            <TextField
                label="Meta tag description"
                defaultValue={props.metaTagDescription}
                onChange={(e) => props.onMetaTagDescriptionChange(e.target.value)}
            />
        </Box>
    )
}