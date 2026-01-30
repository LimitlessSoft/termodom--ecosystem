import { useState } from 'react'
import {
    Box,
    Button,
    CircularProgress,
    Paper,
    Typography,
    Alert,
    Chip,
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    LinearProgress,
    Divider,
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    TableContainer,
    Link,
} from '@mui/material'
import Grid2 from '@mui/material/Unstable_Grid2'
import {
    ExpandMore,
    PlayArrow,
    CheckCircle,
    Warning,
    Error,
    Info,
    OpenInNew,
    Edit,
} from '@mui/icons-material'
import axios from 'axios'
import { toast } from 'react-toastify'
import { adminApi } from '@/apis/adminApi'
import { ISiteAnalysisResult, IPageSeoResult } from '../interfaces/ISeoAnalysis'

const getScoreColor = (score: number): 'success' | 'warning' | 'error' => {
    if (score >= 80) return 'success'
    if (score >= 60) return 'warning'
    return 'error'
}

const getSeverityIcon = (severity: 'error' | 'warning' | 'info') => {
    switch (severity) {
        case 'error':
            return <Error color="error" fontSize="small" />
        case 'warning':
            return <Warning color="warning" fontSize="small" />
        case 'info':
            return <Info color="info" fontSize="small" />
    }
}

const getSeverityColor = (severity: 'error' | 'warning' | 'info') => {
    switch (severity) {
        case 'error':
            return 'error'
        case 'warning':
            return 'warning'
        case 'info':
            return 'info'
    }
}

const formatDuration = (ms: number): string => {
    if (ms < 1000) return `${ms}ms`
    const seconds = Math.floor(ms / 1000)
    if (seconds < 60) return `${seconds}s`
    const minutes = Math.floor(seconds / 60)
    const remainingSeconds = seconds % 60
    return `${minutes}m ${remainingSeconds}s`
}

const getProductSlug = (url: string): string | null => {
    const match = url.match(/\/proizvodi\/([^/]+)$/)
    return match ? match[1] : null
}

