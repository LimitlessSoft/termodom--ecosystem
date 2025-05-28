export const CookieNames = {
    cartId: 'cartId',
}

export const UIDimensions = {
    maxWidth: `1100px`,
}

export const DefaultMetadataTitle = `Gipsane ploče | Fasade | OSB Ploče | Cene | Termodom Online prodavnica`
export const DefaultMetadataDescription = `Gipsane ploče, stiropor, fasade, bavalit, azmafon, osb ploče | Akcija | Cene | Online Prodavnica | TERMODOM.RS | Centar građevinskog materijala`

export const KorpaTitle = `Korpa | Termodom`
export const OrderConclusionTitle = `Zaključi porudžbinu | Termodom`
export const KontaktTitle = `Kontakt | Radno vreme | Informacije | Termodom`
export const ProfiKutakTitle = `Profi kutak | Termodom`

export const AUTOCOMPLETE_NO_OPTIONS_MESSAGE = 'Nema pronadjenih rezultata'

export const ProizvodSrcSEOTitle = (product) =>
    removeMultipleSpaces(product.metaTitle) ||
    `${removeMultipleSpaces(product.title)} - Termodom`
export const ProizvodSrcSEODescription = (product) =>
    removeMultipleSpaces(product.metaDescription) ||
    removeMultipleSpaces(product.shortDescription)

export const removeMultipleSpaces = (str) => str.replace(/\s+/g, ' ').trim()
