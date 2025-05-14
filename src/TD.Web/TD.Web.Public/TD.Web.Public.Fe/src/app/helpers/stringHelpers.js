const stringHelpers = {
    isString: function (value) {
        return typeof value === 'string'
    },
    includesLetter: function (value) {
        return this.isString(value) && /[a-zA-Z]/.test(value)
    },
    includesDigit: function (value) {
        return this.isString(value) && /[0-9]/.test(value)
    },
}

export default stringHelpers
