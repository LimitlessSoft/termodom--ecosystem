const commonValidationMessages = {
    required: (field) => `${field} je obavezno polje.`,
    minLength: (field, length) =>
        `${field} mora imati najmanje ${length} karaktera.`,
    maxLength: (field, length) =>
        `${field} može imati najviše ${length} karaktera.`,
    email: `Email mora biti važeći.`,
    includesLetter: (field) => `${field} mora sadržati najmanje jedno slovo.`,
    includesDigit: (field) => `${field} mora sadržati najmanje jednu cifru.`,
    minYearsOld: (minYears) => `Mora imati najmanje ${minYears} godina.`,
    exactLength: (field, length) =>
        `${field} mora imati tačno ${length} karaktera.`,
}

export default commonValidationMessages
