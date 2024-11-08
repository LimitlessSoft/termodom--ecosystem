import { Comment } from '@mui/icons-material'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    Grid,
    Stack,
    Typography,
} from '@mui/material'
import { useState } from 'react'
import { ISpecifikacijaNovcaOstaloCommentButtonProps } from '../interfaces/ISpecifikacijaNovcaOstaloCommentButtonProps'
import { EnchantedTextField } from '@/widgets'

export const SpecifikacijaNovcaOstaloCommentButton = ({
    comment,
    title,
    onSave,
}: ISpecifikacijaNovcaOstaloCommentButtonProps) => {
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
                        <Button
                            variant={`contained`}
                            onClick={() => {
                                setIsCommentShown(false)
                                onSave && onSave(value)
                            }}
                        >
                            Sacuvaj
                        </Button>
                        <Button
                            variant={`outlined`}
                            onClick={() => {
                                setIsCommentShown(false)
                            }}
                        >
                            Zatvori
                        </Button>
                    </Stack>
                </DialogActions>
            </Dialog>

            <Button
                variant={`contained`}
                onClick={() => setIsCommentShown((prevState) => !prevState)}
            >
                <Comment />
            </Button>
        </Grid>
    )
}
