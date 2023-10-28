import { createTheme } from "@mui/material/styles"

declare module '@mui/material/styles' {
    interface Theme {
        dataBackground?: {
            primary: string,
            primaryHover: string,
            secondary: string,
            secondaryHover: string
        },
        defaultPagination: {
            options: number[],
            default: number
        }
    }

    interface ThemeOptions {
        dataBackground?: {
            primary: string,
            primaryHover: string,
            secondary: string,
            secondaryHover: string
        },
        defaultPagination?: {
            options: number[],
            default: number
        }
    }
}

export const mainTheme = createTheme({
    typography: {
        fontFamily: [
          '-apple-system',
          'BlinkMacSystemFont',
          '"Segoe UI"',
          'Roboto',
          '"Helvetica Neue"',
          'Arial',
          'sans-serif',
          '"Apple Color Emoji"',
          '"Segoe UI Emoji"',
          '"Segoe UI Symbol"',
        ].join(','),
    },
    palette: {
        primary: {
            main: '#ff5b5b',
            light: '#ff7070',
            dark: '#ff4747',
            contrastText: '#fbfffe',
        },
        secondary: {
            main: '#007bff',
            light: '#1f8bff',
            dark: '##0076F5',
            contrastText: '#a38560',
        },
        error: {
            main: '#ff3333'
        },
        warning: {
            main: '#ff875a'
        },
        info: {
            main: '#fde56f'
        },
        success: {
            main: '#009944'
        }
    }
}, {
    dataBackground: {
        primary: '#ffffff',
        primaryHover: '#fffeee',
        secondary: '#eeeeee',
        secondaryHover: '#eeefff',
    },
    defaultPagination: {
        options: [10, 50, 100],
        default: 10
    }
})