'use client'
import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    CircularProgress,
    Divider,
    IconButton,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useRef, useState } from 'react'
import { handleResponse } from '@/helpers/responseHelpers'
import {
    AddCircle,
    ArrowDownward,
    Delete,
    KeyboardArrowLeft,
    Upload,
} from '@mui/icons-material'
import { QuizzEditQuestionNew } from '@/widgets/QuizzEdit/ui/quizzEditQuestionNew'
import { toast } from 'react-toastify'
import { convertImageToBase64 } from '@/widgets/QuizzEdit/helpers/quizzEditHelpers'
import NextLink from 'next/link'
import QuizzQuestionDurationSelectInput from '@/widgets/Quizz/ui/QuizzQuestionDurationSelectInput'
import NumberInput from '@/widgets/Input/NumberInput'

export const QuizzEdit = ({ id }) => {
    const [quizz, setQuizz] = useState(null)
    const [isSaving, setIsSaving] = useState(false)
    const [hasChanges, setHasChanges] = useState(false)
    const [newQuestionDialogOpen, setNewQuestionDialogOpen] = useState(false)

    useEffect(() => {
        fetch(`/api/admin-quiz?id=${id}`).then(async (response) => {
            handleResponse(response, (data) => {
                setQuizz(data)
            })
        })
    }, [id])

    const handleChangeDuration = (index, duration) => {
        setQuizz((prev) => {
            const updatedQuestions = prev.quizz_question.map((question, idx) =>
                idx === index
                    ? {
                          ...question,
                          duration: { ...question.duration, value: duration },
                      }
                    : question
            )

            setHasChanges(
                prev.quizz_question[index].duration.value !== duration
            )

            return { ...prev, quizz_question: updatedQuestions }
        })
    }

    const handleUpdateAnswer = (
        question_index,
        index,
        propertyKey,
        newValue
    ) => {
        setQuizz((prev) => {
            const updatedQuestions = [...prev.quizz_question]
            updatedQuestions[question_index].answers[index][propertyKey] =
                newValue

            return {
                ...prev,
                quizz_question: updatedQuestions,
            }
        })

        setHasChanges(true)
    }

    if (!quizz) return <CircularProgress />
    return (
        <Stack
            justifyContent={`center`}
            alignItems={`center`}
            spacing={2}
            minHeight={`100vh`}
            maxWidth={`600px`}
            margin={`auto`}
        >
            <Stack
                direction={`row`}
                spacing={2}
                alignItems={`center`}
                justifyContent={`space-between`}
                width={`100%`}
            >
                <Button
                    startIcon={<KeyboardArrowLeft />}
                    variant={`contained`}
                    href={`/admin`}
                    LinkComponent={NextLink}
                >
                    Nazad
                </Button>
                <Typography>Id: {id}</Typography>
                <TextField
                    disabled={isSaving}
                    label={`Ime Kviza`}
                    value={quizz.name}
                    onChange={(e) => {
                        setQuizz((prevQuizz) => ({
                            ...prevQuizz,
                            name: e.target.value,
                        }))
                        setHasChanges(true)
                    }}
                />
            </Stack>
            <Paper sx={{ p: 2 }}>
                <Stack gap={2}>
                    <Stack
                        direction={`row`}
                        spacing={2}
                        justifyContent={`space-between`}
                        alignItems={`center`}
                    >
                        <QuizzEditQuestionNew
                            disabled={isSaving}
                            isOpen={newQuestionDialogOpen}
                            onClose={() => setNewQuestionDialogOpen(false)}
                            defaultDuration={quizz.defaultDuration}
                            onConfirm={(newQuestion) => {
                                setQuizz((prevQuizz) => {
                                    const updatedQuestions = [
                                        ...prevQuizz.quizz_question,
                                        newQuestion,
                                    ]
                                    return {
                                        ...prevQuizz,
                                        quizz_question: updatedQuestions,
                                    }
                                })
                                setNewQuestionDialogOpen(false)
                                setHasChanges(true)
                            }}
                        />
                        <Typography variant={`subtitle1`}>
                            Lista pitanja
                        </Typography>
                        <IconButton
                            disabled={isSaving}
                            onClick={() => {
                                setNewQuestionDialogOpen(true)
                            }}
                        >
                            <AddCircle
                                color={isSaving ? `disabled` : `primary`}
                            />
                        </IconButton>
                    </Stack>
                    <Divider sx={{ width: `100%` }} />
                    <Stack spacing={1}>
                        {quizz.quizz_question?.map((question, index) => (
                            <Accordion
                                disabled={isSaving}
                                key={index}
                                sx={{
                                    backgroundColor: `#fdd`,
                                }}
                            >
                                <AccordionSummary
                                    expandIcon={<ArrowDownward />}
                                >
                                    <Typography>{question.title}</Typography>
                                </AccordionSummary>
                                <AccordionDetails>
                                    <Stack spacing={2} minWidth={600}>
                                        <QuizzQuestionDurationSelectInput
                                            defaultDuration={
                                                quizz.defaultDuration
                                            }
                                            duration={question.duration}
                                            onChangeDuration={(duration) => {
                                                handleChangeDuration(
                                                    index,
                                                    duration
                                                )
                                            }}
                                        />
                                        <TextField
                                            disabled={isSaving}
                                            onChange={(e) => {
                                                setQuizz((prevQuizz) => {
                                                    const updatedQuestions = [
                                                        ...prevQuizz.quizz_question,
                                                    ]
                                                    updatedQuestions[
                                                        index
                                                    ].title = e.target.value
                                                    return {
                                                        ...prevQuizz,
                                                        quizz_question:
                                                            updatedQuestions,
                                                    }
                                                })
                                                setHasChanges(true)
                                            }}
                                            value={question.title}
                                            fullWidth
                                            label={`Naslov pitanja`}
                                        />
                                        <TextField
                                            disabled={isSaving}
                                            onChange={(e) => {
                                                setQuizz((prevQuizz) => {
                                                    const updatedQuestions = [
                                                        ...prevQuizz.quizz_question,
                                                    ]
                                                    updatedQuestions[
                                                        index
                                                    ].text = e.target.value
                                                    return {
                                                        ...prevQuizz,
                                                        quizz_question:
                                                            updatedQuestions,
                                                    }
                                                })
                                                setHasChanges(true)
                                            }}
                                            value={question.text}
                                            fullWidth
                                            label={`Tekst pitanja`}
                                        />
                                        <ImageDisplay
                                            isSaving={isSaving}
                                            setQuizz={setQuizz}
                                            setHasChanges={setHasChanges}
                                            question={question}
                                        />
                                        <Divider sx={{ width: `100%` }} />
                                        {question.answers.map(
                                            (answer, ansIndex) => (
                                                <Stack
                                                    key={ansIndex}
                                                    direction={`row`}
                                                    spacing={1}
                                                    alignItems={`center`}
                                                >
                                                    <TextField
                                                        disabled={isSaving}
                                                        onChange={(e) => {
                                                            handleUpdateAnswer(
                                                                index,
                                                                ansIndex,
                                                                'text',
                                                                e.target.value
                                                            )
                                                        }}
                                                        label={`Odgovor ${
                                                            ansIndex + 1
                                                        }`}
                                                        value={answer.text}
                                                        fullWidth
                                                    />
                                                    <NumberInput
                                                        disabled={isSaving}
                                                        label="Broj poena"
                                                        value={
                                                            answer.points ?? ''
                                                        }
                                                        additionalAllowedKeys={[
                                                            '-',
                                                        ]}
                                                        onChange={(e) => {
                                                            const { value } =
                                                                e.target
                                                            handleUpdateAnswer(
                                                                index,
                                                                ansIndex,
                                                                'points',
                                                                value === ''
                                                                    ? undefined
                                                                    : +value
                                                            )
                                                        }}
                                                    />
                                                    <Button
                                                        disabled={isSaving}
                                                        variant={`outlined`}
                                                        onClick={() => {
                                                            handleUpdateAnswer(
                                                                index,
                                                                ansIndex,
                                                                'isCorrect',
                                                                !answer.isCorrect
                                                            )
                                                        }}
                                                    >
                                                        {answer.isCorrect
                                                            ? `Tacan`
                                                            : `Nije tacan`}
                                                    </Button>
                                                    <IconButton
                                                        disabled={isSaving}
                                                        onClick={() => {
                                                            setQuizz(
                                                                (prevQuizz) => {
                                                                    const updatedQuestions =
                                                                        [
                                                                            ...prevQuizz.quizz_question,
                                                                        ]
                                                                    updatedQuestions[
                                                                        index
                                                                    ].answers.splice(
                                                                        ansIndex,
                                                                        1
                                                                    )
                                                                    return {
                                                                        ...prevQuizz,
                                                                        quizz_question:
                                                                            updatedQuestions,
                                                                    }
                                                                }
                                                            )
                                                            setHasChanges(true)
                                                        }}
                                                    >
                                                        <Delete />
                                                    </IconButton>
                                                </Stack>
                                            )
                                        )}
                                        <Button
                                            variant={`outlined`}
                                            disabled={isSaving}
                                            onClick={() => {
                                                setQuizz((prevQuizz) => {
                                                    const updatedQuestions = [
                                                        ...prevQuizz.quizz_question,
                                                    ]
                                                    updatedQuestions[
                                                        index
                                                    ].answers.push({
                                                        text: ``,
                                                        isCorrect: false,
                                                    })
                                                    return {
                                                        ...prevQuizz,
                                                        quizz_question:
                                                            updatedQuestions,
                                                    }
                                                })
                                                setHasChanges(true)
                                            }}
                                        >
                                            Dodaj odgovor
                                        </Button>
                                    </Stack>
                                </AccordionDetails>
                            </Accordion>
                        ))}
                    </Stack>
                    {hasChanges && (
                        <Button
                            disabled={isSaving}
                            variant={`contained`}
                            onClick={() => {
                                setIsSaving(true)
                                fetch(`/api/admin-quiz`, {
                                    method: `PUT`,
                                    headers: {
                                        'Content-Type': 'application/json',
                                    },
                                    body: JSON.stringify({
                                        id: quizz.id,
                                        name: quizz.name,
                                        questions: quizz.quizz_question.map(
                                            (question) => ({
                                                id: question.id,
                                                title: question.title,
                                                text: question.text,
                                                image: question.image,
                                                answers: question.answers,
                                                quizz_schema_id: quizz.id,
                                                duration:
                                                    question.duration.value,
                                            })
                                        ),
                                    }),
                                }).then(async (response) => {
                                    handleResponse(
                                        response,
                                        (data) => {
                                            setIsSaving(false)
                                            setHasChanges(false)
                                            toast.success(`Kviz sacuvan`)
                                        },
                                        () => {
                                            setIsSaving(false)
                                            toast.error(
                                                `Greska prilikom cuvanja kviza`
                                            )
                                        }
                                    )
                                })
                            }}
                        >
                            Sacuvaj izmene
                        </Button>
                    )}
                </Stack>
            </Paper>
        </Stack>
    )
}

