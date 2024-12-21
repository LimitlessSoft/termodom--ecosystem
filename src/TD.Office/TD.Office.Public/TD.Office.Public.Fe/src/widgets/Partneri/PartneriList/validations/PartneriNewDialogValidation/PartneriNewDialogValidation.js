import * as Yup from 'yup'
import {
    requiredMessage,
    minLengthMessage,
    emailMessage,
} from '@/validationCodes/commonValidationCodes'

export const PartneriNewDialogValidation = Yup.object({
    Naziv: Yup.string()
        .min(5, minLengthMessage('Naziv', 5))
        .required(requiredMessage('Naziv')),
    Adresa: Yup.string()
        .min(5, minLengthMessage('Adresa', 5))
        .required(requiredMessage('Adresa')),
    'Postanski broj': Yup.string()
        .min(5, minLengthMessage('Postanski broj', 5))
        .required(requiredMessage('Postanski broj')),
    Grad: Yup.string()
        .min(2, minLengthMessage('Grad', 2))
        .required(requiredMessage('Grad')),
    Mesto: Yup.string()
        .min(5, minLengthMessage('Mesto', 5))
        .required(requiredMessage('Mesto')),
    Email: Yup.string().email(emailMessage).required(requiredMessage('Email')),
    Kontakt: Yup.string()
        .min(5, minLengthMessage('Kontakt', 5))
        .required(requiredMessage('Kontakt')),
    'Maticni broj': Yup.string()
        .min(13, minLengthMessage('Maticni broj', 13))
        .required(requiredMessage('Maticni broj')),
    PIB: Yup.string()
        .min(9, minLengthMessage('PIB', 9))
        .required(requiredMessage('PIB')),
    'U PDV Sistemu': Yup.boolean().required(),
    Mobilni: Yup.string()
        .min(9, minLengthMessage('Mobilni', 5))
        .required(requiredMessage('Mobilni')),
    // Kategorija: Yup.number()
    //     .required(requiredMessage('Kategorija'))
    //     .test(
    //         'is-valid',
    //         'Kategorija mora biti veća od 0 i izabrano je najmanje ${min} grupa.',
    //         function (value) {
    //             const { groupsChecked } = this.options.context // Access groupsChecked from context
    //             return (
    //                 (value > 0 &&
    //                     groupsChecked >= PARTNERI_NEW_MIN_GROUPS_CHECKED) ||
    //                 this.createError({ message: 'Kategorija nije validna.' })
    //             )
    //         }
    //     ),
})
