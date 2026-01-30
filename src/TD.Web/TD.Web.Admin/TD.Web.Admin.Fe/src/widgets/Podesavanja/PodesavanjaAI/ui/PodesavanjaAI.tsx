import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    Container,
    LinearProgress,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { ExpandMore, Save } from '@mui/icons-material'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'

interface AiSettings {
    modelName: string
    maxTokens: string
    temperature: string
    productNameValidation: string
    productDescriptionValidation: string
    productShortDescriptionValidation: string
    productMetaValidation: string
    productNameGenerate: string
    productDescriptionGenerate: string
    productShortDescriptionGenerate: string
    productMetaGenerate: string
    blogTitleValidation: string
    blogContentValidation: string
    blogContentGenerate: string
}

export const PodesavanjaAI = (): JSX.Element => {
    const [settings, setSettings] = useState<AiSettings | undefined>(undefined)
    const [isSaving, setIsSaving] = useState(false)

    useEffect(() => {
        adminApi
            .get('/settings/ai')
            .then((response) => {
                setSettings({
                    modelName: response.data.modelName || '',
                    maxTokens: response.data.maxTokens || '',
                    temperature: response.data.temperature || '',
                    productNameValidation: response.data.productNameValidation || '',
                    productDescriptionValidation: response.data.productDescriptionValidation || '',
                    productShortDescriptionValidation: response.data.productShortDescriptionValidation || '',
                    productMetaValidation: response.data.productMetaValidation || '',
                    productNameGenerate: response.data.productNameGenerate || '',
                    productDescriptionGenerate: response.data.productDescriptionGenerate || '',
                    productShortDescriptionGenerate: response.data.productShortDescriptionGenerate || '',
                    productMetaGenerate: response.data.productMetaGenerate || '',
                    blogTitleValidation: response.data.blogTitleValidation || '',
                    blogContentValidation: response.data.blogContentValidation || '',
                    blogContentGenerate: response.data.blogContentGenerate || '',
                })
            })
            .catch(handleApiError)
    }, [])

    const handleSave = () => {
        if (!settings) return
        setIsSaving(true)
        adminApi
            .put('/settings/ai', settings)
            .then(() => {
                toast('AI podešavanja uspešno sačuvana!', { type: 'success' })
            })
            .catch(handleApiError)
            .finally(() => setIsSaving(false))
    }

    const updateField = (field: keyof AiSettings, value: string) => {
        setSettings((prev) => prev ? { ...prev, [field]: value } : prev)
    }

    if (!settings) {
        return <LinearProgress />
    }

    return (
        <Container maxWidth="md" sx={{ py: 3 }}>
            <Typography variant="h5" gutterBottom fontWeight="bold">
                AI Podešavanja
            </Typography>

            <Stack spacing={2}>
                {/* General Settings */}
                <Accordion defaultExpanded>
                    <AccordionSummary expandIcon={<ExpandMore />}>
                        <Typography fontWeight="medium">Opšta podešavanja</Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Stack spacing={2}>
                            <TextField
                                label="Model"
                                value={settings.modelName}
                                onChange={(e) => updateField('modelName', e.target.value)}
                                helperText="Naziv OpenAI modela (npr. gpt-4o, gpt-4o-mini)"
                                size="small"
                            />
                            <Stack direction="row" spacing={2}>
                                <TextField
                                    label="Max Tokens"
                                    value={settings.maxTokens}
                                    onChange={(e) => updateField('maxTokens', e.target.value)}
                                    helperText="Maksimalan broj tokena"
                                    type="number"
                                    size="small"
                                    sx={{ width: 200 }}
                                />
                                <TextField
                                    label="Temperature"
                                    value={settings.temperature}
                                    onChange={(e) => updateField('temperature', e.target.value)}
                                    helperText="Kreativnost (0-1)"
                                    type="number"
                                    size="small"
                                    sx={{ width: 200 }}
                                    inputProps={{ step: 0.1, min: 0, max: 1 }}
                                />
                            </Stack>
                        </Stack>
                    </AccordionDetails>
                </Accordion>

                {/* Product Validation Prompts */}
                <Accordion>
                    <AccordionSummary expandIcon={<ExpandMore />}>
                        <Typography fontWeight="medium">Promptovi za validaciju proizvoda</Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Stack spacing={2}>
                            <TextField
                                label="Validacija naziva proizvoda"
                                value={settings.productNameValidation}
                                onChange={(e) => updateField('productNameValidation', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                            <TextField
                                label="Validacija opisa proizvoda"
                                value={settings.productDescriptionValidation}
                                onChange={(e) => updateField('productDescriptionValidation', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                            <TextField
                                label="Validacija kratkog opisa"
                                value={settings.productShortDescriptionValidation}
                                onChange={(e) => updateField('productShortDescriptionValidation', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                            <TextField
                                label="Validacija meta tagova"
                                value={settings.productMetaValidation}
                                onChange={(e) => updateField('productMetaValidation', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                        </Stack>
                    </AccordionDetails>
                </Accordion>

                {/* Product Generation Prompts */}
                <Accordion>
                    <AccordionSummary expandIcon={<ExpandMore />}>
                        <Typography fontWeight="medium">Promptovi za generisanje sadržaja</Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Stack spacing={2}>
                            <TextField
                                label="Generisanje naziva proizvoda"
                                value={settings.productNameGenerate}
                                onChange={(e) => updateField('productNameGenerate', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                            <TextField
                                label="Generisanje opisa proizvoda"
                                value={settings.productDescriptionGenerate}
                                onChange={(e) => updateField('productDescriptionGenerate', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                            <TextField
                                label="Generisanje kratkog opisa"
                                value={settings.productShortDescriptionGenerate}
                                onChange={(e) => updateField('productShortDescriptionGenerate', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                            <TextField
                                label="Generisanje meta tagova"
                                value={settings.productMetaGenerate}
                                onChange={(e) => updateField('productMetaGenerate', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                        </Stack>
                    </AccordionDetails>
                </Accordion>

                {/* Blog Prompts */}
                <Accordion>
                    <AccordionSummary expandIcon={<ExpandMore />}>
                        <Typography fontWeight="medium">Promptovi za blog</Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Stack spacing={2}>
                            <TextField
                                label="Validacija naslova bloga"
                                value={settings.blogTitleValidation}
                                onChange={(e) => updateField('blogTitleValidation', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                            <TextField
                                label="Validacija sadržaja bloga"
                                value={settings.blogContentValidation}
                                onChange={(e) => updateField('blogContentValidation', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                            <TextField
                                label="Generisanje sadržaja bloga"
                                value={settings.blogContentGenerate}
                                onChange={(e) => updateField('blogContentGenerate', e.target.value)}
                                multiline
                                rows={4}
                                size="small"
                            />
                        </Stack>
                    </AccordionDetails>
                </Accordion>

                {/* Save Button */}
                <Box sx={{ pt: 1 }}>
                    <Button
                        variant="contained"
                        color="primary"
                        startIcon={<Save />}
                        onClick={handleSave}
                        disabled={isSaving}
                    >
                        {isSaving ? 'Čuvanje...' : 'Sačuvaj podešavanja'}
                    </Button>
                </Box>
            </Stack>
        </Container>
    )
}
