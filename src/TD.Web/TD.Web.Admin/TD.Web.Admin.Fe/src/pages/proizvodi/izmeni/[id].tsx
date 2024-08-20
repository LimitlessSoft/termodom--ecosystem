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
import { useRouter } from 'next/router'
import { ProizvodiMetaTagsEdit } from '@/widgets'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { PERMISSIONS_GROUPS, USER_PERMISSIONS } from '@/constants'
import { getStatuses } from '@/helpers/productHelpers'
import { IStockType } from '@/widgets/Proizvodi/interfaces/IStockType'
import { IPriceGroup } from '@/widgets/Proizvodi/interfaces/IPriceGroup'
import { IProductGroup } from '@/widgets/Proizvodi/interfaces/IProductGroup'
import { IProductUnit } from '@/widgets/Proizvodi/interfaces/IProductUnit'
import { IEditProductDetails } from '@/widgets/Proizvodi/interfaces/IEditProductDetails'

const textFieldVariant = 'standard'

const ProizvodIzmeni = () => {
    const router = useRouter()
    const permissions = usePermissions(PERMISSIONS_GROUPS.PRODUCTS)
    const productId = router.query.id
    const [units, setUnits] = useState<IProductUnit[]>([])
    const [groups, setGroups] = useState<IProductGroup[]>([])
    const [priceGroups, setPriceGroups] = useState<IPriceGroup[]>([])
    const [stockTypes, setStockTypes] = useState<IStockType[]>([])
    const imagePreviewRef = useRef<any>(null)
    const [checkedGroups, setCheckedGroups] = useState<number[]>([])
    const [imageToUpload, setImageToUpload] = useState<File | null>(null)
    const [isCreating, setIsCreating] = useState(false)
    const [isLoaded, setIsLoaded] = useState(false)
    const [hasAlternateUnit, setHasAlternateUnit] = useState(false)

    const [requestBody, setRequestBody] = useState<IEditProductDetails>({
        name: '',
        src: '',
        image: '',
        id: 0,
        unitId: 0,
        stockType: 0,
        status: 0,
        productPriceGroupId: 0,
        priorityIndex: 0,
        oneAlternatePackageEquals: 0,
        alternateUnitId: 0,
        shortDescription: '',
        description: '',
        catalogId: '',
        classification: 0,
        minWebBase: 0,
        maxWebBase: 0,
        vat: 20,
        metaTitle: '',
        metaDescription: '',
        groups: [],
        canEdit: false,
    })

    useEffect(() => {
        if (!productId) return

        Promise.all([
            adminApi.get(`/units`),
            adminApi.get(`/products-groups`),
            adminApi.get(`/products-prices-groups`),
            adminApi.get(`/product-stock-types`),
            adminApi.get(`/products/${productId}`),
        ])
            .then(
                ([
                    unitsResponse,
                    groupsResponse,
                    priceGroupsResponse,
                    stockTypes,
                    productResponse,
                ]) => {
                    setUnits(unitsResponse.data)
                    setGroups(groupsResponse.data)
                    setPriceGroups(priceGroupsResponse.data)
                    setStockTypes(stockTypes.data)

                    const productData = productResponse.data
                    setHasAlternateUnit(productData.alternateUnitId != null)
                    setRequestBody(productData)
                    setCheckedGroups(productData.groups)

                    setIsLoaded(true)
                }
            )
            .catch((err) => handleApiError(err))
    }, [productId])

    useEffect(() => {
        if (!isLoaded) return

        adminApi
            .get(`/images?image=${requestBody.image}&quality=600`)
            .then((response) => {
                imagePreviewRef.current.src = `data:${response.data.contentType};base64,${response.data.data}`
                setImageToUpload(
                    dataURLtoFile(imagePreviewRef.current.src, 'file')
                )
            })
            .catch((err) => handleApiError(err))
    }, [isLoaded, requestBody.image])

    const dataURLtoFile = (dataurl: any, filename: string) => {
        var arr = dataurl.split(','),
            mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[arr.length - 1]),
            n = bstr.length,
            u8arr = new Uint8Array(n)
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n)
        }
        return new File([u8arr], filename, { type: mime })
    }

    const handleEditProduct = () => {
        setIsCreating(true)

        const updatedRequestBody = { ...requestBody, groups: checkedGroups }

        const formData = new FormData()

        if (imageToUpload) formData.append('Image', imageToUpload)

        adminApi
            .post(`/images`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            })
            .then((imageResponse) => {
                toast.success('Slika uspešno uploadovan-a!')

                const finalRequestBody = {
                    ...updatedRequestBody,
                    image: imageResponse.data,
                }

                toast('Menjam proizvod...')

                return adminApi.put(`/products`, finalRequestBody)
            })
            .then(() => toast.success(`Proizvod uspešno izmenjen!`))
            .catch(handleApiError)
            .finally(() => setIsCreating(false))
    }

    return isLoaded ? (
        <Stack
            direction={'column'}
            alignItems={'center'}
            sx={{ m: 0, '& .MuiTextField-root': { m: 1, width: '40ch' } }}
        >
            <Typography sx={{ m: 2 }} variant="h4">
                Izmeni proizvod
            </Typography>
            <TextField
                id="status"
                select
                required
                value={requestBody.status}
                label="Status"
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, status: e.target.value }
                    })
                }}
                helperText="Izaberite status proizvoda"
            >
                {Object.keys(getStatuses()).map((key: string) => {
                    return (
                        <MenuItem
                            key={`status-option-${getStatuses()[key]}`}
                            value={getStatuses()[key]}
                        >
                            {key
                                .split(/(?=[A-Z])/)
                                .join(' ')
                                .toLocaleUpperCase()}
                        </MenuItem>
                    )
                })}
            </TextField>
            <Card sx={{ maxWidth: 600 }}>
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
                    Promeni sliku
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
                                var fr = new FileReader()
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
                value={requestBody.name}
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, name: e.target.value }
                    })
                }}
                variant={textFieldVariant}
            />

            <TextField
                required
                disabled={
                    !hasPermission(
                        permissions,
                        USER_PERMISSIONS.PROIZVODI.EDIT_SRC
                    )
                }
                id="src"
                value={requestBody.src}
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
                value={requestBody.catalogId}
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
                    value={requestBody.unitId}
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
                        checked={hasAlternateUnit == true}
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
                units == null ? (
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
                    value={requestBody.stockType}
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
                value={requestBody.classification}
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
                value={requestBody.vat}
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
                    value={requestBody.productPriceGroupId}
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
                required
                id="priority-index"
                label="Prioritetni indeks"
                value={requestBody.priorityIndex}
                onChange={(e) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, priorityIndex: e.target.value }
                    })
                }}
                variant={textFieldVariant}
            />

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
                            disabled={
                                !hasPermission(
                                    permissions,
                                    USER_PERMISSIONS.PROIZVODI.EDIT_ALL
                                )
                            }
                            setCheckedGroups={setCheckedGroups}
                            checkedGroups={checkedGroups}
                            groups={groups}
                            parentId={null}
                        />
                    )}
                </Box>
            </Box>

            <ProizvodiMetaTagsEdit
                disabled={
                    !hasPermission(
                        permissions,
                        USER_PERMISSIONS.PROIZVODI.EDIT_META_TAGS
                    )
                }
                metaTagTitle={requestBody.metaTitle}
                metaTagDescription={requestBody.metaDescription}
                onMetaTagTitleChange={(value?: string) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, metaTitle: value }
                    })
                }}
                onMetaTagDescriptionChange={(value?: string) => {
                    setRequestBody((prev: any) => {
                        return { ...prev, metaDescription: value }
                    })
                }}
            />

            <Button
                endIcon={
                    isCreating ? <CircularProgress color="inherit" /> : null
                }
                disabled={isCreating == true}
                size="large"
                sx={{ m: 2, px: 5, py: 1 }}
                variant="contained"
                onClick={handleEditProduct}
            >
                Izmeni
            </Button>
        </Stack>
    ) : (
        <CircularProgress />
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

const Group = (props: any) => {
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
                                disabled={props.disabled}
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
                            disabled={props.disabled}
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

export default ProizvodIzmeni
