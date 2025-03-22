import {
    Box,
    Button,
    Card,
    CardMedia,
    Checkbox,
    CircularProgress,
    FormControlLabel,
    MenuItem,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import React, { useRef } from 'react'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { IProductGroup } from '@/widgets/Proizvodi/interfaces/IProductGroup'
import { IProductUnit } from '@/widgets/Proizvodi/interfaces/IProductUnit'
import { IPriceGroup } from '@/widgets/Proizvodi/interfaces/IPriceGroup'
import { IStockType } from '@/widgets/Proizvodi/interfaces/IStockType'
import { ICreateProductDetails } from '@/widgets/Proizvodi/interfaces/ICreateProductDetails'
import { useRouter } from 'next/router'

const textFieldVariant = 'standard'

const ProizvodiNovi = (): JSX.Element => {
    const [units, setUnits] = useState<IProductUnit[]>([])
    const [groups, setGroups] = useState<IProductGroup[]>([])
    const [priceGroups, setPriceGroups] = useState<IPriceGroup[]>([])
    const [stockTypes, setStockTypes] = useState<IStockType[]>([])
    const imagePreviewRef = useRef<any>(null)
    const [checkedGroups, setCheckedGroups] = useState<number[]>([])
    const [imageToUpload, setImageToUpload] = useState<File | null>(null)
    const [isCreating, setIsCreating] = useState(false)
    const [hasAlternateUnit, setHasAlternateUnit] = useState(false)

    const [requestBody, setRequestBody] = useState<ICreateProductDetails>({
        name: '',
        src: '',
        image: '',
        unitId: 0,
        alternateUnitId: undefined,
        shortDescription: '',
        description: '',
        oneAlternatePackageEquals: 0,
        catalogId: '',
        classification: 0,
        vat: 20,
        productPriceGroupId: 0,
        stockType: 0,
    })

    const router = useRouter()

    useEffect(() => {
        Promise.all([
            adminApi.get(`/units`),
            adminApi.get(`/products-groups`),
            adminApi.get(`/products-prices-groups`),
            adminApi.get(`/product-stock-types`),
        ])
            .then(
                ([
                    unitsResponse,
                    groupsResponse,
                    priceGroupsResponse,
                    stockTypes,
                ]) => {
                    setRequestBody((prev: any) => ({
                        ...prev,
                        unitId: unitsResponse.data[0].id,
                        productPriceGroupId: priceGroupsResponse.data[0].id,
                    }))

                    setUnits(unitsResponse.data)
                    setGroups(groupsResponse.data)
                    setPriceGroups(priceGroupsResponse.data)
                    setStockTypes(stockTypes.data)
                }
            )
            .catch((err) => handleApiError(err))
    }, [])

    const handleCreateNewProduct = () => {
        setIsCreating(true)

        const updatedRequestBody = { ...requestBody, groups: checkedGroups }

        const formData = new FormData()
        if (imageToUpload) {
            formData.append('Image', imageToUpload)
        }

        adminApi
            .post(`/images`, formData, {
                headers: { 'Content-Type': 'multipart/form-data' },
            })
            .then((imageResponse) => {
                toast.success('Slika uspešno uploadovan-a!')

                const finalRequestBody = {
                    ...updatedRequestBody,
                    image: imageResponse.data,
                }

                return adminApi.put('/products', finalRequestBody)
            })
            .then((productResponse) => {
                toast.success(`Proizvod uspešno kreiran!`)
                router.push(`/proizvodi/izmeni/${productResponse.data}`)
            })
            .catch(handleApiError)
            .finally(() => setIsCreating(false))
    }

    return (
        <Stack
            direction={'column'}
            alignItems={'center'}
            sx={{ m: 2, '& .MuiTextField-root': { m: 1, width: '50ch' } }}
        >
            <Typography sx={{ m: 2 }} variant="h4">
                Kreiraj novi proizvod
            </Typography>

            <Card sx={{ width: 600 }}>
                <CardMedia
                    sx={{ objectFit: 'contain' }}
                    component={'img'}
                    alt={'upload-image'}
                    height={'536'}
                    ref={imagePreviewRef}
                    src="https://termodom.rs/img/gallery/source/cdddv.jpg"
                />
                <Button
                    sx={{ width: '100%' }}
                    variant="contained"
                    component="label"
                >
                    Izaberi sliku
                    <input
                        type="file"
                        onChange={(evt) => {
                            let tgt = evt.target
                            let files = tgt.files

                            if (files == null) {
                                toast('Error upload image. Image is null.', {
                                    type: 'error',
                                })
                                return
                            }
                            setImageToUpload(files[0])

                            if (FileReader && files && files.length) {
                                const fr = new FileReader()
                                fr.onload = function () {
                                    imagePreviewRef.current.src = fr.result
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
                id="name"
                label="Ime proizvoda"
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, name: e.target.value }
                    })
                }}
                variant={textFieldVariant}
            />

            <TextField
                required
                id="src"
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, src: e.target.value }
                    })
                }}
                label="link (putanja nakon termodom.rs/grupa/[ovde])"
                variant={textFieldVariant}
            />

            <TextField
                required
                id="catalogId"
                label="Kataloški broj"
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, catalogId: e.target.value }
                    })
                }}
                variant={textFieldVariant}
            />

            {units && units.length > 0 && (
                <TextField
                    id="unit"
                    select
                    required
                    defaultValue={requestBody.unitId}
                    label="Jedinica mere"
                    onChange={(e) => {
                        setRequestBody((prev: any) => {
                            return { ...prev, unitId: e.target.value }
                        })
                    }}
                    helperText="Izaberite jedinicu mere proizvoda"
                >
                    {units.map((unit: any, index: any) => {
                        return (
                            <MenuItem
                                key={`jm-option-${index}`}
                                value={unit.id}
                            >
                                {unit.name}
                            </MenuItem>
                        )
                    })}
                </TextField>
            )}

            <FormControlLabel
                label="Ima alternativnu jedinicu mere"
                control={
                    <Checkbox
                        checked={hasAlternateUnit}
                        onChange={(e) => {
                            setRequestBody((prev: any) => {
                                return {
                                    ...prev,
                                    alternateUnitId: units[0].id,
                                    oneAlternatePackageEquals: 1,
                                }
                            })
                            setHasAlternateUnit(e.target.checked)

                            if (!e.target.checked) {
                                setRequestBody((prev: any) => {
                                    return {
                                        ...prev,
                                        alternateUnitId: null,
                                        oneAlternatePackageEquals: null,
                                    }
                                })
                            }
                        }}
                    />
                }
            />
            {hasAlternateUnit ? (
                units.length === 0 ? (
                    <CircularProgress />
                ) : (
                    <Box>
                        <Box>
                            <TextField
                                id="unit"
                                select
                                required
                                defaultValue={requestBody.alternateUnitId}
                                label="Alternativna jedinica mere"
                                onChange={(e) => {
                                    setRequestBody((prev: any) => {
                                        return {
                                            ...prev,
                                            alternateUnitId: e.target.value,
                                        }
                                    })
                                }}
                                helperText="Izaberite alternativnu jedinicu mere proizvoda"
                            >
                                {units.map((unit: any, index: any) => {
                                    return (
                                        <MenuItem
                                            key={`jm-option-${index}`}
                                            value={unit.id}
                                        >
                                            {unit.name}
                                        </MenuItem>
                                    )
                                })}
                            </TextField>
                        </Box>

                        <TextField
                            required
                            id="vat"
                            label={`1 ${units.find((item: any) => item.id == requestBody.alternateUnitId)?.name} = ${requestBody.oneAlternatePackageEquals} ${units.find((item: any) => item.id == requestBody.unitId)?.name}`}
                            defaultValue={requestBody.oneAlternatePackageEquals}
                            onChange={(e) => {
                                setRequestBody((prev: any) => {
                                    return {
                                        ...prev,
                                        oneAlternatePackageEquals:
                                            e.target.value,
                                    }
                                })
                            }}
                            variant={textFieldVariant}
                        />
                    </Box>
                )
            ) : null}

            {stockTypes && stockTypes.length > 0 && (
                <TextField
                    id="stockType"
                    select
                    required
                    onChange={(e) => {
                        setRequestBody((prev: any) => {
                            return {
                                ...prev,
                                stockType: e.target.value,
                            }
                        })
                    }}
                    label="Tip lagera"
                    helperText="Izaberite tip lagera"
                >
                    {stockTypes.map((stockType: any, index: any) => {
                        return (
                            <MenuItem
                                key={`stock-type-option-${index}`}
                                value={stockType.id}
                            >
                                {stockType.name}
                            </MenuItem>
                        )
                    })}
                </TextField>
            )}

            <TextField
                id="classification"
                select
                required
                label="Klasifikacija"
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, classification: e.target.value }
                    })
                }}
                helperText="Izaberite klasu proizvoda"
            >
                <MenuItem value={0}>Hobi</MenuItem>
                <MenuItem value={1}>Standard</MenuItem>
                <MenuItem value={2}>Profi</MenuItem>
            </TextField>

            <TextField
                required
                id="vat"
                label="PDV"
                defaultValue={20}
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, vat: e.target.value }
                    })
                }}
                variant={textFieldVariant}
            />

            {priceGroups && priceGroups.length > 0 && (
                <TextField
                    id="priceGroup"
                    select
                    required
                    onChange={(e) => {
                        setRequestBody((prev: any) => {
                            return {
                                ...prev,
                                productPriceGroupId: e.target.value,
                            }
                        })
                    }}
                    label="Cenovna grupa proizvoda"
                    helperText="Izaberite cenovnu grupu proizvoda"
                >
                    {priceGroups.map((cenovnaGrupa: any, index: any) => {
                        return (
                            <MenuItem
                                key={`price-group-option-${index}`}
                                value={cenovnaGrupa.id}
                            >
                                {cenovnaGrupa.name}
                            </MenuItem>
                        )
                    })}
                </TextField>
            )}

            <TextField
                id="kratak-opis"
                label="Kratak opis proizvoda"
                defaultValue={requestBody.shortDescription}
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, shortDescription: e.target.value }
                    })
                }}
                variant={textFieldVariant}
            />

            <TextField
                id="pun-opis"
                multiline
                rows={8}
                label="Pun opis proizvoda"
                defaultValue={requestBody.description}
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, description: e.target.value }
                    })
                }}
                variant={`outlined`}
            />

            <Box sx={{ m: 1 }}>
                <Typography variant="caption">
                    Čekiraj grupe/podgrupe
                </Typography>
                <Box>
                    {groups && groups.length > 0 && (
                        <Group
                            setCheckedGroups={setCheckedGroups}
                            checkedGroups={checkedGroups}
                            groups={groups}
                            parentId={null}
                        />
                    )}
                </Box>
            </Box>

            <Button
                endIcon={
                    isCreating ? <CircularProgress color="inherit" /> : null
                }
                disabled={isCreating == true}
                size="large"
                sx={{ m: 2, px: 5, py: 1 }}
                variant="contained"
                onClick={handleCreateNewProduct}
            >
                Kreiraj
            </Button>
        </Stack>
    )
}

