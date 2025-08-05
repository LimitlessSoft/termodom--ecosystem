import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Divider,
    IconButton,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { useState } from 'react'
import { Delete, Upload } from '@mui/icons-material'
import { convertImageToBase64 } from '@/widgets/QuizzEdit/helpers/quizzEditHelpers'

export const QuizzEditQuestionNew = ({
    isOpen,
    onClose,
    onConfirm,
    disabled,
}) => {
    const [question, setQuestion] = useState({
        title: ``,
        text: ``,
        image: null,
        answers: [],
    })
    return (
        <>
            <Dialog open={isOpen} onClose={onClose} fullWidth={true}>
                <DialogTitle>Novo pitanje</DialogTitle>
                <DialogContent>
                    <Stack spacing={1} py={1}>
                        <TextField
                            disabled={disabled}
                            label={`Naslov pitanja`}
                            value={question.title}
                            onChange={(e) => {
                                setQuestion((prev) => ({
                                    ...prev,
                                    title: e.target.value,
                                }))
                            }}
                        />
                        <TextField
                            disabled={disabled}
                            label={`Tekst pitanja`}
                            multiline
                            rows={4}
                            value={question.text}
                            onChange={(e) => {
                                setQuestion((prev) => ({
                                    ...prev,
                                    text: e.target.value,
                                }))
                            }}
                        />
                        <Box
                            sx={{
                                display: `flex`,
                                position: `relative`,
                                overflow: `hidden`,
                                backgroundImage: `url(${question.image})`,
                                backgroundSize: `contain`,
                                backgroundRepeat: `no-repeat`,
                                backgroundPosition: `center`,
                                alignItems: `center`,
                                justifyContent: `center`,
                                cursor: disabled ? `not-allowed` : `pointer`,
                                border: `1px dashed #444`,
                                borderRadius: 1,
                                p: 2,
                                minHeight: `200px`,
                                width: `100%`,
                            }}
                            onClick={() => {
                                if (disabled) return
                                document
                                    .querySelector(`input[type="file"]`)
                                    .click()
                            }}
                        >
                            {!question.image && <Upload />}
                        </Box>
                        <TextField
                            sx={{ display: `none` }}
                            disabled={disabled}
                            type="file"
                            variant="outlined"
                            onChange={(e) => {
                                const file = e.target.files[0]
                                if (file) {
                                    // check if file is an image
                                    if (!file.type.startsWith(`image/`)) {
                                        alert(
                                            `Molimo odaberite sliku (jpg, png, jpeg)`
                                        )
                                        return
                                    }
                                    convertImageToBase64(file).then(
                                        (base64) => {
                                            setQuestion((prev) => ({
                                                ...prev,
                                                image: base64,
                                            }))
                                        }
                                    )
                                }
                            }}
                        />
                        <Divider sx={{ width: `100%`, py: 1 }} />
                        <Button
                            disabled={disabled}
                            variant={`outlined`}
                            onClick={() => {
                                setQuestion((prev) => ({
                                    ...prev,
                                    answers: [
                                        ...prev.answers,
                                        { text: ``, isCorrect: false },
                                    ],
                                }))
                            }}
                        >
                            Dodaj odgovor
                        </Button>
                        {question.answers.map((answer, index) => (
                            <Stack
                                key={index}
                                direction={`row`}
                                spacing={1}
                                alignItems={`center`}
                            >
                                <TextField
                                    disabled={disabled}
                                    label={`Odgovor ${index + 1}`}
                                    value={answer.text}
                                    onChange={(e) => {
                                        setQuestion((prev) => {
                                            const newAnswers = prev.answers.map(
                                                (ans, i) =>
                                                    i === index
                                                        ? {
                                                              ...ans,
                                                              text: e.target
                                                                  .value,
                                                          }
                                                        : ans
                                            )
                                            return {
                                                ...prev,
                                                answers: newAnswers,
                                            }
                                        })
                                    }}
                                    fullWidth
                                />
                                <Button
                                    disabled={disabled}
                                    variant={`outlined`}
                                    onClick={() => {
                                        setQuestion((prev) => {
                                            const newAnswers = prev.answers.map(
                                                (ans, i) =>
                                                    i === index
                                                        ? {
                                                              ...ans,
                                                              isCorrect:
                                                                  !ans.isCorrect,
                                                          }
                                                        : ans
                                            )
                                            return {
                                                ...prev,
                                                answers: newAnswers,
                                            }
                                        })
                                    }}
                                >
                                    {answer.isCorrect ? `Tacan` : `Nije tacan`}
                                </Button>
                                <IconButton
                                    onClick={() => {
                                        setQuestion((prev) => ({
                                            ...prev,
                                            answers: prev.answers.filter(
                                                (_, i) => i !== index
                                            ),
                                        }))
                                    }}
                                >
                                    <Delete />
                                </IconButton>
                            </Stack>
                        ))}
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Button
                        disabled={disabled}
                        variant={`contained`}
                        onClick={() => {
                            onConfirm(question)
                            setQuestion({
                                title: ``,
                                text: ``,
                                image: null,
                                answers: [],
                            })
                        }}
                    >
                        Kreiraj
                    </Button>
                    <Button
                        variant={`outlined`}
                        onClick={onClose}
                        disabled={disabled}
                    >
                        Odustani
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    )
}
