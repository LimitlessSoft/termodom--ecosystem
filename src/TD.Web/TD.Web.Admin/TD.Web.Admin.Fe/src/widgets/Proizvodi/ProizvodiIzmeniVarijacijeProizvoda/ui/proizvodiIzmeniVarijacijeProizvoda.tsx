import {
    Box,
    Button,
    Chip,
    CircularProgress,
    Collapse,
    IconButton,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { ExpandMore, ExpandLess, Link as LinkIcon, LinkOff } from '@mui/icons-material'
import React, { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { toast } from 'react-toastify'

interface Props {
    productId: string | string[] | undefined
}

export const ProizvodiIzmeniVarijacijeProizvoda = ({ productId }: Props) => {
    const [linkInput, setLinkInput] = useState('')
    const [linkedProducts, setLinkedProducts] = useState<string[]>([])
    const [link, setLink] = useState<string | undefined>(undefined)
    const [updating, setUpdating] = useState(false)
    const [expanded, setExpanded] = useState(true)

    const fetchLinkedProducts = async () => {
        if (!productId) return
        try {
            const response = await adminApi.get(`/products/${productId}/linked`)
            setLinkedProducts(response.data.linkedProducts)
            setLink(response.data.link || '')
            setLinkInput(response.data.link || '')
        } catch (error) {
            handleApiError(error)
        } finally {
            setUpdating(false)
        }
    }

    useEffect(() => {
        fetchLinkedProducts()
    }, [productId])

    const handleSaveLink = () => {
        if (link === '') {
            // Setting new link
            if (!linkInput || linkInput.length === 0) {
                toast.error('Unesite link proizvoda!')
                return
            }
            if (!/^[a-zA-Z0-9-_]+$/.test(linkInput)) {
                toast.error('Link može sadržati samo slova, brojeve, - i _')
                return
            }
            setUpdating(true)
            adminApi
                .post(`/products/${productId}/linked/${linkInput}`)
                .then(() => {
                    toast.success('Proizvod uspešno povezan!')
                    fetchLinkedProducts()
                })
                .catch(handleApiError)
                .finally(() => setUpdating(false))
        } else if (linkInput === '') {
            // Removing link
            setUpdating(true)
            adminApi
                .delete(`/products/${productId}/linked`)
                .then(() => {
                    toast.success('Link uspešno uklonjen!')
                    fetchLinkedProducts()
                })
                .catch(handleApiError)
                .finally(() => setUpdating(false))
        } else if (linkInput !== link) {
            // Updating link
            if (!/^[a-zA-Z0-9-_]+$/.test(linkInput)) {
                toast.error('Link može sadržati samo slova, brojeve, - i _')
                return
            }
            setUpdating(true)
            adminApi
                .post(`/products/${productId}/linked/${linkInput}`)
                .then(() => {
                    toast.success('Link uspešno izmenjen!')
                    fetchLinkedProducts()
                })
                .catch(handleApiError)
                .finally(() => setUpdating(false))
        }
    }

    if (link === undefined) {
        return (
            <Paper variant="outlined" sx={{ p: 3, display: 'flex', justifyContent: 'center' }}>
                <CircularProgress size={24} />
            </Paper>
        )
    }

    const hasLink = link !== ''
    const linkedCount = hasLink ? linkedProducts.length + 1 : 0
    const linkChanged = linkInput !== link

    return (
        <Paper variant="outlined">
            <Box
                sx={{
                    p: 2,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    cursor: 'pointer',
                    '&:hover': { bgcolor: 'action.hover' },
                }}
                onClick={() => setExpanded(!expanded)}
            >
                <Stack direction="row" spacing={2} alignItems="center">
                    <LinkIcon color={hasLink ? 'primary' : 'disabled'} />
                    <Typography fontWeight="medium">Varijacije proizvoda</Typography>
                    {hasLink && (
                        <>
                            <Chip label={link} size="small" variant="outlined" />
                            <Chip label={`${linkedCount} proizvod${linkedCount === 1 ? '' : 'a'}`} size="small" />
                        </>
                    )}
                    {!hasLink && (
                        <Typography variant="body2" color="text.secondary">
                            Nije povezan
                        </Typography>
                    )}
                </Stack>
                <IconButton size="small">
                    {expanded ? <ExpandLess /> : <ExpandMore />}
                </IconButton>
            </Box>

            <Collapse in={expanded}>
                <Box sx={{ p: 2, pt: 0, borderTop: 1, borderColor: 'divider' }}>
                    {hasLink && linkedProducts.length > 0 && (
                        <Box sx={{ mb: 3 }}>
                            <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                                Povezani proizvodi:
                            </Typography>
                            <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
                                {linkedProducts.map((lp) => (
                                    <Chip key={lp} label={lp} size="small" variant="outlined" />
                                ))}
                            </Stack>
                        </Box>
                    )}

                    {hasLink && linkedProducts.length === 0 && (
                        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                            Ovo je jedini proizvod sa ovim linkom.
                        </Typography>
                    )}

                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                        Proizvodi sa istim linkom biće međusobno povezani kao varijacije.
                    </Typography>

                    <Stack direction="row" spacing={2} alignItems="flex-start">
                        <TextField
                            size="small"
                            fullWidth
                            disabled={updating}
                            placeholder="Unesite link za povezivanje"
                            value={linkInput}
                            onChange={(e) => setLinkInput(e.target.value)}
                            helperText="Samo slova, brojevi, - i _"
                            sx={{ maxWidth: 300 }}
                        />
                        {hasLink ? (
                            <Stack direction="row" spacing={1}>
                                {linkChanged && linkInput && (
                                    <Button
                                        variant="contained"
                                        disabled={updating}
                                        onClick={handleSaveLink}
                                        startIcon={updating ? <CircularProgress size={16} /> : <LinkIcon />}
                                    >
                                        Izmeni
                                    </Button>
                                )}
                                <Button
                                    variant="outlined"
                                    color="error"
                                    disabled={updating}
                                    onClick={() => {
                                        setUpdating(true)
                                        adminApi
                                            .delete(`/products/${productId}/linked`)
                                            .then(() => {
                                                toast.success('Link uspešno uklonjen!')
                                                fetchLinkedProducts()
                                            })
                                            .catch(handleApiError)
                                            .finally(() => setUpdating(false))
                                    }}
                                    startIcon={<LinkOff />}
                                >
                                    Ukloni
                                </Button>
                            </Stack>
                        ) : (
                            <Button
                                variant="contained"
                                disabled={updating || !linkInput}
                                onClick={handleSaveLink}
                                startIcon={updating ? <CircularProgress size={16} /> : <LinkIcon />}
                            >
                                Poveži
                            </Button>
                        )}
                    </Stack>
                </Box>
            </Collapse>
        </Paper>
    )
}
