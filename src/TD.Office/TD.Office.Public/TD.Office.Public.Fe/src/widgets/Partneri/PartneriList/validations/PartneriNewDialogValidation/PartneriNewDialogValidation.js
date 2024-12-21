import * as Yup from 'yup'
import {
    requiredMessage,
    minLengthMessage,
    emailMessage,
} from '@/validationCodes/commonValidationCodes'
import { PARTNERI_NEW } from '../../constants'

const { VALIDATION_FIELDS } = PARTNERI_NEW

export const PartneriNewDialogValidation = Yup.object().shape({
    [VALIDATION_FIELDS.NAME.FIELD]: Yup.string()
        .min(5, minLengthMessage(VALIDATION_FIELDS.NAME.FIELD, 5))
        .required(requiredMessage(VALIDATION_FIELDS.NAME.FIELD)),
    [VALIDATION_FIELDS.ADDRESS.FIELD]: Yup.string()
        .min(5, minLengthMessage(VALIDATION_FIELDS.ADDRESS.FIELD, 5))
        .required(requiredMessage(VALIDATION_FIELDS.ADDRESS.FIELD)),
    [VALIDATION_FIELDS.POSTAL_CODE.FIELD]: Yup.string()
        .min(5, minLengthMessage(VALIDATION_FIELDS.POSTAL_CODE.FIELD, 5))
        .required(requiredMessage(VALIDATION_FIELDS.POSTAL_CODE.FIELD)),
    [VALIDATION_FIELDS.CITY.FIELD]: Yup.string()
        .min(2, minLengthMessage(VALIDATION_FIELDS.CITY.FIELD, 2))
        .required(requiredMessage(VALIDATION_FIELDS.CITY.FIELD)),
    [VALIDATION_FIELDS.PLACE.FIELD]: Yup.string()
        .min(5, minLengthMessage(VALIDATION_FIELDS.PLACE.FIELD, 5))
        .required(requiredMessage(VALIDATION_FIELDS.PLACE.FIELD)),
    [VALIDATION_FIELDS.EMAIL.FIELD]: Yup.string()
        .email(emailMessage)
        .required(requiredMessage(VALIDATION_FIELDS.EMAIL.FIELD)),
    [VALIDATION_FIELDS.CONTACT.FIELD]: Yup.string()
        .min(5, minLengthMessage(VALIDATION_FIELDS.CONTACT.FIELD, 5))
        .required(requiredMessage(VALIDATION_FIELDS.CONTACT.FIELD)),
    [VALIDATION_FIELDS.JMBG.FIELD]: Yup.string()
        .min(13, minLengthMessage(VALIDATION_FIELDS.JMBG.FIELD, 13))
        .required(requiredMessage(VALIDATION_FIELDS.JMBG.FIELD)),
    [VALIDATION_FIELDS.PIB.FIELD]: Yup.string()
        .min(9, minLengthMessage(VALIDATION_FIELDS.PIB.FIELD, 9))
        .required(requiredMessage(VALIDATION_FIELDS.PIB.FIELD)),
    [VALIDATION_FIELDS.IN_PDV_SYSTEM.FIELD]: Yup.boolean().required(),
    [VALIDATION_FIELDS.MOBILE.FIELD]: Yup.string()
        .min(9, minLengthMessage(VALIDATION_FIELDS.MOBILE.FIELD, 5))
        .required(requiredMessage(VALIDATION_FIELDS.MOBILE.FIELD)),
    [VALIDATION_FIELDS.CATEGORY.FIELD]: Yup.number()
        .min(1, '')
        .required(requiredMessage(VALIDATION_FIELDS.CATEGORY.FIELD)),
})
