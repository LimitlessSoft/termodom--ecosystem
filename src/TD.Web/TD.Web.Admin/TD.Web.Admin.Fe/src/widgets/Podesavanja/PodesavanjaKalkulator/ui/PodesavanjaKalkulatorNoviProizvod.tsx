import {
    Accordion,
    AccordionActions,
    AccordionDetails,
    AccordionSummary,
    Autocomplete,
    Box,
    Button,
    LinearProgress,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { ArrowDownward } from '@mui/icons-material'
import { toast } from 'react-toastify'

export const PodesavanjaKalkulatorNoviProizvod = (props: any) => {
    const [products, setProducts] = useState<any>(undefined)
    const [updating, setUpdating] = useState<boolean>(false)
    const [selectedProduct, setSelectedProduct] = useState<any>(undefined)

    useEffect(() => {
        adminApi
            .get(`/products`)
            .then((response) => {
                setProducts(response.data)
            })
            .catch(handleApiError)
    }, [])

    return (
        <Box>
            {!products ? (
                <LinearProgress />
            ) : (
                <Accordion
                    sx={{
                        maxWidth: 400,
                    }}
                >
                    <AccordionSummary expandIcon={<ArrowDownward />}>
                        <Typography>Novi proizvod</Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Autocomplete
                            disabled={updating}
                            defaultValue={products[0]}
                            options={products}
                            onChange={(event, value) => {
                                setSelectedProduct(value)
                            }}
                            isOptionEqualToValue={(option, value) => {
                                return option.id === value.id
                            }}
                            getOptionLabel={(option) => {
                                return `${option.name}`
                            }}
                            renderInput={(params) => (
                                <TextField {...params} label={`Proizvod`} />
                            )}
                        />
                    </AccordionDetails>
                    <AccordionActions>
                        <Button
                            variant={`contained`}
                            onClick={() => {
                                adminApi
                                    .post(`/calculator-items`, {
                                        calculatorType: props.calculatorType,
                                        productId: selectedProduct.id,
                                        quantity: 1,
                                        isPrimary: false,
                                    })
                                    .then(() => {
                                        toast.success(
                                            'Proizvod dodat u kalkulator'
                                        )
                                        props.reload()
                                    })
                                    .catch(handleApiError)
                            }}
                        >
                            Dodaj izabrani proizvod u kalkulator
                        </Button>
                    </AccordionActions>
                </Accordion>
            )}
        </Box>
    )
}
