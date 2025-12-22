import {
    Box,
    Button,
    Card,
    CardMedia,
    CircularProgress,
    MenuItem,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useRef, useState } from 'react'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { useRouter } from 'next/router'
import { BlogStatus, BlogStatusLabels } from '@/widgets/Blogovi/interfaces/IBlog'
import { MarkdownEditor } from '@/widgets/MarkdownEditor'

const textFieldVariant = 'standard'

const BlogoviIzmeni = () => {
    const router = useRouter()
    const { id } = router.query
    const imagePreviewRef = useRef(null)
    const [imageToUpload, setImageToUpload] = useState(null)
    const [isSaving, setIsSaving] = useState(false)
    const [isLoading, setIsLoading] = useState(true)

    const [formData, setFormData] = useState({
        id: null,
        title: '',
        text: '',
        slug: '',
        status: BlogStatus.Draft,
        summary: '',
        coverImage: '',
    })
    const [imageLoaded, setImageLoaded] = useState(false)

    useEffect(() => {
        if (!id) return

        setIsLoading(true)
        adminApi
            .get(`/blogs/${id}`)
            .then((response) => {
                setFormData({
                    id: response.data.id,
                    title: response.data.title || '',
                    text: response.data.text || '',
                    slug: response.data.slug || '',
                    status: response.data.status,
                    summary: response.data.summary || '',
                    coverImage: response.data.coverImage || '',
                })
            })
            .catch(handleApiError)
            .finally(() => setIsLoading(false))
    }, [id])

    useEffect(() => {
        if (!formData.coverImage || imageLoaded) return

        adminApi
            .get(`/images?image=${formData.coverImage}&quality=600`)
            .then((response) => {
                if (imagePreviewRef.current) {
                    imagePreviewRef.current.src = `data:${response.data.contentType};base64,${response.data.data}`
                }
                setImageLoaded(true)
            })
            .catch((err) => {
                console.error('Failed to load image:', err)
            })
    }, [formData.coverImage, imageLoaded])

    const handleFieldChange = (field) => (e) => {
        setFormData((prev) => ({
            ...prev,
            [field]: e.target.value,
        }))
    }

    const handleSave = () => {
        if (!formData.title.trim()) {
            toast.error('Naslov je obavezan!')
            return
        }
        if (!formData.text.trim()) {
            toast.error('Tekst je obavezan!')
            return
        }

        setIsSaving(true)

        const saveRequest = async () => {
            let coverImage = formData.coverImage

            if (imageToUpload) {
                const imageFormData = new FormData()
                imageFormData.append('Image', imageToUpload)

                const imageResponse = await adminApi.post('/images', imageFormData, {
                    headers: { 'Content-Type': 'multipart/form-data' },
                })
                coverImage = imageResponse.data
                toast.success('Slika uspešno uploadovana!')
            }

            const requestBody = {
                ...formData,
                coverImage,
            }

            await adminApi.put('/blogs', requestBody)
            toast.success('Blog uspešno sačuvan!')
        }

        saveRequest()
            .catch(handleApiError)
            .finally(() => setIsSaving(false))
    }

    const handlePublish = () => {
        adminApi
            .put(`/blogs/${id}/publish`)
            .then(() => {
                toast.success('Blog uspešno objavljen!')
                setFormData((prev) => ({ ...prev, status: BlogStatus.Published }))
            })
            .catch(handleApiError)
    }

    const handleUnpublish = () => {
        adminApi
            .put(`/blogs/${id}/unpublish`)
            .then(() => {
                toast.success('Blog uspešno povučen!')
                setFormData((prev) => ({ ...prev, status: BlogStatus.Draft }))
            })
            .catch(handleApiError)
    }

    if (isLoading) {
        return (
            <Stack alignItems="center" justifyContent="center" sx={{ mt: 10 }}>
                <CircularProgress />
                <Typography sx={{ mt: 2 }}>Učitavanje...</Typography>
            </Stack>
        )
    }

    const placeholderImage = 'https://via.placeholder.com/600x300?text=Cover+Image'

    return (
        <Stack
            direction="column"
            alignItems="center"
            sx={{ m: 2, '& .MuiTextField-root': { m: 1, width: '50ch' } }}
        >
            <Typography sx={{ m: 2 }} variant="h4">
                Izmeni blog
            </Typography>

            <Card sx={{ width: 600 }}>
                <CardMedia
                    sx={{ objectFit: 'contain' }}
                    component="img"
                    alt="upload-image"
                    height="300"
                    ref={imagePreviewRef}
                    src={placeholderImage}
                />
                <Button sx={{ width: '100%' }} variant="contained" component="label">
                    Promeni sliku
                    <input
                        type="file"
                        accept="image/*"
                        onChange={(evt) => {
                            const files = evt.target.files
                            if (!files || files.length === 0) {
                                toast.error('Greška pri učitavanju slike.')
                                return
                            }
                            setImageToUpload(files[0])

                            if (FileReader && files.length) {
                                const fr = new FileReader()
                                fr.onload = function () {
                                    if (imagePreviewRef.current) {
                                        imagePreviewRef.current.src = fr.result
                                    }
                                }
                                fr.readAsDataURL(files[0])
                            }
                        }}
                        hidden
                    />
                </Button>
            </Card>

            <TextField
                required
                id="title"
                label="Naslov"
                value={formData.title}
                onChange={handleFieldChange('title')}
                variant={textFieldVariant}
            />

            <TextField
                id="slug"
                label="Slug (URL putanja)"
                value={formData.slug}
                onChange={handleFieldChange('slug')}
                helperText="Ostavite prazno za automatsko generisanje"
                variant={textFieldVariant}
            />

            <TextField
                id="summary"
                label="Kratak opis"
                value={formData.summary}
                onChange={handleFieldChange('summary')}
                multiline
                rows={2}
                variant="outlined"
            />

            <Box sx={{ width: '100%', maxWidth: 800, mx: 'auto', my: 2 }}>
                <Typography variant="subtitle2" sx={{ mb: 1 }}>
                    Tekst bloga *
                </Typography>
                <MarkdownEditor
                    value={formData.text}
                    onChange={(value) => setFormData((prev) => ({ ...prev, text: value }))}
                    rows={15}
                    required
                />
            </Box>

            <TextField
                id="status"
                select
                label="Status"
                value={formData.status}
                onChange={handleFieldChange('status')}
                helperText="Izaberite status bloga"
                disabled
            >
                {Object.entries(BlogStatus).map(([key, value]) => (
                    <MenuItem key={key} value={value}>
                        {BlogStatusLabels[value]}
                    </MenuItem>
                ))}
            </TextField>

            <Stack direction="row" gap={2} sx={{ mt: 2 }}>
                <Button
                    endIcon={isSaving ? <CircularProgress size={20} color="inherit" /> : null}
                    disabled={isSaving}
                    size="large"
                    sx={{ px: 5, py: 1 }}
                    variant="contained"
                    onClick={handleSave}
                >
                    Sačuvaj
                </Button>

                {formData.status === BlogStatus.Draft ? (
                    <Button
                        size="large"
                        sx={{ px: 5, py: 1 }}
                        variant="outlined"
                        color="success"
                        onClick={handlePublish}
                    >
                        Objavi
                    </Button>
                ) : (
                    <Button
                        size="large"
                        sx={{ px: 5, py: 1 }}
                        variant="outlined"
                        color="warning"
                        onClick={handleUnpublish}
                    >
                        Povuci
                    </Button>
                )}
            </Stack>
        </Stack>
    )
}

export default BlogoviIzmeni
