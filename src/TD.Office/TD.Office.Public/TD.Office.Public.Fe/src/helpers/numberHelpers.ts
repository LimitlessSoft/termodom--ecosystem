import { FORMAT_NUMBER_CONSTANTS } from '@/constants'

export interface NumberFormatOptions {
    thousandSeparator: boolean
    decimalSeparator: boolean
    decimalCount: number
}

const defaultOptions: NumberFormatOptions = {
    thousandSeparator: true,
    decimalSeparator: true,
    decimalCount: FORMAT_NUMBER_CONSTANTS.FORMAT_NUMBER_DECIMAL_COUNT,
}

export const formatNumber = (number: number, options?: NumberFormatOptions) => {
    if (options == null || options == undefined) options = defaultOptions

    return number
        ?.toFixed(options.decimalCount)
        .toString()
        .replace(/\B(?=(\d{3})+(?!\d))/g, ',')
}
