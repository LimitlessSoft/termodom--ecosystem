export const otpremniceHelpers = {
    types: {
        MP: 'Interni MP',
        VP: 'Interni VP',
    },
    magaciniVrste: (otpremnicaType) => {
        if (otpremnicaType === otpremniceHelpers.types.MP) {
            return [2]
        } else {
            return [1]
        }
    },
}