const ImageDisplay = ({ question, isSaving, setQuizz, setHasChanges }) => {
    const inputRef = useRef(null)
    return (
        <>
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
                    cursor: isSaving ? `not-allowed` : `pointer`,
                    border: `1px dashed #444`,
                    borderRadius: 1,
                    p: 2,
                    minHeight: `200px`,
                    width: `100%`,
                }}
                onClick={() => {
                    if (isSaving) return
                    inputRef.current.click()
                }}
            >
                {!question.image && <Upload />}
            </Box>
            <TextField
                sx={{ display: `none` }}
                disabled={isSaving}
                type="file"
                inputRef={inputRef}
                variant="outlined"
                onChange={(e) => {
                    const file = e.target.files[0]
                    if (file) {
                        // check if file is an image
                        if (!file.type.startsWith(`image/`)) {
                            alert(`Molimo odaberite sliku (jpg, png, jpeg)`)
                            return
                        }
                        convertImageToBase64(file).then((base64) => {
                            setQuizz((prevQuizz) => {
                                const updatedQuestions =
                                    prevQuizz.quizz_question.map((q) =>
                                        q === question
                                            ? { ...q, image: base64 }
                                            : q
                                    )
                                return {
                                    ...prevQuizz,
                                    quizz_question: updatedQuestions,
                                }
                            })
                            setHasChanges(true)
                        })
                    }
                }}
            />
        </>
    )
}
