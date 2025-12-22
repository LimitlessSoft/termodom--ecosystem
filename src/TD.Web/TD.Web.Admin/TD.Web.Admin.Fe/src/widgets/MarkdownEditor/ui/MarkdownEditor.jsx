import { Box, Tab, Tabs, TextField, Paper, Typography } from '@mui/material'
import { useState } from 'react'
import ReactMarkdown from 'react-markdown'

export const MarkdownEditor = ({
    value,
    onChange,
    label = 'Tekst',
    rows = 15,
    required = false,
    placeholder = 'Unesite tekst u Markdown formatu...',
}) => {
    const [activeTab, setActiveTab] = useState(0)

    const handleTabChange = (event, newValue) => {
        setActiveTab(newValue)
    }

    return (
        <Box sx={{ width: '100%' }}>
            <Paper variant="outlined" sx={{ overflow: 'hidden' }}>
                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                    <Tabs value={activeTab} onChange={handleTabChange}>
                        <Tab label="Izmena" />
                        <Tab label="Pregled" />
                    </Tabs>
                </Box>

                {activeTab === 0 && (
                    <Box sx={{ p: 0 }}>
                        <TextField
                            fullWidth
                            multiline
                            rows={rows}
                            value={value}
                            onChange={(e) => onChange(e.target.value)}
                            placeholder={placeholder}
                            required={required}
                            variant="outlined"
                            sx={{
                                '& .MuiOutlinedInput-root': {
                                    borderRadius: 0,
                                    '& fieldset': {
                                        border: 'none',
                                    },
                                },
                            }}
                        />
                    </Box>
                )}

                {activeTab === 1 && (
                    <Box
                        sx={{
                            p: 2,
                            minHeight: rows * 24,
                            maxHeight: rows * 24 + 100,
                            overflow: 'auto',
                        }}
                    >
                        {value ? (
                            <Box
                                sx={{
                                    '& h1': { fontSize: '2rem', fontWeight: 'bold', my: 2 },
                                    '& h2': { fontSize: '1.5rem', fontWeight: 'bold', my: 2 },
                                    '& h3': { fontSize: '1.25rem', fontWeight: 'bold', my: 1.5 },
                                    '& h4': { fontSize: '1.1rem', fontWeight: 'bold', my: 1 },
                                    '& p': { my: 1, lineHeight: 1.7 },
                                    '& ul, & ol': { pl: 3, my: 1 },
                                    '& li': { my: 0.5 },
                                    '& blockquote': {
                                        borderLeft: 4,
                                        borderColor: 'primary.main',
                                        pl: 2,
                                        my: 2,
                                        color: 'text.secondary',
                                        fontStyle: 'italic',
                                    },
                                    '& code': {
                                        backgroundColor: 'grey.100',
                                        px: 0.5,
                                        py: 0.25,
                                        borderRadius: 1,
                                        fontFamily: 'monospace',
                                    },
                                    '& pre': {
                                        backgroundColor: 'grey.100',
                                        p: 2,
                                        borderRadius: 1,
                                        overflow: 'auto',
                                        '& code': {
                                            backgroundColor: 'transparent',
                                            p: 0,
                                        },
                                    },
                                    '& a': {
                                        color: 'primary.main',
                                        textDecoration: 'underline',
                                    },
                                    '& img': {
                                        maxWidth: '100%',
                                        height: 'auto',
                                    },
                                    '& hr': {
                                        my: 2,
                                        border: 'none',
                                        borderTop: 1,
                                        borderColor: 'divider',
                                    },
                                    '& table': {
                                        borderCollapse: 'collapse',
                                        width: '100%',
                                        my: 2,
                                    },
                                    '& th, & td': {
                                        border: 1,
                                        borderColor: 'divider',
                                        p: 1,
                                        textAlign: 'left',
                                    },
                                    '& th': {
                                        backgroundColor: 'grey.100',
                                        fontWeight: 'bold',
                                    },
                                }}
                            >
                                <ReactMarkdown>{value}</ReactMarkdown>
                            </Box>
                        ) : (
                            <Typography color="text.secondary" sx={{ fontStyle: 'italic' }}>
                                Nema teksta za prikaz
                            </Typography>
                        )}
                    </Box>
                )}
            </Paper>

            <Typography variant="caption" color="text.secondary" sx={{ mt: 1, display: 'block' }}>
                Podrzani Markdown: **bold**, *italic*, # naslovi, - liste, [linkovi](url), `kod`
            </Typography>
        </Box>
    )
}
