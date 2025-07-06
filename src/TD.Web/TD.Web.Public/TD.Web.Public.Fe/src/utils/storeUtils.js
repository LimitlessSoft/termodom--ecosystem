import { STORE_CONSTANTS } from '@/constants'

export const isDeliveryPickupPlace = (value) =>
    +value === STORE_CONSTANTS.DELIVERY_PICKUP_PLACE_ID
