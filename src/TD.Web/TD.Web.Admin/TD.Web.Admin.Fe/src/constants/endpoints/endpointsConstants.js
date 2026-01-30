export const ENDPOINTS_CONSTANTS = {
    PRODUCTS: {
        SEARCH_KEYWORDS: (productId) =>
            `/products/${productId}/search-keywords`,
        AI_VALIDATE_NAME: (id) => `/products/${id}/ai/validate/name`,
        AI_VALIDATE_DESCRIPTION: (id) => `/products/${id}/ai/validate/description`,
        AI_VALIDATE_SHORT_DESCRIPTION: (id) => `/products/${id}/ai/validate/short-description`,
        AI_VALIDATE_META: (id) => `/products/${id}/ai/validate/meta`,
        AI_GENERATE_NAME: (id) => `/products/${id}/ai/generate/name`,
        AI_GENERATE_DESCRIPTION: (id) => `/products/${id}/ai/generate/description`,
        AI_GENERATE_SHORT_DESCRIPTION: (id) => `/products/${id}/ai/generate/short-description`,
        AI_GENERATE_META: (id) => `/products/${id}/ai/generate/meta`,
    },
    MODULES_HELPS: {
        GET: (moduleType) => `/module-helps?module=${moduleType}`,
        PUT: `/module-helps`,
    },
}
