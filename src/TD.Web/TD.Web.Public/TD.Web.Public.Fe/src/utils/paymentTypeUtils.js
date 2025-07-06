import { PAYMENT_TYPE_CONSTANTS } from '@/constants'

export const isWireTransferPaymentType = (value) =>
    +value === PAYMENT_TYPE_CONSTANTS.WIRE_TRANSFER_PAYMENT_TYPE_ID