const PageRow = ({ page, index }: { page: IPageSeoResult; index: number }) => {
    const [expanded, setExpanded] = useState(false)
    const [isLoadingProduct, setIsLoadingProduct] = useState(false)
    const productSlug = getProductSlug(page.url)

    const handleEditProduct = async () => {
        if (!productSlug) return
        setIsLoadingProduct(true)
        try {
            const response = await adminApi.get(`/products/by-slug/${encodeURIComponent(productSlug)}`)
            if (response.data?.id) {
                window.open(`/proizvodi/izmeni/${response.data.id}`, '_blank')
            } else {
                toast.error('Proizvod nije pronađen')
            }
        } catch {
            toast.error('Proizvod nije pronađen')
        } finally {
            setIsLoadingProduct(false)
        }
    }

    return (
        <>
            <TableRow
                hover
                onClick={() => setExpanded(!expanded)}
                sx={{ cursor: 'pointer', '& > *': { borderBottom: expanded ? 'none' : undefined } }}
            >
                <TableCell sx={{ width: 50 }}>{index + 1}</TableCell>
                <TableCell>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <Typography variant="body2" sx={{ wordBreak: 'break-all' }}>
                            {page.url.replace('https://termodom.rs', '')}
                        </Typography>
                        <Link
                            href={page.url}
                            target="_blank"
                            onClick={(e) => e.stopPropagation()}
                            sx={{ display: 'flex' }}
                        >
                            <OpenInNew fontSize="small" />
                        </Link>
                    </Box>
                </TableCell>
                <TableCell align="center">
                    <Chip
                        label={page.score}
                        size="small"
                        color={getScoreColor(page.score)}
                    />
                </TableCell>
                <TableCell align="center">
                    <Typography
                        variant="body2"
                        color={!page.title.content || page.title.length > 60 ? 'error' : page.title.length < 30 ? 'warning.main' : 'text.primary'}
                    >
                        {page.title.length || '-'}
                    </Typography>
                </TableCell>
                <TableCell align="center">
                    <Typography
                        variant="body2"
                        color={!page.metaDescription.content || page.metaDescription.length > 160 ? 'error' : page.metaDescription.length < 120 ? 'warning.main' : 'text.primary'}
                    >
                        {page.metaDescription.length || '-'}
                    </Typography>
                </TableCell>
                <TableCell align="center">
                    <Typography variant="body2" color={page.headings.h1.length === 1 ? 'success.main' : 'error'}>
                        {page.headings.h1.length}
                    </Typography>
                </TableCell>
                <TableCell align="center">
                    {page.issues.length > 0 ? (
                        <Chip label={page.issues.length} size="small" color="warning" variant="outlined" />
                    ) : (
                        <CheckCircle color="success" fontSize="small" />
                    )}
                </TableCell>
            </TableRow>
            {expanded && (
                <TableRow>
                    <TableCell colSpan={7} sx={{ bgcolor: 'grey.50', py: 2 }}>
                        <Grid2 container spacing={2}>
                            <Grid2 xs={12} md={6}>
                                <Typography variant="subtitle2" gutterBottom>Title</Typography>
                                <Paper variant="outlined" sx={{ p: 1, mb: 2 }}>
                                    <Typography variant="body2">
                                        {page.title.content || <em style={{ color: 'red' }}>Nedostaje</em>}
                                    </Typography>
                                </Paper>
                                <Typography variant="subtitle2" gutterBottom>Meta Description</Typography>
                                <Paper variant="outlined" sx={{ p: 1 }}>
                                    <Typography variant="body2">
                                        {page.metaDescription.content || <em style={{ color: 'red' }}>Nedostaje</em>}
                                    </Typography>
                                </Paper>
                            </Grid2>
                            <Grid2 xs={12} md={6}>
                                <Typography variant="subtitle2" gutterBottom>Problemi ({page.issues.length})</Typography>
                                {page.issues.length === 0 ? (
                                    <Alert severity="success" sx={{ py: 0 }}>Nema problema</Alert>
                                ) : (
                                    <List dense disablePadding>
                                        {page.issues.map((issue, i) => (
                                            <ListItem key={i} disablePadding sx={{ py: 0.25 }}>
                                                <ListItemIcon sx={{ minWidth: 28 }}>
                                                    {getSeverityIcon(issue.severity)}
                                                </ListItemIcon>
                                                <ListItemText
                                                    primary={issue.message}
                                                    primaryTypographyProps={{ variant: 'body2' }}
                                                />
                                            </ListItem>
                                        ))}
                                    </List>
                                )}
                            </Grid2>
                            <Grid2 xs={12}>
                                <Divider sx={{ my: 1 }} />
                                <Box sx={{ display: 'flex', gap: 3, flexWrap: 'wrap', alignItems: 'center' }}>
                                    <Typography variant="body2">
                                        <strong>H1:</strong> {page.headings.h1.join(', ') || '-'}
                                    </Typography>
                                    <Typography variant="body2">
                                        <strong>H2:</strong> {page.headings.h2Count}
                                    </Typography>
                                    <Typography variant="body2">
                                        <strong>H3:</strong> {page.headings.h3Count}
                                    </Typography>
                                    <Typography variant="body2">
                                        <strong>Slike:</strong> {page.images.total} ({page.images.withoutAlt} bez alt)
                                    </Typography>
                                    <Typography variant="body2">
                                        <strong>Linkovi:</strong> {page.links.internal} int / {page.links.external} ext
                                    </Typography>
                                    <Typography variant="body2">
                                        <strong>Schema.org:</strong> {page.hasStructuredData ? 'Da' : 'Ne'}
                                    </Typography>
                                    <Typography variant="body2">
                                        <strong>Canonical:</strong> {page.canonical ? 'Da' : 'Ne'}
                                    </Typography>
                                    {productSlug && (
                                        <Button
                                            variant="contained"
                                            size="small"
                                            startIcon={isLoadingProduct ? <CircularProgress size={16} color="inherit" /> : <Edit />}
                                            onClick={handleEditProduct}
                                            disabled={isLoadingProduct}
                                            sx={{ ml: 'auto' }}
                                        >
                                            Izmeni proizvod
                                        </Button>
                                    )}
                                </Box>
                            </Grid2>
                        </Grid2>
                    </TableCell>
                </TableRow>
            )}
        </>
    )
}

