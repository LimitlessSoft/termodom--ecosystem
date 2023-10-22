import { createTheme } from "@mui/material/styles"

declare module '@mui/material/styles' {
    interface Theme {
        dataBackground?: {
            primary: string,
            primaryHover: string,
            secondary: string,
            secondaryHover: string
        }
    }

    interface ThemeOptions {
        dataBackground?: {
            primary: string,
            primaryHover: string,
            secondary: string,
            secondaryHover: string
        }
    }
}

export const mainTheme = createTheme({
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
    }
})