import { CustomHead } from '@/widgets/CustomHead'
import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import {
    Box,
    Breadcrumbs,
    CircularProgress,
    Link,
    Stack,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { handleApiError, webApi } from '@/api/webApi'
import NextLink from 'next/link'
import { ArrowBack } from '@mui/icons-material'
import ReactMarkdown from 'react-markdown'

const BlogDetailPage = () => {
    const router = useRouter()
    const { slug } = router.query
    const [blog, setBlog] = useState(null)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState(null)

    useEffect(() => {
        if (!slug) return

        setIsLoading(true)
        setError(null)
        webApi
            .get(`/blogs/${slug}`)
            .then((res) => {
                setBlog(res.data)
            })
            .catch((err) => {
                if (err.response?.status === 404) {
                    setError('Blog nije pronađen')
                } else {
                    handleApiError(err)
                    setError('Greška pri učitavanju bloga')
                }
            })
            .finally(() => setIsLoading(false))
    }, [slug])

    if (isLoading) {
        return (
            <CenteredContentWrapper>
                <Stack
                    alignItems="center"
                    justifyContent="center"
                    sx={{ py: 10, width: '100%' }}
                >
                    <CircularProgress />
                </Stack>
            </CenteredContentWrapper>
        )
    }

    if (error || !blog) {
        return (
            <CenteredContentWrapper>
                <Stack
                    alignItems="center"
                    justifyContent="center"
                    sx={{ py: 10, width: '100%' }}
                >
                    <Typography variant="h5" color="text.secondary">
                        {error || 'Blog nije pronađen'}
                    </Typography>
                    <Link
                        component={NextLink}
                        href="/blog"
                        sx={{ mt: 2, display: 'flex', alignItems: 'center', gap: 1 }}
                    >
                        <ArrowBack fontSize="small" />
                        Nazad na blog
                    </Link>
                </Stack>
            </CenteredContentWrapper>
        )
    }

    const imageUrl = blog.coverImageData
        ? `data:${blog.coverImageData.contentType};base64,${blog.coverImageData.data}`
        : null

    const formattedDate = blog.publishedAt
        ? new Date(blog.publishedAt).toLocaleDateString('sr-RS', {
              year: 'numeric',
              month: 'long',
              day: 'numeric',
          })
        : null

    const pageTitle = `${blog.title} | Termodom Blog`
    const pageDescription = blog.summary || blog.title

    return (
        <>
            <CustomHead title={pageTitle} description={pageDescription} />
            <CenteredContentWrapper>
                <Stack sx={{ width: '100%', px: 2 }}>
                    <Breadcrumbs sx={{ py: 2 }}>
                        <Link
                            component={NextLink}
                            href="/"
                            underline="hover"
                            color="inherit"
                        >
                            Početna
                        </Link>
                        <Link
                            component={NextLink}
                            href="/blog"
                            underline="hover"
                            color="inherit"
                        >
                            Blog
                        </Link>
                        <Typography color="text.primary">{blog.title}</Typography>
                    </Breadcrumbs>

                    {imageUrl && (
                        <Box
                            component="img"
                            src={imageUrl}
                            alt={blog.title}
                            sx={{
                                width: '100%',
                                maxHeight: 400,
                                objectFit: 'cover',
                                borderRadius: 2,
                                mb: 3,
                            }}
                        />
                    )}

                    <Typography
                        variant="h3"
                        component="h1"
                        sx={{
                            fontFamily: 'GothamProMedium',
                            mb: 2,
                        }}
                    >
                        {blog.title}
                    </Typography>

                    {formattedDate && (
                        <Typography
                            variant="body2"
                            color="text.secondary"
                            sx={{ mb: 3 }}
                        >
                            Objavljeno: {formattedDate}
                        </Typography>
                    )}

                    {blog.summary && (
                        <Typography
                            variant="subtitle1"
                            sx={{
                                mb: 3,
                                fontStyle: 'italic',
                                color: 'text.secondary',
                                borderLeft: 3,
                                borderColor: 'primary.main',
                                pl: 2,
                            }}
                        >
                            {blog.summary}
                        </Typography>
                    )}

                    <Box
                        sx={{
                            '& h1': { fontSize: '2rem', fontWeight: 'bold', mt: 4, mb: 2 },
                            '& h2': { fontSize: '1.5rem', fontWeight: 'bold', mt: 4, mb: 2 },
                            '& h3': { fontSize: '1.25rem', fontWeight: 'bold', mt: 3, mb: 2 },
                            '& h4': { fontSize: '1.1rem', fontWeight: 'bold', mt: 2, mb: 1 },
                            '& p': { mb: 2, lineHeight: 1.8 },
                            '& ul, & ol': { pl: 3, mb: 2 },
                            '& li': { mb: 1 },
                            '& blockquote': {
                                borderLeft: 4,
                                borderColor: 'primary.main',
                                pl: 2,
                                my: 2,
                                color: 'text.secondary',
                                fontStyle: 'italic',
                            },
                            '& code': {
                                backgroundColor: 'grey.100',
                                px: 0.5,
                                py: 0.25,
                                borderRadius: 1,
                                fontFamily: 'monospace',
                            },
                            '& pre': {
                                backgroundColor: 'grey.100',
                                p: 2,
                                borderRadius: 1,
                                overflow: 'auto',
                                '& code': {
                                    backgroundColor: 'transparent',
                                    p: 0,
                                },
                            },
                            '& a': {
                                color: 'primary.main',
                                textDecoration: 'underline',
                            },
                            '& img': {
                                maxWidth: '100%',
                                height: 'auto',
                                borderRadius: 1,
                            },
                            '& hr': {
                                my: 3,
                                border: 'none',
                                borderTop: 1,
                                borderColor: 'divider',
                            },
                            fontSize: '1.1rem',
                        }}
                    >
                        <ReactMarkdown>{blog.text}</ReactMarkdown>
                    </Box>

                    <Box sx={{ py: 4 }}>
                        <Link
                            component={NextLink}
                            href="/blog"
                            sx={{ display: 'flex', alignItems: 'center', gap: 1 }}
                        >
                            <ArrowBack fontSize="small" />
                            Nazad na blog
                        </Link>
                    </Box>
                </Stack>
            </CenteredContentWrapper>
        </>
    )
}

export default BlogDetailPage