export const SeoAnalyzer = (): JSX.Element => {
    const [isAnalyzing, setIsAnalyzing] = useState(false)
    const [result, setResult] = useState<ISiteAnalysisResult | null>(null)
    const [error, setError] = useState<string | null>(null)

    const handleAnalyze = async () => {
        setIsAnalyzing(true)
        setError(null)
        setResult(null)

        try {
            const response = await axios.post('/api/seo/analyze', {}, { timeout: 300000 }) // 5 min timeout
            setResult(response.data)
            toast.success(`Analiza završena! Pronađeno ${response.data.totalPages} stranica.`)
        } catch (err: any) {
            const errorMessage = err.response?.data?.error || 'Greška prilikom analize'
            setError(errorMessage)
            toast.error(errorMessage)
        } finally {
            setIsAnalyzing(false)
        }
    }

    return (
        <Box p={2}>
            <Paper sx={{ p: 3, mb: 3 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', flexWrap: 'wrap', gap: 2 }}>
                    <Box>
                        <Typography variant="h5" gutterBottom>
                            SEO Analyzer - termodom.rs
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            Automatska analiza svih stranica na sajtu
                        </Typography>
                    </Box>
                    <Button
                        variant="contained"
                        size="large"
                        startIcon={isAnalyzing ? <CircularProgress size={20} color="inherit" /> : <PlayArrow />}
                        onClick={handleAnalyze}
                        disabled={isAnalyzing}
                    >
                        {isAnalyzing ? 'Analizira se...' : 'Pokreni analizu'}
                    </Button>
                </Box>
                {isAnalyzing && (
                    <Box sx={{ mt: 2 }}>
                        <LinearProgress />
                        <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                            Crawling sajta i analiza stranica... Ovo može potrajati nekoliko minuta.
                        </Typography>
                    </Box>
                )}
            </Paper>

            {error && (
                <Alert severity="error" sx={{ mb: 3 }}>
                    {error}
                </Alert>
            )}

            {result && (
                <Grid2 container spacing={3}>
                    {/* Overview Cards */}
                    <Grid2 xs={12} md={3}>
                        <Paper sx={{ p: 3, textAlign: 'center', height: '100%' }}>
                            <Typography variant="h6" gutterBottom>Prosečan SEO Score</Typography>
                            <Box sx={{ position: 'relative', display: 'inline-flex', mb: 1 }}>
                                <CircularProgress
                                    variant="determinate"
                                    value={result.averageScore}
                                    size={100}
                                    color={getScoreColor(result.averageScore)}
                                    thickness={5}
                                />
                                <Box
                                    sx={{
                                        top: 0, left: 0, bottom: 0, right: 0,
                                        position: 'absolute',
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center',
                                    }}
                                >
                                    <Typography variant="h4" fontWeight="bold">
                                        {result.averageScore}
                                    </Typography>
                                </Box>
                            </Box>
                            <Typography variant="body2" color="text.secondary">od 100</Typography>
                        </Paper>
                    </Grid2>

                    <Grid2 xs={12} md={3}>
                        <Paper sx={{ p: 3, textAlign: 'center', height: '100%' }}>
                            <Typography variant="h6" gutterBottom>Analizirano stranica</Typography>
                            <Typography variant="h2" color="primary">{result.totalPages}</Typography>
                            <Typography variant="body2" color="text.secondary">
                                za {formatDuration(result.totalTimeMs)}
                            </Typography>
                        </Paper>
                    </Grid2>

                    <Grid2 xs={12} md={3}>
                        <Paper sx={{ p: 3, textAlign: 'center', height: '100%' }}>
                            <Typography variant="h6" gutterBottom>Slike</Typography>
                            <Typography variant="h2" color="primary">{result.summary.totalImages}</Typography>
                            <Typography variant="body2" color={result.summary.imagesWithoutAlt > 0 ? 'error' : 'text.secondary'}>
                                {result.summary.imagesWithoutAlt} bez alt teksta
                            </Typography>
                        </Paper>
                    </Grid2>

                    <Grid2 xs={12} md={3}>
                        <Paper sx={{ p: 3, textAlign: 'center', height: '100%' }}>
                            <Typography variant="h6" gutterBottom>Linkovi</Typography>
                            <Typography variant="h2" color="primary">{result.summary.totalInternalLinks}</Typography>
                            <Typography variant="body2" color="text.secondary">
                                interni ({result.summary.totalExternalLinks} ext)
                            </Typography>
                        </Paper>
                    </Grid2>

                    {/* Summary Issues */}
                    <Grid2 xs={12} md={6}>
                        <Paper sx={{ p: 3, height: '100%' }}>
                            <Typography variant="h6" gutterBottom>Pregled problema po tipu</Typography>
                            <Table size="small">
                                <TableBody>
                                    <TableRow>
                                        <TableCell>Stranice bez title</TableCell>
                                        <TableCell align="right">
                                            <Chip
                                                label={result.summary.pagesWithoutTitle}
                                                size="small"
                                                color={result.summary.pagesWithoutTitle > 0 ? 'error' : 'success'}
                                            />
                                        </TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell>Stranice bez meta description</TableCell>
                                        <TableCell align="right">
                                            <Chip
                                                label={result.summary.pagesWithoutDescription}
                                                size="small"
                                                color={result.summary.pagesWithoutDescription > 0 ? 'error' : 'success'}
                                            />
                                        </TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell>Stranice bez H1</TableCell>
                                        <TableCell align="right">
                                            <Chip
                                                label={result.summary.pagesWithoutH1}
                                                size="small"
                                                color={result.summary.pagesWithoutH1 > 0 ? 'error' : 'success'}
                                            />
                                        </TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell>Stranice bez canonical</TableCell>
                                        <TableCell align="right">
                                            <Chip
                                                label={result.summary.pagesWithoutCanonical}
                                                size="small"
                                                color={result.summary.pagesWithoutCanonical > 0 ? 'warning' : 'success'}
                                            />
                                        </TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell>Stranice bez viewport</TableCell>
                                        <TableCell align="right">
                                            <Chip
                                                label={result.summary.pagesWithoutViewport}
                                                size="small"
                                                color={result.summary.pagesWithoutViewport > 0 ? 'error' : 'success'}
                                            />
                                        </TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell>Stranice bez Open Graph</TableCell>
                                        <TableCell align="right">
                                            <Chip
                                                label={result.summary.pagesWithoutOg}
                                                size="small"
                                                color={result.summary.pagesWithoutOg > 0 ? 'warning' : 'success'}
                                            />
                                        </TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell>Stranice bez Schema.org</TableCell>
                                        <TableCell align="right">
                                            <Chip
                                                label={result.summary.pagesWithoutStructuredData}
                                                size="small"
                                                color={result.summary.pagesWithoutStructuredData > 0 ? 'info' : 'success'}
                                            />
                                        </TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </Paper>
                    </Grid2>

                    {/* Common Issues */}
                    <Grid2 xs={12} md={6}>
                        <Paper sx={{ p: 3, height: '100%' }}>
                            <Typography variant="h6" gutterBottom>
                                Najčešći problemi
                            </Typography>
                            {result.commonIssues.length === 0 ? (
                                <Alert severity="success">Nisu pronađeni problemi!</Alert>
                            ) : (
                                <List dense>
                                    {result.commonIssues.slice(0, 10).map((issue, index) => (
                                        <ListItem key={index} disablePadding sx={{ py: 0.5 }}>
                                            <ListItemIcon sx={{ minWidth: 32 }}>
                                                {getSeverityIcon(issue.severity)}
                                            </ListItemIcon>
                                            <ListItemText
                                                primary={issue.issue}
                                                secondary={`${issue.count} stranica`}
                                            />
                                            <Chip
                                                label={issue.count}
                                                size="small"
                                                color={getSeverityColor(issue.severity)}
                                                variant="outlined"
                                            />
                                        </ListItem>
                                    ))}
                                </List>
                            )}
                        </Paper>
                    </Grid2>

                    {/* Pages Table */}
                    <Grid2 xs={12}>
                        <Accordion defaultExpanded>
                            <AccordionSummary expandIcon={<ExpandMore />}>
                                <Typography variant="h6">
                                    Sve stranice ({result.pages.length}) - sortirano po score (najlošije prvo)
                                </Typography>
                            </AccordionSummary>
                            <AccordionDetails sx={{ p: 0 }}>
                                <TableContainer>
                                    <Table size="small">
                                        <TableHead>
                                            <TableRow sx={{ bgcolor: 'grey.100' }}>
                                                <TableCell sx={{ fontWeight: 'bold' }}>#</TableCell>
                                                <TableCell sx={{ fontWeight: 'bold' }}>URL</TableCell>
                                                <TableCell sx={{ fontWeight: 'bold' }} align="center">Score</TableCell>
                                                <TableCell sx={{ fontWeight: 'bold' }} align="center">Title</TableCell>
                                                <TableCell sx={{ fontWeight: 'bold' }} align="center">Desc</TableCell>
                                                <TableCell sx={{ fontWeight: 'bold' }} align="center">H1</TableCell>
                                                <TableCell sx={{ fontWeight: 'bold' }} align="center">Problemi</TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {result.pages.map((page, index) => (
                                                <PageRow key={page.url} page={page} index={index} />
                                            ))}
                                        </TableBody>
                                    </Table>
                                </TableContainer>
                            </AccordionDetails>
                        </Accordion>
                    </Grid2>

                    {/* Analysis Info */}
                    <Grid2 xs={12}>
                        <Typography variant="body2" color="text.secondary" textAlign="center">
                            Analiza izvršena: {new Date(result.analyzedAt).toLocaleString('sr-RS')} |
                            Trajanje: {formatDuration(result.totalTimeMs)} |
                            Sajt: {result.baseUrl}
                        </Typography>
                    </Grid2>
                </Grid2>
            )}
        </Box>
    )
}
