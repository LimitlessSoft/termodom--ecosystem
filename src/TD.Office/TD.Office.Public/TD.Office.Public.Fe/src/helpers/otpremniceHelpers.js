export const otpremniceHelpers = {
    types: {
        MP: 'Interne MP',
        VP: 'Interne VP',
    },
    magaciniVrste: (otpremnicaType) => {
        if (otpremnicaType === otpremniceHelpers.types.MP) {
            return [2]
        } else {
            return [1]
        }
    },
}