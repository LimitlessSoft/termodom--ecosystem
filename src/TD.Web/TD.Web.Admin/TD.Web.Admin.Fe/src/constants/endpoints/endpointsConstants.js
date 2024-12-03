export const ENDPOINTS_CONSTANTS = {
    PRODUCTS: {
        SEARCH_KEYWORDS: (productId) =>
            `/products/${productId}/search-keywords`,
    },
    MODULES_HELPS: {
        GET: (moduleType) => `/module-helps?module=${moduleType}`,
        PUT: `/module-helps`,
    },
}
