import React from 'react'
import {
    Alert,
    Box,
    Button,
    Chip,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Divider,
    LinearProgress,
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import {
    CheckCircle,
    Warning,
    Lightbulb,
    Error as ErrorIcon,
} from '@mui/icons-material'

export const AiSuggestionDialog = ({
    open,
    onClose,
    onAccept,
    result,
    fieldLabel,
    isLoading = false,
}) => {
    if (!result && !isLoading) return null

    const getConfidenceColor = (score) => {
        if (score >= 0.8) return 'success'
        if (score >= 0.5) return 'warning'
        return 'error'
    }

    const getConfidenceLabel = (score) => {
        if (score >= 0.8) return 'Visoka pouzdanost'
        if (score >= 0.5) return 'Srednja pouzdanost'
        return 'Niska pouzdanost'
    }

    return (
        <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
            <DialogTitle>
                <Stack
                    direction="row"
                    alignItems="center"
                    justifyContent="space-between"
                >
                    <Typography variant="h6">
                        AI Validacija: {fieldLabel}
                    </Typography>
                    {result && (
                        <Chip
                            size="small"
                            color={getConfidenceColor(result.confidenceScore)}
                            label={`${getConfidenceLabel(result.confidenceScore)} (${Math.round(result.confidenceScore * 100)}%)`}
                        />
                    )}
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
                            AI analizira sadrzaj...
                        </Typography>
                        <LinearProgress />
                    </Box>
                )}

                {result && !isLoading && (
                    <Stack spacing={3}>
                        {result.isValid ? (
                            <Alert
                                severity="success"
                                icon={<CheckCircle />}
                                sx={{ mb: 2 }}
                            >
                                <Typography variant="body1">
                                    Sadrzaj je validan i dobro formatiran!
                                </Typography>
                            </Alert>
                        ) : (
                            <Alert
                                severity="warning"
                                icon={<Warning />}
                                sx={{ mb: 2 }}
                            >
                                <Typography variant="body1">
                                    Pronadjeno je {result.issues?.length || 0}{' '}
                                    problema
                                </Typography>
                            </Alert>
                        )}

                        {result.issues && result.issues.length > 0 && (
                            <Box>
                                <Typography
                                    variant="subtitle2"
                                    color="error"
                                    sx={{ mb: 1 }}
                                >
                                    Problemi:
                                </Typography>
                                <List dense>
                                    {result.issues.map((issue, index) => (
                                        <ListItem key={index}>
                                            <ListItemIcon sx={{ minWidth: 36 }}>
                                                <ErrorIcon
                                                    color="error"
                                                    fontSize="small"
                                                />
                                            </ListItemIcon>
                                            <ListItemText primary={issue} />
                                        </ListItem>
                                    ))}
                                </List>
                            </Box>
                        )}

                        {result.suggestions &&
                            result.suggestions.length > 0 && (
                                <Box>
                                    <Typography
                                        variant="subtitle2"
                                        color="info.main"
                                        sx={{ mb: 1 }}
                                    >
                                        Predlozi za poboljsanje:
                                    </Typography>
                                    <List dense>
                                        {result.suggestions.map(
                                            (suggestion, index) => (
                                                <ListItem key={index}>
                                                    <ListItemIcon
                                                        sx={{ minWidth: 36 }}
                                                    >
                                                        <Lightbulb
                                                            color="info"
                                                            fontSize="small"
                                                        />
                                                    </ListItemIcon>
                                                    <ListItemText
                                                        primary={suggestion}
                                                    />
                                                </ListItem>
                                            )
                                        )}
                                    </List>
                                </Box>
                            )}

                        {result.suggestedValue && (
                            <>
                                <Divider />
                                <Box>
                                    <Typography
                                        variant="subtitle2"
                                        color="success.main"
                                        sx={{ mb: 1 }}
                                    >
                                        Predlozena izmena:
                                    </Typography>
                                    <Paper
                                        variant="outlined"
                                        sx={{
                                            p: 2,
                                            bgcolor: 'success.50',
                                            borderColor: 'success.200',
                                        }}
                                    >
                                        <Typography
                                            variant="body2"
                                            sx={{ whiteSpace: 'pre-wrap' }}
                                        >
                                            {result.suggestedValue}
                                        </Typography>
                                    </Paper>
                                </Box>
                            </>
                        )}

                        {result.originalValue && (
                            <Box>
                                <Typography
                                    variant="subtitle2"
                                    color="text.secondary"
                                    sx={{ mb: 1 }}
                                >
                                    Originalni sadrzaj:
                                </Typography>
                                <Paper
                                    variant="outlined"
                                    sx={{
                                        p: 2,
                                        bgcolor: 'grey.50',
                                        maxHeight: 150,
                                        overflow: 'auto',
                                    }}
                                >
                                    <Typography
                                        variant="body2"
                                        color="text.secondary"
                                        sx={{ whiteSpace: 'pre-wrap' }}
                                    >
                                        {result.originalValue}
                                    </Typography>
                                </Paper>
                            </Box>
                        )}
                    </Stack>
                )}
            </DialogContent>

            <DialogActions>
                <Button onClick={onClose} color="inherit">
                    Zatvori
                </Button>
                {result?.suggestedValue && (
                    <Button
                        variant="contained"
                        color="success"
                        onClick={() => {
                            onAccept(result.suggestedValue)
                            onClose()
                        }}
                    >
                        Prihvati predlog
                    </Button>
                )}
            </DialogActions>
        </Dialog>
    )
}

export default AiSuggestionDialog
