export const getUkupnoGotovine = (specifikacijaNovca) =>
    specifikacijaNovca?.specifikacijaNovca.novcanice.reduce(
        (prevNovcanica, currentNovcanica) =>
            prevNovcanica + currentNovcanica.value * currentNovcanica.key,
        0
    ) ?? 0

const encPow = 2
const encKey = 1000
export const encryptSpecifikacijaNovcaId = (id) => {
    if (!id) return null
    return id + Math.pow(encKey, encPow)
}

export const decryptSpecifikacijaNovcaId = (encryptedId) => {
    if (!encryptedId) return null
    return encryptedId - Math.pow(encKey, encPow)
}
