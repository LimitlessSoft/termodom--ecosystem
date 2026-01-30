export interface IPageSeoResult {
    url: string
    fetchTimeMs: number
    statusCode: number
    title: {
        content: string | null
        length: number
    }
    metaDescription: {
        content: string | null
        length: number
    }
    canonical: string | null
    robots: string | null
    viewport: string | null
    openGraph: {
        title: string | null
        description: string | null
        image: string | null
    }
    headings: {
        h1: string[]
        h2Count: number
        h3Count: number
    }
    images: {
        total: number
        withAlt: number
        withoutAlt: number
    }
    links: {
        internal: number
        external: number
    }
    hasStructuredData: boolean
    wordCount: number
    score: number
    issues: { severity: 'error' | 'warning' | 'info'; message: string }[]
}

export interface ISiteAnalysisResult {
    baseUrl: string
    totalPages: number
    analyzedAt: string
    totalTimeMs: number
    averageScore: number
    pages: IPageSeoResult[]
    summary: {
        pagesWithoutTitle: number
        pagesWithoutDescription: number
        pagesWithoutH1: number
        pagesWithoutCanonical: number
        pagesWithoutViewport: number
        pagesWithoutOg: number
        pagesWithoutStructuredData: number
        imagesWithoutAlt: number
        totalImages: number
        totalInternalLinks: number
        totalExternalLinks: number
    }
    commonIssues: { issue: string; count: number; severity: 'error' | 'warning' | 'info' }[]
}
