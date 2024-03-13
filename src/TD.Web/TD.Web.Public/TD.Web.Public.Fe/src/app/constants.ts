export const CookieNames = {
    cartId: 'cartId',
}

export const UIDimensions = {
    maxWidth: `1100px`
}

export const DefaultMetadataTitle = `TERMODOM - Gips ploče | Fasade | Suva gradnja | Izolacija | Cene`
export const DefaultMetadataDescription = `Termodom Online prodavnica. Gips karton ploče, stiropor, fasade, bavalit, azmafon, stirodur - TERMODOM.RS - Centar građevinskog materijala.`


export const KorpaTitle = `TERMODOM - Korpa`
export const KontaktTitle = `TERMODOM - Kontakt | Radno vreme | Informacije`
export const ProfiKutakTitle = `TERMODOM - Profi kutak`

export const ProizvodSrcTitle = (proizvodTitle: string) => `TERMODOM - ${removeMultipleSpaces(proizvodTitle)}`
export const ProizvodSrcDescription = (proizvodShortDescription: string) => removeMultipleSpaces(proizvodShortDescription)

export const removeMultipleSpaces = (str: string) => str.replace(/\s+/g, ' ').trim()