export const CookieNames = {
    cartId: 'cartId',
}

export const UIDimensions = {
    maxWidth: `1100px`
}

export const DefaultMetadataTitle = `Gipsane ploče | Fasade | OSB Ploče | Cene | Termodom Online prodavnica`
export const DefaultMetadataDescription = `Gipsane ploče, stiropor, fasade, bavalit, azmafon, osb ploče | Akcija | Cene | Online Prodavnica | TERMODOM.RS | Centar građevinskog materijala`


export const KorpaTitle = `TERMODOM - Korpa`
export const KontaktTitle = `TERMODOM - Kontakt | Radno vreme | Informacije`
export const ProfiKutakTitle = `TERMODOM - Profi kutak`

export const ProizvodSrcTitle = (proizvodTitle: string) => `${removeMultipleSpaces(proizvodTitle)} - Termodom`
export const ProizvodSrcDescription = (proizvodShortDescription: string) => removeMultipleSpaces(proizvodShortDescription)

export const removeMultipleSpaces = (str: string) => str.replace(/\s+/g, ' ').trim()