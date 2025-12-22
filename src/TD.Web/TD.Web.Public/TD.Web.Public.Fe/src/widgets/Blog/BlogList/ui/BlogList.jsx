import { CircularProgress, Grid, Pagination, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { handleApiError, webApi } from '@/api/webApi'
import { BlogCard } from '@/widgets/Blog/BlogCard'

export const BlogList = () => {
    const [blogs, setBlogs] = useState(null)
    const [isLoading, setIsLoading] = useState(true)
    const [page, setPage] = useState(1)
    const [totalPages, setTotalPages] = useState(1)
    const pageSize = 9

    useEffect(() => {
        setIsLoading(true)
        webApi
            .get('/blogs', {
                params: {
                    page: page,
                    pageSize: pageSize,
                    sortColumn: 'PublishedAt',
                    sortDirection: 1,
                },
            })
            .then((res) => {
                setBlogs(res.data?.payload || [])
                const totalPages = res.data?.pagination?.totalPages || 1
                setTotalPages(totalPages)
            })
            .catch((err) => handleApiError(err))
            .finally(() => setIsLoading(false))
    }, [page])

    const handlePageChange = (event, value) => {
        setPage(value)
        window.scrollTo({ top: 0, behavior: 'smooth' })
    }

    if (isLoading) {
        return (
            <Stack alignItems="center" justifyContent="center" sx={{ py: 10 }}>
                <CircularProgress />
            </Stack>
        )
    }

    if (!blogs || blogs.length === 0) {
        return (
            <Stack alignItems="center" justifyContent="center" sx={{ py: 10 }}>
                <Typography variant="h6" color="text.secondary">
                    Nema objavljenih blogova
                </Typography>
            </Stack>
        )
    }

    return (
        <Stack spacing={4}>
            <Grid container spacing={3}>
                {blogs.map((blog) => (
                    <Grid item xs={12} sm={6} md={4} key={blog.id}>
                        <BlogCard blog={blog} />
                    </Grid>
                ))}
            </Grid>
            {totalPages > 1 && (
                <Stack alignItems="center" sx={{ py: 2 }}>
                    <Pagination
                        count={totalPages}
                        page={page}
                        onChange={handlePageChange}
                        color="primary"
                        size="large"
                    />
                </Stack>
            )}
        </Stack>
    )
}
