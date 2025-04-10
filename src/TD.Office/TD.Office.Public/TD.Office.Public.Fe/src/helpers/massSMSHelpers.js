export const massSMSHelpers = {
    formatStatus: (status) => {
        switch (status) {
            case 'Initial':
                return 'U pripremi'
            default:
                return 'Nepoznat status'
        }
    },
}
