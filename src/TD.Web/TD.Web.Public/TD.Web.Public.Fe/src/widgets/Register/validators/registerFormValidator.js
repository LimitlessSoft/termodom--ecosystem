import * as Yup from 'yup'
import stringHelpers from '@/app/helpers/stringHelpers'
import dateHelpers from '@/app/helpers/dateHelpers'
import commonValidationMessages from '@/validationMessages/commonValidationMessages'
import REGISTER_CONSTANTS from '../constants'
import registerFormValidationMessages from '../validationMessages/registerFormValidationMessages'
import { yupResolver } from '@hookform/resolvers/yup'

const registerFormValidator = (isIndividual) => {
    const { VALIDATION_FIELDS } = REGISTER_CONSTANTS

    const commonFields = {
        [VALIDATION_FIELDS.USERNAME.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(
                    VALIDATION_FIELDS.USERNAME.LABEL
                )
            )
            .min(
                6,
                commonValidationMessages.minLength(
                    VALIDATION_FIELDS.USERNAME.LABEL,
                    6
                )
            )
            .max(
                32,
                commonValidationMessages.maxLength(
                    VALIDATION_FIELDS.USERNAME.LABEL,
                    32
                )
            ),
        [VALIDATION_FIELDS.PASSWORD.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(
                    VALIDATION_FIELDS.PASSWORD.LABEL
                )
            )
            .min(
                8,
                commonValidationMessages.minLength(
                    VALIDATION_FIELDS.PASSWORD.LABEL,
                    8
                )
            )
            .test(
                'includes-letter',
                commonValidationMessages.includesLetter(
                    VALIDATION_FIELDS.PASSWORD.LABEL
                ),
                (value) => stringHelpers.includesLetter(value)
            )
            .test(
                'includes-digit',
                commonValidationMessages.includesDigit(
                    VALIDATION_FIELDS.PASSWORD.LABEL
                ),
                (value) => stringHelpers.includesDigit(value)
            ),
        [VALIDATION_FIELDS.CONFIRM_PASSWORD.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(
                    VALIDATION_FIELDS.CONFIRM_PASSWORD.LABEL
                )
            )
            .oneOf(
                [Yup.ref(VALIDATION_FIELDS.PASSWORD.FIELD)],
                registerFormValidationMessages.passwordsDontMatch
            ),
        [VALIDATION_FIELDS.DATE_OF_BIRTH.FIELD]: Yup.date()
            .required(
                commonValidationMessages.required(
                    VALIDATION_FIELDS.DATE_OF_BIRTH.LABEL
                )
            )
            .test(
                'is-18',
                commonValidationMessages.minYearsOld(18),
                (value) => {
                    if (!value) return false

                    return dateHelpers.isAtLeastYearsOld(value, 18)
                }
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
        [VALIDATION_FIELDS.ADDRESS.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(
                    VALIDATION_FIELDS.ADDRESS.LABEL
                )
            )
            .min(
                5,
                commonValidationMessages.minLength(
                    VALIDATION_FIELDS.ADDRESS.LABEL,
                    5
                )
            ),
        [VALIDATION_FIELDS.CITY.FIELD]: Yup.number().required(
            commonValidationMessages.required(VALIDATION_FIELDS.CITY.LABEL)
        ),

        [VALIDATION_FIELDS.FAVORITE_STORE.FIELD]: Yup.number().required(
            commonValidationMessages.required(
                VALIDATION_FIELDS.FAVORITE_STORE.LABEL
            )
        ),

        [VALIDATION_FIELDS.MAIL.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(VALIDATION_FIELDS.MAIL.LABEL)
            )
            .min(
                5,
                commonValidationMessages.minLength(
                    VALIDATION_FIELDS.MAIL.LABEL,
                    5
                )
            )
            .email(commonValidationMessages.email),
    }

    const individualFields = {
        [VALIDATION_FIELDS.NICKNAME.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(
                    VALIDATION_FIELDS.NICKNAME.LABEL
                )
            )
            .min(
                6,
                commonValidationMessages.minLength(
                    VALIDATION_FIELDS.NICKNAME.LABEL,
                    6
                )
            )
            .max(
                32,
                commonValidationMessages.maxLength(
                    VALIDATION_FIELDS.NICKNAME.LABEL,
                    32
                )
            ),
    }

    const companyFields = {
        [VALIDATION_FIELDS.COMPANY.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(
                    VALIDATION_FIELDS.COMPANY.LABEL
                )
            )
            .min(
                2,
                commonValidationMessages.minLength(
                    VALIDATION_FIELDS.COMPANY.LABEL,
                    2
                )
            )
            .max(
                32,
                commonValidationMessages.maxLength(
                    VALIDATION_FIELDS.COMPANY.LABEL,
                    32
                )
            ),
        [VALIDATION_FIELDS.PIB.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(VALIDATION_FIELDS.PIB.LABEL)
            )
            .matches(
                /^\d{9}$/,
                commonValidationMessages.exactLength(
                    VALIDATION_FIELDS.PIB.LABEL,
                    9
                )
            ),
        [VALIDATION_FIELDS.MB.FIELD]: Yup.string()
            .required(
                commonValidationMessages.required(VALIDATION_FIELDS.MB.LABEL)
            )
            .matches(
                /^\d{13}$/,
                commonValidationMessages.exactLength(
                    VALIDATION_FIELDS.MB.LABEL,
                    13
                )
            ),
    }

    return yupResolver(
        Yup.object().shape({
            ...(isIndividual ? individualFields : companyFields),
            ...commonFields,
        })
    )
}

export default registerFormValidator
