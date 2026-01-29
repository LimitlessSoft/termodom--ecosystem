import { createTheme } from '@mui/material/styles'

declare module '@mui/material/styles' {
    interface Theme {
        dataBackground?: {
            primary: string
            primaryHover: string
            secondary: string
            secondaryHover: string
        }
        defaultPagination: {
            options: number[]
            default: number
        }
    }

    interface ThemeOptions {
        dataBackground?: {
            primary: string
            primaryHover: string
            secondary: string
            secondaryHover: string
        }
        defaultPagination?: {
            options: number[]
            default: number
        }
    }
}

export const mainTheme = createTheme(
    {
        palette: {
            primary: {
                main: '#e12121',
                light: '#ff7070',
                dark: '#ff4747',
                contrastText: '#fbfffe',
            },
            secondary: {
                main: '#007bff',
                light: '#1f8bff',
                dark: '#0076F5',
                contrastText: '#fff',
            },
            error: {
                main: '#ff3333',
            },
            warning: {
                main: '#ff875a',
            },
            info: {
                main: '#fde56f',
            },
            success: {
                main: '#009944',
            },
            background: {
                default: '#f0f2f5',
                paper: '#ffffff',
            },
            divider: '#d0d7de',
        },
        components: {
            MuiPaper: {
                styleOverrides: {
                    root: {
                        border: '1px solid #d0d7de',
                        boxShadow: '0 1px 3px rgba(0, 0, 0, 0.08)',
                    },
                    elevation1: {
                        boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
                    },
                    elevation2: {
                        boxShadow: '0 2px 6px rgba(0, 0, 0, 0.12)',
                    },
                },
            },
            MuiCard: {
                styleOverrides: {
                    root: {
                        border: '1px solid #d0d7de',
                        boxShadow: '0 1px 4px rgba(0, 0, 0, 0.1)',
                    },
                },
            },
            MuiTextField: {
                defaultProps: {
                    variant: 'outlined',
                },
                styleOverrides: {
                    root: {
                        '& .MuiOutlinedInput-root': {
                            backgroundColor: '#ffffff',
                            '& fieldset': {
                                borderColor: '#c0c8d0',
                                borderWidth: '1px',
                            },
                            '&:hover fieldset': {
                                borderColor: '#8090a0',
                            },
                            '&.Mui-focused fieldset': {
                                borderColor: '#007bff',
                                borderWidth: '2px',
                            },
                        },
                    },
                },
            },
            MuiOutlinedInput: {
                styleOverrides: {
                    root: {
                        backgroundColor: '#ffffff',
                        '& .MuiOutlinedInput-notchedOutline': {
                            borderColor: '#c0c8d0',
                        },
                        '&:hover .MuiOutlinedInput-notchedOutline': {
                            borderColor: '#8090a0',
                        },
                        '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                            borderColor: '#007bff',
                        },
                    },
                },
            },
            MuiSelect: {
                styleOverrides: {
                    root: {
                        backgroundColor: '#ffffff',
                    },
                },
            },
            MuiButton: {
                styleOverrides: {
                    root: {
                        textTransform: 'none',
                        fontWeight: 500,
                        boxShadow: '0 1px 2px rgba(0, 0, 0, 0.1)',
                    },
                    contained: {
                        boxShadow: '0 1px 3px rgba(0, 0, 0, 0.15)',
                        '&:hover': {
                            boxShadow: '0 2px 4px rgba(0, 0, 0, 0.2)',
                        },
                    },
                    outlined: {
                        borderWidth: '1px',
                        '&:hover': {
                            borderWidth: '1px',
                        },
                    },
                },
            },
            MuiTableContainer: {
                styleOverrides: {
                    root: {
                        border: '1px solid #d0d7de',
                        borderRadius: '4px',
                    },
                },
            },
            MuiTableHead: {
                styleOverrides: {
                    root: {
                        backgroundColor: '#f6f8fa',
                        '& .MuiTableCell-head': {
                            fontWeight: 600,
                            borderBottom: '2px solid #d0d7de',
                        },
                    },
                },
            },
            MuiTableCell: {
                styleOverrides: {
                    root: {
                        borderBottom: '1px solid #e1e4e8',
                    },
                },
            },
            MuiTabs: {
                styleOverrides: {
                    root: {
                        borderBottom: '1px solid #d0d7de',
                    },
                },
            },
            MuiTab: {
                styleOverrides: {
                    root: {
                        textTransform: 'none',
                        fontWeight: 500,
                    },
                },
            },
            MuiChip: {
                styleOverrides: {
                    root: {
                        fontWeight: 500,
                    },
                    outlined: {
                        borderColor: '#c0c8d0',
                    },
                },
            },
            MuiAccordion: {
                styleOverrides: {
                    root: {
                        border: '1px solid #d0d7de',
                        '&:before': {
                            display: 'none',
                        },
                        '&.Mui-expanded': {
                            margin: 0,
                        },
                    },
                },
            },
            MuiAccordionSummary: {
                styleOverrides: {
                    root: {
                        backgroundColor: '#f6f8fa',
                        borderBottom: '1px solid #d0d7de',
                        '&.Mui-expanded': {
                            minHeight: 48,
                        },
                    },
                    content: {
                        '&.Mui-expanded': {
                            margin: '12px 0',
                        },
                    },
                },
            },
        },
    },
    {
        dataBackground: {
            primary: '#ffffff',
            primaryHover: '#f6f8fa',
            secondary: '#f0f2f5',
            secondaryHover: '#e8ebef',
        },
        defaultPagination: {
            options: [10, 50, 100],
            default: 10,
        },
    }
)
