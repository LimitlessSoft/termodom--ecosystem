export const CookieNames = {
    cartId: 'cartId',
}

export const UIDimensions = {
    maxWidth: `1100px`,
}

export const DefaultMetadataTitle = `Gipsane ploče | Fasade | OSB Ploče | Cene | Termodom Online prodavnica`
export const DefaultMetadataDescription = `Gipsane ploče, stiropor, fasade, bavalit, azmafon, osb ploče | Akcija | Cene | Online Prodavnica | TERMODOM.RS | Centar građevinskog materijala`

export const KorpaTitle = `Korpa | Termodom`
export const KontaktTitle = `Kontakt | Radno vreme | Informacije | Termodom`
export const ProfiKutakTitle = `Profi kutak | Termodom`

export const ProizvodSrcTitle = (proizvodTitle: string) =>
    `${removeMultipleSpaces(proizvodTitle)} - Termodom`
export const ProizvodSrcDescription = (proizvodShortDescription: string) =>
    removeMultipleSpaces(proizvodShortDescription)

export const removeMultipleSpaces = (str: string) =>
    str.replace(/\s+/g, ' ').trim()
