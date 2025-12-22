import {
    Card,
    CardActionArea,
    CardContent,
    CardMedia,
    Typography,
} from '@mui/material'
import NextLink from 'next/link'

const placeholderImage = '/images/blog-placeholder.png'

export const BlogCard = ({ blog }) => {
    const imageUrl = blog.coverImageData
        ? `data:${blog.coverImageContentType};base64,${blog.coverImageData}`
        : placeholderImage

    const formattedDate = blog.publishedAt
        ? new Date(blog.publishedAt).toLocaleDateString('sr-RS', {
              year: 'numeric',
              month: 'long',
              day: 'numeric',
          })
        : null

    return (
        <Card
            sx={{
                height: '100%',
                display: 'flex',
                flexDirection: 'column',
            }}
        >
            <CardActionArea
                component={NextLink}
                href={`/blog/${blog.slug}`}
                sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column', alignItems: 'stretch' }}
            >
                <CardMedia
                    component="img"
                    height="200"
                    image={imageUrl}
                    alt={blog.title}
                    sx={{ objectFit: 'cover' }}
                />
                <CardContent sx={{ flexGrow: 1 }}>
                    <Typography
                        gutterBottom
                        variant="h6"
                        component="h2"
                        sx={{
                            fontFamily: 'GothamProMedium',
                            overflow: 'hidden',
                            textOverflow: 'ellipsis',
                            display: '-webkit-box',
                            WebkitLineClamp: 2,
                            WebkitBoxOrient: 'vertical',
                        }}
                    >
                        {blog.title}
                    </Typography>
                    {formattedDate && (
                        <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{ mb: 1, display: 'block' }}
                        >
                            {formattedDate}
                        </Typography>
                    )}
                    {blog.summary && (
                        <Typography
                            variant="body2"
                            color="text.secondary"
                            sx={{
                                overflow: 'hidden',
                                textOverflow: 'ellipsis',
                                display: '-webkit-box',
                                WebkitLineClamp: 3,
                                WebkitBoxOrient: 'vertical',
                            }}
                        >
                            {blog.summary}
                        </Typography>
                    )}
                </CardContent>
            </CardActionArea>
        </Card>
    )
}
