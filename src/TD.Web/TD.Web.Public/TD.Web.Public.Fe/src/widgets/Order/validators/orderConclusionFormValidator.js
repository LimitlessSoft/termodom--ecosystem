import * as Yup from 'yup'
import commonValidationMessages from '@/validationMessages/commonValidationMessages'
import ORDER_CONSTANTS from '../constants'
import { yupResolver } from '@hookform/resolvers/yup'

const orderConclusionFormValidator = () => {
    const { VALIDATION_FIELDS } = ORDER_CONSTANTS

    const commonFields = {
        [VALIDATION_FIELDS.PICKUP_PLACE.FIELD]: Yup.number().required(
            commonValidationMessages.required(
                VALIDATION_FIELDS.PICKUP_PLACE.LABEL
            )
        ),
        [VALIDATION_FIELDS.FULL_NAME.FIELD]: Yup.string().required(
            commonValidationMessages.required(VALIDATION_FIELDS.FULL_NAME.LABEL)
        ),
        [VALIDATION_FIELDS.MOBILE.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(
                    VALIDATION_FIELDS.MOBILE.LABEL
                )
            )
            .min(
                9,
                commonValidationMessages.minLength(
                    VALIDATION_FIELDS.MOBILE.LABEL,
                    9
                )
            )
            .max(
                10,
                commonValidationMessages.maxLength(
                    VALIDATION_FIELDS.MOBILE.LABEL,
                    10
                )
            ),
        [VALIDATION_FIELDS.PAYMENT_TYPE.FIELD]: Yup.number().required(
            commonValidationMessages.required(
                VALIDATION_FIELDS.PAYMENT_TYPE.LABEL
            )
        ),
    }

    const deliveryFields = {
        [VALIDATION_FIELDS.DELIVERY_ADDRESS.FIELD]: Yup.string().when(
            VALIDATION_FIELDS.PICKUP_PLACE.FIELD,
            (value, schema) => {
                const storeId = Array.isArray(value) ? value[0] : value

                if (storeId === -5) {
                    return schema
                        .required(
                            commonValidationMessages.required(
                                VALIDATION_FIELDS.DELIVERY_ADDRESS.LABEL
                            )
                        )
                        .min(
                            5,
                            commonValidationMessages.minLength(
                                VALIDATION_FIELDS.DELIVERY_ADDRESS.LABEL,
                                5
                            )
                        )
                }
                return schema.notRequired()
            }
        ),
    }

    const wireTransferFields = {
        [VALIDATION_FIELDS.COMPANY.FIELD]: Yup.string().when(
            VALIDATION_FIELDS.PAYMENT_TYPE.FIELD,
            (val, schema) => {
                const paymentTypeId = Array.isArray(val) ? val[0] : val

                if (paymentTypeId === 6) {
                    return schema
                        .required(
                            commonValidationMessages.required(
                                VALIDATION_FIELDS.COMPANY.LABEL
                            )
                        )
                        .min(
                            9,
                            commonValidationMessages.minLength(
                                VALIDATION_FIELDS.COMPANY.LABEL,
                                9
                            )
                        )
                        .max(
                            13,
                            commonValidationMessages.maxLength(
                                VALIDATION_FIELDS.COMPANY.LABEL,
                                13
                            )
                        )
                }

                return schema.notRequired()
            }
        ),
    }

    return yupResolver(
        Yup.object().shape({
            ...commonFields,
            ...deliveryFields,
            ...wireTransferFields,
        })
    )
}

export default orderConclusionFormValidator
