import { DATE_CONSTANTS } from '@/constants'
import dayjs from 'dayjs'

export const asUtcString = (date) => {
    return date?.toString() + 'Z'
}
