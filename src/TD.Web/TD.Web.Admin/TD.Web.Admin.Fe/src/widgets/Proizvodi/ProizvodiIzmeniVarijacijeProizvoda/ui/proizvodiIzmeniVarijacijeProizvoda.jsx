import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    CircularProgress,
    Divider,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { ArrowDownward } from '@mui/icons-material'
import React, { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '../../../../apis/adminApi'
import { toast } from 'react-toastify'

export const ProizvodiIzmeniVarijacijeProizvoda = ({ productId }) => {
    const [linkInput, setLinkInput] = useState('')
    const [linkedProducts, setLinkedProducts] = useState([])
    const [link, setLink] = useState(undefined)
    const [updating, setUpdating] = useState(false)
    const fetchLinkedProducts = async (
        productId,
        setLinkedProducts,
        setLink
    ) => {
        if (!productId) return
        try {
            const response = await adminApi.get(`/products/${productId}/linked`)
            setLinkedProducts(response.data.linkedProducts)
            setLink(response.data.link || '')
        } catch (error) {
            handleApiError(error)
        } finally {
            setUpdating(false)
        }
    }

    useEffect(() => {
        fetchLinkedProducts(productId, setLinkedProducts, setLink)
    }, [productId])
    if (link === undefined) return <CircularProgress />
    return (
        <Accordion>
            <AccordionSummary expandIcon={<ArrowDownward />}>
                <Typography>
                    Varijacije proizvoda [ {link} ] -{' '}
                    {link === '' ? 0 : linkedProducts.length + 1}
                </Typography>
            </AccordionSummary>
            <AccordionDetails>
                <Stack>
                    <Typography variant={`h6`} gutterBottom>
                        Proizvodi povezani sa ovim proizvodom:
                    </Typography>
                    {link === '' && (
                        <Typography>Nema povezanih proizvoda.</Typography>
                    )}
                    {linkedProducts.length === 0 && link !== '' && (
                        <Typography>
                            Ovo je jedini proizvod sa ovim linkom.
                        </Typography>
                    )}
                    {linkedProducts.map((lp) => (
                        <Box
                            key={lp}
                            sx={{
                                display: 'flex',
                                justifyContent: 'space-between',
                                alignItems: 'center',
                                mb: 1,
                            }}
                        >
                            <Typography>{lp}</Typography>
                        </Box>
                    ))}
                    <Divider sx={{ my: 2 }} />
                    <Typography>
                        Ovo ispod je link. Proizvodi sa istim linkom ce
                        medjusobno biti povezani i biti prikazani tamo u delu
                        iznad unosa kolicine. Promeni/postavi link da povezes
                        ovaj proizvod sa drugim proizvodima. Izmisli link ako je
                        ovo prvi proizvod koji povezujes.
                    </Typography>
                    <Box textAlign={`center`}>
                        <TextField
                            disabled={updating}
                            placeholder={`Unesite link proizvoda`}
                            defaultValue={link}
                            onChange={(e) => setLinkInput(e.target.value)}
                        />
                    </Box>
                    <Button
                        disabled={updating}
                        variant={`contained`}
                        onClick={() => {
                            if (link === '') {
                                if (!linkInput || linkInput.length === 0) {
                                    toast.error('Unesite link proizvoda!')
                                    return
                                }
                                if (!/^[a-zA-Z0-9-_]+$/.test(linkInput)) {
                                    toast.error(
                                        'Link proizvoda moze sadrzati samo slova, brojke, - i _'
                                    )
                                    return
                                }
                                setUpdating(true)
                                adminApi
                                    .post(
                                        `/products/${productId}/linked/${linkInput}`
                                    )
                                    .then((response) => {
                                        toast.success(
                                            'Uspesno povezan proizvod!'
                                        )
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        fetchLinkedProducts(
                                            productId,
                                            setLinkedProducts,
                                            setLink
                                        )
                                    })
                            } else if (link !== '') {
                                if (linkInput === '') {
                                    setUpdating(true)
                                    // remove link
                                    adminApi
                                        .delete(`/products/${productId}/linked`)
                                        .then((response) => {
                                            toast.success(
                                                'Uspesno uklonjen link proizvoda!'
                                            )
                                        })
                                        .catch(handleApiError)
                                        .finally(() => {
                                            fetchLinkedProducts(
                                                productId,
                                                setLinkedProducts,
                                                setLink
                                            )
                                        })
                                } else {
                                    // update link
                                    if (!/^[a-zA-Z0-9-_]+$/.test(linkInput)) {
                                        toast.error(
                                            'Link proizvoda moze sadrzati samo slova, brojke, - i _'
                                        )
                                        return
                                    }
                                    setUpdating(true)
                                    adminApi
                                        .post(
                                            `/products/${productId}/linked/${linkInput}`
                                        )
                                        .then((response) => {
                                            toast.success(
                                                'Uspesno izmenjen link proizvoda!'
                                            )
                                        })
                                        .catch(handleApiError)
                                        .finally(() => {
                                            fetchLinkedProducts(
                                                productId,
                                                setLinkedProducts,
                                                setLink
                                            )
                                        })
                                }
                            }
                        }}
                    >
                        {link === ''
                            ? 'Postavi novi link'
                            : linkInput !== ''
                              ? 'Izmeni link'
                              : 'Ukloni link'}
                    </Button>
                </Stack>
            </AccordionDetails>
        </Accordion>
    )
}
