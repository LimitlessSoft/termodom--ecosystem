export const requiredMessage = (field) => `${field} je obavezno polje.`

export const minLengthMessage = (field, min) =>
    `${field} mora imati najmanje ${min} karaktera.`

export const maxLengthMessage = (field, max) =>
    `${field} najvise moze da ima ${max} karaktera.`

export const emailMessage = () => `Email adresa nije validna.`
