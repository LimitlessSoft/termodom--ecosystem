const defaultOptions = {
    thousandSeparator: true,
    decimalSeparator: true,
    decimalCount: 2,
}

const numbersHelpers = {
    formatNumber: (number, options) => {
        if (!options) options = defaultOptions

        if (typeof number === 'string') {
            number = +number
        }

        if (isNaN(number)) {
            return 'Invalid number'
        }

        return number
            .toFixed(options.decimalCount)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, ',')
    },
}

export default numbersHelpers
