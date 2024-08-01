export const getClassificationColor = (classification: number) => {
    const hobiBorderColor = 'gray'
    const standardBorderColor = 'green'
    const profiBorderColor = 'orange'

    switch (classification) {
        case 0:
            return hobiBorderColor
        case 2:
            return profiBorderColor
        default:
            return standardBorderColor
    }
}
