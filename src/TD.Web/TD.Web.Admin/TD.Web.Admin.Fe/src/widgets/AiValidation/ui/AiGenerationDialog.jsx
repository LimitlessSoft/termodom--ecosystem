import React, { useState } from 'react'
import {
    Alert,
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Divider,
    LinearProgress,
    Paper,
    Stack,
    Tab,
    Tabs,
    Typography,
} from '@mui/material'
import {
    CheckCircle,
    Error as ErrorIcon,
    AutoAwesome,
} from '@mui/icons-material'

export const AiGenerationDialog = ({
    open,
    onClose,
    onAccept,
    result,
    fieldLabel,
    isLoading = false,
}) => {
    const [selectedTab, setSelectedTab] = useState(0)

    if (!result && !isLoading) return null

    const hasHtmlContent = result?.htmlContent && result.htmlContent !== result?.generatedContent
    const hasAlternative = result?.alternativeContent

    return (
        <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
            <DialogTitle>
                <Stack direction="row" alignItems="center" spacing={1}>
                    <AutoAwesome color="secondary" />
                    <Typography variant="h6">
                        AI Generisanje: {fieldLabel}
                    </Typography>
                </Stack>
            </DialogTitle>

            <DialogContent>
                {isLoading && (
                    <Box sx={{ py: 4 }}>
                        <Typography
                            variant="body2"
                            color="text.secondary"
                            align="center"
                            sx={{ mb: 2 }}
                        >
                            AI generise sadrzaj...
                        </Typography>
                        <LinearProgress color="secondary" />
                    </Box>
                )}

                {result && !isLoading && (
                    <Stack spacing={3}>
                        {result.success ? (
                            <Alert
                                severity="success"
                                icon={<CheckCircle />}
                                sx={{ mb: 2 }}
                            >
                                <Typography variant="body1">
                                    Sadrzaj je uspesno generisan!
                                </Typography>
                            </Alert>
                        ) : (
                            <Alert
                                severity="error"
                                icon={<ErrorIcon />}
                                sx={{ mb: 2 }}
                            >
                                <Typography variant="body1">
                                    {result.errorMessage || 'Greska pri generisanju sadrzaja'}
                                </Typography>
                            </Alert>
                        )}

                        {result.success && (
                            <>
                                {(hasHtmlContent || hasAlternative) && (
                                    <Tabs
                                        value={selectedTab}
                                        onChange={(_, newValue) => setSelectedTab(newValue)}
                                        sx={{ borderBottom: 1, borderColor: 'divider' }}
                                    >
                                        <Tab label="Generisani sadrzaj" />
                                        {hasHtmlContent && <Tab label="HTML verzija" />}
                                        {hasAlternative && <Tab label="Alternativa" />}
                                    </Tabs>
                                )}

                                {selectedTab === 0 && result.generatedContent && (
                                    <Box>
                                        <Typography
                                            variant="subtitle2"
                                            color="secondary.main"
                                            sx={{ mb: 1 }}
                                        >
                                            Generisani sadrzaj:
                                        </Typography>
                                        <Paper
                                            variant="outlined"
                                            sx={{
                                                p: 2,
                                                bgcolor: 'secondary.50',
                                                borderColor: 'secondary.200',
                                                maxHeight: 300,
                                                overflow: 'auto',
                                            }}
                                        >
                                            <Typography
                                                variant="body2"
                                                sx={{ whiteSpace: 'pre-wrap' }}
                                            >
                                                {result.generatedContent}
                                            </Typography>
                                        </Paper>
                                    </Box>
                                )}

                                {selectedTab === 1 && hasHtmlContent && (
                                    <Box>
                                        <Typography
                                            variant="subtitle2"
                                            color="secondary.main"
                                            sx={{ mb: 1 }}
                                        >
                                            HTML formatirana verzija:
                                        </Typography>
                                        <Paper
                                            variant="outlined"
                                            sx={{
                                                p: 2,
                                                bgcolor: 'secondary.50',
                                                borderColor: 'secondary.200',
                                                maxHeight: 300,
                                                overflow: 'auto',
                                            }}
                                        >
                                            <Typography
                                                variant="body2"
                                                sx={{ whiteSpace: 'pre-wrap' }}
                                            >
                                                {result.htmlContent}
                                            </Typography>
                                        </Paper>
                                        <Divider sx={{ my: 2 }} />
                                        <Typography
                                            variant="subtitle2"
                                            color="text.secondary"
                                            sx={{ mb: 1 }}
                                        >
                                            Pregled:
                                        </Typography>
                                        <Paper
                                            variant="outlined"
                                            sx={{
                                                p: 2,
                                                maxHeight: 200,
                                                overflow: 'auto',
                                            }}
                                        >
                                            <div
                                                dangerouslySetInnerHTML={{
                                                    __html: result.htmlContent,
                                                }}
                                            />
                                        </Paper>
                                    </Box>
                                )}

                                {selectedTab === (hasHtmlContent ? 2 : 1) && hasAlternative && (
                                    <Box>
                                        <Typography
                                            variant="subtitle2"
                                            color="info.main"
                                            sx={{ mb: 1 }}
                                        >
                                            Alternativni predlog:
                                        </Typography>
                                        <Paper
                                            variant="outlined"
                                            sx={{
                                                p: 2,
                                                bgcolor: 'info.50',
                                                borderColor: 'info.200',
                                                maxHeight: 300,
                                                overflow: 'auto',
                                            }}
                                        >
                                            <Typography
                                                variant="body2"
                                                sx={{ whiteSpace: 'pre-wrap' }}
                                            >
                                                {result.alternativeContent}
                                            </Typography>
                                        </Paper>
                                    </Box>
                                )}
                            </>
                        )}
                    </Stack>
                )}
            </DialogContent>

            <DialogActions>
                <Button onClick={onClose} color="inherit">
                    Zatvori
                </Button>
                {result?.success && result?.generatedContent && (
                    <>
                        {hasHtmlContent && (
                            <Button
                                variant="outlined"
                                color="secondary"
                                onClick={() => {
                                    onAccept(result.htmlContent)
                                    onClose()
                                }}
                            >
                                Prihvati HTML
                            </Button>
                        )}
                        <Button
                            variant="contained"
                            color="secondary"
                            onClick={() => {
                                onAccept(result.generatedContent)
                                onClose()
                            }}
                        >
                            Prihvati sadrzaj
                        </Button>
                    </>
                )}
            </DialogActions>
        </Dialog>
    )
}

export default AiGenerationDialog