const isChecked = (groups: any[], checkedGroups: number[], id: number) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)
    const thisChecked = checkedGroups.find((item) => item == id) != null
    if (thisChecked || subGroups.length === 0) return thisChecked

    const results: any = subGroups.map((sg) => {
        return isChecked(groups, checkedGroups, sg.id)
    })

    return results.find((i: any) => i == true) != null
}

const decheck = (groups: any[], setCheckedGroups: any, id: number) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)

    setCheckedGroups((prev: any) => [
        ...prev.filter((item: any) => item !== id),
    ])
    subGroups.map((sg) => {
        decheck(groups, setCheckedGroups, sg.id)
    })
}

const Group = (props: any): JSX.Element => {
    return props.groups
        .filter((item: any) => item.parentGroupId === props.parentId)
        .map((group: any) => {
            const mxVal = props.parentId == null ? 0 : 4
            return (
                <Box sx={{ mx: mxVal }} key={`group-cb-${group.id}`}>
                    <FormControlLabel
                        label={group.name}
                        control={
                            <Checkbox
                                checked={isChecked(
                                    props.groups,
                                    props.checkedGroups,
                                    group.id
                                )}
                                onChange={(e) => {
                                    if (e.target.checked) {
                                        props.setCheckedGroups((prev: any) => [
                                            ...prev,
                                            group.id,
                                        ])
                                    } else {
                                        decheck(
                                            props.groups,
                                            props.setCheckedGroups,
                                            group.id
                                        )
                                    }
                                }}
                            />
                        }
                    />
                    {props.groups.filter(
                        (item: any) => item.parentGroupId == group.id
                    ).length > 0 ? (
                        <Group
                            setCheckedGroups={props.setCheckedGroups}
                            checkedGroups={props.checkedGroups}
                            groups={props.groups}
                            parentId={group.id}
                        />
                    ) : null}
                </Box>
            )
        })
}

export default ProizvodiNovi
