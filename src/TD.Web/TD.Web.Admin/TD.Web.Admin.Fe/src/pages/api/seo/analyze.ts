import type { NextApiRequest, NextApiResponse } from 'next'

const BASE_URL = 'https://termodom.rs'
const MAX_PAGES = 10000

interface PageSeoResult {
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

export interface SiteAnalysisResult {
    baseUrl: string
    totalPages: number
    analyzedAt: string
    totalTimeMs: number
    averageScore: number
    pages: PageSeoResult[]
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
    commonIssues: {
        issue: string
        count: number
        severity: 'error' | 'warning' | 'info'
    }[]
}

function extractMetaContent(html: string, name: string): string | null {
    const patterns = [
        new RegExp(
            `<meta[^>]+name=["']${name}["'][^>]+content=["']([^"']*)["']`,
            'i'
        ),
        new RegExp(
            `<meta[^>]+content=["']([^"']*?)["'][^>]+name=["']${name}["']`,
            'i'
        ),
    ]
    for (const pattern of patterns) {
        const match = html.match(pattern)
        if (match) return match[1]
    }
    return null
}

function extractMetaProperty(html: string, property: string): string | null {
    const patterns = [
        new RegExp(
            `<meta[^>]+property=["']${property}["'][^>]+content=["']([^"']*)["']`,
            'i'
        ),
        new RegExp(
            `<meta[^>]+content=["']([^"']*?)["'][^>]+property=["']${property}["']`,
            'i'
        ),
    ]
    for (const pattern of patterns) {
        const match = html.match(pattern)
        if (match) return match[1]
    }
    return null
}

function extractTitle(html: string): string | null {
    const match = html.match(/<title[^>]*>([^<]*)<\/title>/i)
    return match ? match[1].trim() : null
}

function extractHeadings(html: string, tag: string): string[] {
    const regex = new RegExp(`<${tag}[^>]*>([^<]*)</${tag}>`, 'gi')
    const headings: string[] = []
    let match
    while ((match = regex.exec(html)) !== null) {
        const text = match[1].replace(/<[^>]*>/g, '').trim()
        if (text) headings.push(text)
    }
    return headings
}

function extractImages(html: string): {
    total: number
    withAlt: number
    withoutAlt: number
} {
    const imgRegex = /<img[^>]+>/gi
    const images = html.match(imgRegex) || []
    let withAlt = 0
    let withoutAlt = 0

    images.forEach((img) => {
        const altMatch = img.match(/alt=["']([^"']*)["']/i)
        if (altMatch && altMatch[1].trim()) {
            withAlt++
        } else {
            withoutAlt++
        }
    })

    return { total: images.length, withAlt, withoutAlt }
}

function extractInternalLinks(html: string, baseHost: string): string[] {
    const linkRegex = /<a[^>]+href=["']([^"'#]+)["'][^>]*>/gi
    const links: Set<string> = new Set()

    let match
    while ((match = linkRegex.exec(html)) !== null) {
        const href = match[1]

        if (
            href.startsWith('javascript:') ||
            href.startsWith('mailto:') ||
            href.startsWith('tel:')
        ) {
            continue
        }

        try {
            const linkUrl = href.startsWith('http')
                ? new URL(href)
                : new URL(href, BASE_URL)
            if (
                linkUrl.hostname === baseHost ||
                linkUrl.hostname === `www.${baseHost}` ||
                `www.${linkUrl.hostname}` === baseHost
            ) {
                // Normalize URL
                let normalizedPath = linkUrl.pathname
                if (normalizedPath.endsWith('/') && normalizedPath !== '/') {
                    normalizedPath = normalizedPath.slice(0, -1)
                }
                links.add(
                    `${linkUrl.protocol}//${linkUrl.hostname}${normalizedPath}`
                )
            }
        } catch {
            // Invalid URL, skip
        }
    }

    return Array.from(links)
}

function countLinks(
    html: string,
    baseHost: string
): { internal: number; external: number } {
    const linkRegex = /<a[^>]+href=["']([^"']+)["'][^>]*>/gi
    let internal = 0
    let external = 0

    let match
    while ((match = linkRegex.exec(html)) !== null) {
        const href = match[1]
        if (
            href.startsWith('#') ||
            href.startsWith('javascript:') ||
            href.startsWith('mailto:') ||
            href.startsWith('tel:')
        ) {
            continue
        }

        try {
            const linkUrl = href.startsWith('http')
                ? new URL(href)
                : new URL(href, BASE_URL)
            if (
                linkUrl.hostname === baseHost ||
                linkUrl.hostname === `www.${baseHost}` ||
                `www.${linkUrl.hostname}` === baseHost
            ) {
                internal++
            } else {
                external++
            }
        } catch {
            internal++
        }
    }

    return { internal, external }
}

function hasStructuredData(html: string): boolean {
    return /<script[^>]+type=["']application\/ld\+json["'][^>]*>/i.test(html)
}

function extractCanonical(html: string): string | null {
    const match = html.match(
        /<link[^>]+rel=["']canonical["'][^>]+href=["']([^"']+)["']/i
    )
    return match ? match[1] : null
}

function countWords(html: string): number {
    const textContent = html
        .replace(/<script[^>]*>[\s\S]*?<\/script>/gi, '')
        .replace(/<style[^>]*>[\s\S]*?<\/style>/gi, '')
        .replace(/<[^>]+>/g, ' ')
        .replace(/\s+/g, ' ')
        .trim()

    return textContent.split(' ').filter((word) => word.length > 0).length
}

function calculatePageScore(page: Partial<PageSeoResult>): {
    score: number
    issues: PageSeoResult['issues']
} {
    let score = 100
    const issues: PageSeoResult['issues'] = []

    if (!page.title?.content) {
        score -= 15
        issues.push({ severity: 'error', message: 'Nedostaje title tag' })
    } else {
        if (page.title.length < 30) {
            score -= 5
            issues.push({
                severity: 'warning',
                message: `Title prekratak (${page.title.length} kar.)`,
            })
        } else if (page.title.length > 60) {
            score -= 5
            issues.push({
                severity: 'warning',
                message: `Title predugačak (${page.title.length} kar.)`,
            })
        }
    }

    if (!page.metaDescription?.content) {
        score -= 10
        issues.push({
            severity: 'error',
            message: 'Nedostaje meta description',
        })
    } else {
        if (page.metaDescription.length < 120) {
            score -= 5
            issues.push({
                severity: 'warning',
                message: `Meta description prekratak (${page.metaDescription.length} kar.)`,
            })
        } else if (page.metaDescription.length > 160) {
            score -= 5
            issues.push({
                severity: 'warning',
                message: `Meta description predugačak (${page.metaDescription.length} kar.)`,
            })
        }
    }

    if (!page.headings?.h1.length) {
        score -= 10
        issues.push({ severity: 'error', message: 'Nedostaje H1' })
    } else if (page.headings.h1.length > 1) {
        score -= 5
        issues.push({
            severity: 'warning',
            message: `Više H1 tagova (${page.headings.h1.length})`,
        })
    }

    if (!page.canonical) {
        score -= 5
        issues.push({ severity: 'warning', message: 'Nedostaje canonical' })
    }

    if (!page.viewport) {
        score -= 10
        issues.push({ severity: 'error', message: 'Nedostaje viewport' })
    }

    if (
        !page.openGraph?.title ||
        !page.openGraph?.description ||
        !page.openGraph?.image
    ) {
        score -= 5
        issues.push({ severity: 'warning', message: 'Nepotpuni OG tagovi' })
    }

    if (page.images && page.images.withoutAlt > 0) {
        const penalty = Math.min(page.images.withoutAlt * 2, 10)
        score -= penalty
        issues.push({
            severity: 'warning',
            message: `${page.images.withoutAlt} slika bez alt`,
        })
    }

    if (!page.hasStructuredData) {
        score -= 5
        issues.push({ severity: 'info', message: 'Nema Schema.org' })
    }

    return { score: Math.max(0, score), issues }
}

async function analyzePage(url: string): Promise<PageSeoResult | null> {
    try {
        const startTime = Date.now()
        const response = await fetch(url, {
            headers: {
                'User-Agent': 'Mozilla/5.0 (compatible; TermodomSEOBot/1.0)',
                Accept: 'text/html',
            },
        })

        if (
            !response.ok ||
            !response.headers.get('content-type')?.includes('text/html')
        ) {
            return null
        }

        const fetchTimeMs = Date.now() - startTime
        const html = await response.text()
        const baseHost = new URL(BASE_URL).hostname

        const title = extractTitle(html)
        const metaDescription = extractMetaContent(html, 'description')
        const h1s = extractHeadings(html, 'h1')
        const images = extractImages(html)
        const links = countLinks(html, baseHost)

        const partialResult: Partial<PageSeoResult> = {
            url,
            fetchTimeMs,
            statusCode: response.status,
            title: { content: title, length: title?.length || 0 },
            metaDescription: {
                content: metaDescription,
                length: metaDescription?.length || 0,
            },
            canonical: extractCanonical(html),
            robots: extractMetaContent(html, 'robots'),
            viewport: extractMetaContent(html, 'viewport'),
            openGraph: {
                title: extractMetaProperty(html, 'og:title'),
                description: extractMetaProperty(html, 'og:description'),
                image: extractMetaProperty(html, 'og:image'),
            },
            headings: {
                h1: h1s,
                h2Count: extractHeadings(html, 'h2').length,
                h3Count: extractHeadings(html, 'h3').length,
            },
            images,
            links,
            hasStructuredData: hasStructuredData(html),
            wordCount: countWords(html),
        }

        const { score, issues } = calculatePageScore(partialResult)

        return { ...partialResult, score, issues } as PageSeoResult
    } catch (error) {
        console.error(`Error analyzing ${url}:`, error)
        return null
    }
}

const BATCH_SIZE = 10 // Process 10 pages in parallel

async function fetchAndAnalyzePage(
    url: string,
    baseHost: string
): Promise<{ result: PageSeoResult | null; newLinks: string[] }> {
    try {
        const startTime = Date.now()
        const response = await fetch(url, {
            headers: {
                'User-Agent': 'Mozilla/5.0 (compatible; TermodomSEOBot/1.0)',
                Accept: 'text/html',
            },
        })

        if (
            !response.ok ||
            !response.headers.get('content-type')?.includes('text/html')
        ) {
            return { result: null, newLinks: [] }
        }

        const fetchTimeMs = Date.now() - startTime
        const html = await response.text()

        // Extract links for crawling
        const newLinks = extractInternalLinks(html, baseHost)

        // Analyze the page (reuse the fetched HTML)
        const title = extractTitle(html)
        const metaDescription = extractMetaContent(html, 'description')
        const h1s = extractHeadings(html, 'h1')
        const images = extractImages(html)
        const links = countLinks(html, baseHost)

        const partialResult: Partial<PageSeoResult> = {
            url,
            fetchTimeMs,
            statusCode: response.status,
            title: { content: title, length: title?.length || 0 },
            metaDescription: {
                content: metaDescription,
                length: metaDescription?.length || 0,
            },
            canonical: extractCanonical(html),
            robots: extractMetaContent(html, 'robots'),
            viewport: extractMetaContent(html, 'viewport'),
            openGraph: {
                title: extractMetaProperty(html, 'og:title'),
                description: extractMetaProperty(html, 'og:description'),
                image: extractMetaProperty(html, 'og:image'),
            },
            headings: {
                h1: h1s,
                h2Count: extractHeadings(html, 'h2').length,
                h3Count: extractHeadings(html, 'h3').length,
            },
            images,
            links,
            hasStructuredData: hasStructuredData(html),
            wordCount: countWords(html),
        }

        const { score, issues } = calculatePageScore(partialResult)
        const result = { ...partialResult, score, issues } as PageSeoResult

        return { result, newLinks }
    } catch (error) {
        console.error(`Error analyzing ${url}:`, error)
        return { result: null, newLinks: [] }
    }
}

async function crawlSite(): Promise<SiteAnalysisResult> {
    const startTime = Date.now()
    const baseHost = new URL(BASE_URL).hostname
    const visited: Set<string> = new Set()
    const toVisit: Set<string> = new Set([BASE_URL])
    const pages: PageSeoResult[] = []

    while (toVisit.size > 0 && visited.size < MAX_PAGES) {
        // Get batch of URLs to process
        const batch: string[] = []
        const iterator = toVisit.values()
        while (
            batch.length < BATCH_SIZE &&
            batch.length < MAX_PAGES - visited.size
        ) {
            const next = iterator.next()
            if (next.done) break
            const url = next.value
            if (!visited.has(url)) {
                batch.push(url)
                visited.add(url)
                toVisit.delete(url)
            }
        }

        if (batch.length === 0) break

        // Process batch in parallel
        const results = await Promise.all(
            batch.map((url) => fetchAndAnalyzePage(url, baseHost))
        )

        // Collect results and new links
        for (const { result, newLinks } of results) {
            if (result) {
                pages.push(result)
            }
            for (const link of newLinks) {
                if (!visited.has(link)) {
                    toVisit.add(link)
                }
            }
        }
    }

    // Calculate summary
    const summary = {
        pagesWithoutTitle: pages.filter((p) => !p.title.content).length,
        pagesWithoutDescription: pages.filter((p) => !p.metaDescription.content)
            .length,
        pagesWithoutH1: pages.filter((p) => p.headings.h1.length === 0).length,
        pagesWithoutCanonical: pages.filter((p) => !p.canonical).length,
        pagesWithoutViewport: pages.filter((p) => !p.viewport).length,
        pagesWithoutOg: pages.filter(
            (p) =>
                !p.openGraph.title ||
                !p.openGraph.description ||
                !p.openGraph.image
        ).length,
        pagesWithoutStructuredData: pages.filter((p) => !p.hasStructuredData)
            .length,
        imagesWithoutAlt: pages.reduce(
            (sum, p) => sum + p.images.withoutAlt,
            0
        ),
        totalImages: pages.reduce((sum, p) => sum + p.images.total, 0),
        totalInternalLinks: pages.reduce((sum, p) => sum + p.links.internal, 0),
        totalExternalLinks: pages.reduce((sum, p) => sum + p.links.external, 0),
    }

    // Calculate common issues
    const issueCount: Map<
        string,
        { count: number; severity: 'error' | 'warning' | 'info' }
    > = new Map()
    for (const page of pages) {
        for (const issue of page.issues) {
            const existing = issueCount.get(issue.message)
            if (existing) {
                existing.count++
            } else {
                issueCount.set(issue.message, {
                    count: 1,
                    severity: issue.severity,
                })
            }
        }
    }

    const commonIssues = Array.from(issueCount.entries())
        .map(([issue, data]) => ({
            issue,
            count: data.count,
            severity: data.severity,
        }))
        .sort((a, b) => b.count - a.count)

    const averageScore =
        pages.length > 0
            ? Math.round(
                  pages.reduce((sum, p) => sum + p.score, 0) / pages.length
              )
            : 0

    return {
        baseUrl: BASE_URL,
        totalPages: pages.length,
        analyzedAt: new Date().toISOString(),
        totalTimeMs: Date.now() - startTime,
        averageScore,
        pages: pages.sort((a, b) => a.score - b.score), // Worst first
        summary,
        commonIssues,
    }
}

export default async function handler(
    req: NextApiRequest,
    res: NextApiResponse<SiteAnalysisResult | { error: string }>
) {
    if (req.method !== 'POST') {
        return res.status(405).json({ error: 'Method not allowed' })
    }

    try {
        const result = await crawlSite()
        res.status(200).json(result)
    } catch (error) {
        console.error('Site analysis error:', error)
        res.status(500).json({
            error: `Failed to analyze site: ${error instanceof Error ? error.message : 'Unknown error'}`,
        })
    }
}
