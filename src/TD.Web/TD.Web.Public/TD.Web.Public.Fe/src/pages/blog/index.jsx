import { CustomHead } from '@/widgets/CustomHead'
import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import { BlogList } from '@/widgets/Blog/BlogList'
import { Stack, Typography } from '@mui/material'

const BlogTitle = 'Blog | Termodom'
const BlogDescription = 'Najnovije vesti, saveti i informacije iz sveta građevinskog materijala | Termodom Blog'

const BlogPage = () => {
    return (
        <>
            <CustomHead title={BlogTitle} description={BlogDescription} />
            <CenteredContentWrapper>
                <Stack sx={{ width: '100%', px: 2 }}>
                    <Typography
                        variant="h4"
                        component="h1"
                        sx={{
                            textAlign: 'center',
                            py: 4,
                            fontFamily: 'GothamProMedium',
                        }}
                    >
                        Blog
                    </Typography>
                    <BlogList />
                </Stack>
            </CenteredContentWrapper>
        </>
    )
}

export default BlogPage
