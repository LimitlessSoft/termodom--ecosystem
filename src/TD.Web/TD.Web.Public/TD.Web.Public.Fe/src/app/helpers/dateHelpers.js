import dayjs from 'dayjs'

const dateHelpers = {
    isValidDate: function (value) {
        return dayjs(value).isValid()
    },
    isAtLeastYearsOld: function (date, minAge = 18) {
        if (!this.isValidDate(date)) return false

        const birthDate = dayjs(date)
        const today = dayjs()
        const minDate = today.subtract(minAge, 'year')

        return birthDate.isBefore(minDate) || birthDate.isSame(minDate)
    },
    asUtcString: function (date) {
        return date?.toString() + 'Z'
    },
}

export default dateHelpers
