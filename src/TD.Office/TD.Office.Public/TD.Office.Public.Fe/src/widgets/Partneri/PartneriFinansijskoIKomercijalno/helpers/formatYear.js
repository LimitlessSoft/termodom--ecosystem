export const formatYear = (year) => {
    let formattedYearValue

    if (typeof year === 'number') {
        formattedYearValue = year.toString()
    } else {
        formattedYearValue = year
    }

    const yearParts = formattedYearValue.split(' ')

    return yearParts.length > 1 ? yearParts[1] : yearParts[0]
}
