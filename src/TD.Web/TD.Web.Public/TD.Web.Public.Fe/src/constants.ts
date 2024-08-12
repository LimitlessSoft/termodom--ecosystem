export const COOKIES = {
    TOKEN: {
        NAME: 'token',
        DEFAULT_VALUE: undefined,
    },
}

export const PAGES = {
    ERROR: (status: number) => `/error?status=${status}`,
}
