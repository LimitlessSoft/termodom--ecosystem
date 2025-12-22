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
import { useRef, useState } from 'react'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { useRouter } from 'next/router'
import { BlogStatus, BlogStatusLabels } from '@/widgets/Blogovi/interfaces/IBlog'
import { MarkdownEditor } from '@/widgets/MarkdownEditor'

const textFieldVariant = 'standard'

const BlogoviNovi = () => {
    const router = useRouter()
    const imagePreviewRef = useRef(null)
    const [imageToUpload, setImageToUpload] = useState(null)
    const [isSaving, setIsSaving] = useState(false)

    const [formData, setFormData] = useState({
        title: '',
        text: '',
        slug: '',
        status: BlogStatus.Draft,
        summary: '',
        coverImage: '',
    })

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

            const response = await adminApi.put('/blogs', requestBody)
            toast.success('Blog uspešno kreiran!')
            router.push(`/blogovi/${response.data}`)
        }

        saveRequest()
            .catch(handleApiError)
            .finally(() => setIsSaving(false))
    }

    return (
        <Stack
            direction="column"
            alignItems="center"
            sx={{ m: 2, '& .MuiTextField-root': { m: 1, width: '50ch' } }}
        >
            <Typography sx={{ m: 2 }} variant="h4">
                Kreiraj novi blog
            </Typography>

            <Card sx={{ width: 600 }}>
                <CardMedia
                    sx={{ objectFit: 'contain' }}
                    component="img"
                    alt="upload-image"
                    height="300"
                    ref={imagePreviewRef}
                    src="https://via.placeholder.com/600x300?text=Cover+Image"
                />
                <Button sx={{ width: '100%' }} variant="contained" component="label">
                    Izaberi sliku
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
            >
                {Object.entries(BlogStatus).map(([key, value]) => (
                    <MenuItem key={key} value={value}>
                        {BlogStatusLabels[value]}
                    </MenuItem>
                ))}
            </TextField>

            <Button
                endIcon={isSaving ? <CircularProgress size={20} color="inherit" /> : null}
                disabled={isSaving}
                size="large"
                sx={{ m: 2, px: 5, py: 1 }}
                variant="contained"
                onClick={handleSave}
            >
                Kreiraj
            </Button>
        </Stack>
    )
}

export default BlogoviNovi
