import { createTheme } from "@mui/material/styles"

declare module '@mui/material/styles' {
    interface Theme {
        fontSizes?: {
            _260: string,
            _360: string,
            _520: string,
            _720: string,
            _960: string,
            _1336: string,
            _1600: string,
            _1920: string,
        },
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
        fontSizes?: {
            _260: string,
            _360: string,
            _520: string,
            _720: string,
            _960: string,
            _1336: string,
            _1600: string,
            _1920: string,
        },
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
        fontFamily: "GothamProRegular",
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
            contrastText: '#fbfffe',
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
    fontSizes: {
        _260: `0.7rem`,
        _360: `0.8rem`,
        _520: `0.8rem`,
        _720: `0.8rem`,
        _960: `0.8rem`,
        _1336: `1rem`,
        _1600: `1rem`,
        _1920: `1rem`,
    },
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