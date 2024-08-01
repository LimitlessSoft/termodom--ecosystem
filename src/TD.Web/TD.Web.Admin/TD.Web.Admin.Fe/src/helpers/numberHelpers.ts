export interface NumberFormatOptions {
    thousandSeparator: boolean
    decimalSeparator: boolean
    decimalCount: number
}

const defaultOptions: NumberFormatOptions = {
    thousandSeparator: true,
    decimalSeparator: true,
    decimalCount: 2,
}

export const formatNumber = (number: number, options?: NumberFormatOptions) => {
    if (options == null || options == undefined) options = defaultOptions

    return number
        ?.toFixed(options.decimalCount)
        .toString()
        .replace(/\B(?=(\d{3})+(?!\d))/g, ',')
}
