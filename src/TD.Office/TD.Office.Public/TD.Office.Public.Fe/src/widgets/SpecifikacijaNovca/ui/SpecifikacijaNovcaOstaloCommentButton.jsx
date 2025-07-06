import { Comment, Replay } from '@mui/icons-material'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    Grid,
    IconButton,
    Stack,
    Typography,
} from '@mui/material'
import { useState } from 'react'
import { EnchantedTextField } from '@/widgets'

export const SpecifikacijaNovcaOstaloCommentButton = ({
    comment,
    title,
    onChange,
}) => {
    const initialValue = comment || ''
    const [value, setValue] = useState(comment)
    const [isCommentShown, setIsCommentShown] = useState(false)

    return (
        <Grid item>
            <Dialog
                open={isCommentShown}
                onClose={() => {
                    setIsCommentShown(false)
                }}
            >
                <DialogContent
                    sx={{
                        minWidth: `300px`,
                    }}
                >
                    <Stack gap={2}>
                        <Typography variant={`h6`}>Komentar {title}</Typography>
                        <EnchantedTextField
                            fullWidth
                            textAlignment="left"
                            value={value}
                            multiline
                            onChange={(e) => setValue(e)}
                        />
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Stack direction={`row`} gap={2}>
                        {initialValue !== value && (
                            <IconButton
                                onClick={() => {
                                    setValue(initialValue)
                                }}
                            >
                                <Replay color="primary" />
                            </IconButton>
                        )}
                        <Button
                            variant={`contained`}
                            onClick={() => {
                                setIsCommentShown(false)
                                onChange(value)
                            }}
                        >
                            Zatvori
                        </Button>
                    </Stack>
                </DialogActions>
            </Dialog>

            <Button
                variant={value ? `contained` : `outlined`}
                onClick={() => setIsCommentShown((prevState) => !prevState)}
            >
                <Comment />
            </Button>
        </Grid>
    )
}
