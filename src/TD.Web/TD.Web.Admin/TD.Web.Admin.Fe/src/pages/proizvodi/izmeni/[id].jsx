import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    Card,
    CardMedia,
    Checkbox,
    Chip,
    CircularProgress,
    FormControlLabel,
    IconButton,
    LinearProgress,
    MenuItem,
    Paper,
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
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { getStatuses } from '@/helpers/productHelpers'
import Grid2 from '@mui/material/Unstable_Grid2'
import { Add, ArrowDownward } from '@mui/icons-material'
import { ProizvodiIzmeniVarijacijeProizvoda } from '../../../widgets/Proizvodi/ProizvodiIzmeniVarijacijeProizvoda/ui/proizvodiIzmeniVarijacijeProizvoda'
const textFieldVariant = 'standard'

const ProizvodIzmeni = () => {
    const router = useRouter()
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PRODUCTS
    )
    const productId = router.query.id
    const [units, setUnits] = useState([])
    const [groups, setGroups] = useState([])
    const [priceGroups, setPriceGroups] = useState([])
    const [stockTypes, setStockTypes] = useState([])
    const imagePreviewRef = useRef(null)
    const [checkedGroups, setCheckedGroups] = useState([])
    const [imageToUpload, setImageToUpload] = useState(null)
    const [isCreating, setIsCreating] = useState(false)
    const [isLoaded, setIsLoaded] = useState(false)
    const [hasAlternateUnit, setHasAlternateUnit] = useState(false)

    const [searchKeywordDeleting, setSearchKeywordDeleting] = useState(false)

    const [searchKeywords, setSearchKeywords] = useState(undefined)
    const [newSearchKeyword, setNewSearchKeyword] = useState('')

    const [requestBody, setRequestBody] = useState({
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
                    setSearchKeywords(productData.searchKeywords)

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

    const dataURLtoFile = (dataurl, filename) => {
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
                    setRequestBody((prev) => {
                        return { ...prev, status: e.target.value }
                    })
                }}
                helperText="Izaberite status proizvoda"
            >
                {Object.keys(getStatuses()).map((key) => {
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
                value={requestBody.name}
                onChange={(e) => {
                    setRequestBody((prev) => {
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
                        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PROIZVODI
                            .EDIT_SRC
                    )
                }
                id="src"
                value={requestBody.src}
                onChange={(e) => {
                    setRequestBody((prev) => {
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
                    setRequestBody((prev) => {
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
                        setRequestBody((prev) => {
                            return { ...prev, unitId: e.target.value }
                        })
                    }}
                    helperText="Izaberite jedinicu mere proizvoda"
                >
                    {units.map((unit, index) => {
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
                            setRequestBody((prev) => {
                                return {
                                    ...prev,
                                    alternateUnitId: units[0].id,
                                    oneAlternatePackageEquals: 1,
                                }
                            })
                            setHasAlternateUnit(e.target.checked)

                            if (!e.target.checked) {
                                setRequestBody((prev) => {
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
                                    setRequestBody((prev) => {
                                        return {
                                            ...prev,
                                            alternateUnitId: e.target.value,
                                        }
                                    })
                                }}
                                helperText="Izaberite alternativnu jedinicu mere proizvoda"
                            >
                                {units.map((unit, index) => {
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
                            label={`1 ${units.find((item) => item.id == requestBody.alternateUnitId)?.name} = ${requestBody.oneAlternatePackageEquals} ${units.find((item) => item.id == requestBody.unitId)?.name}`}
                            defaultValue={requestBody.oneAlternatePackageEquals}
                            onChange={(e) => {
                                setRequestBody((prev) => {
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
                        setRequestBody((prev) => {
                            return {
                                ...prev,
                                stockType: e.target.value,
                            }
                        })
                    }}
                    label="Tip lagera"
                    helperText="Izaberite tip lagera"
                >
                    {stockTypes.map((stockType, index) => {
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
                    setRequestBody((prev) => {
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
                    setRequestBody((prev) => {
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
                        setRequestBody((prev) => {
                            return {
                                ...prev,
                                productPriceGroupId: e.target.value,
                            }
                        })
                    }}
                    label="Cenovna grupa proizvoda"
                    helperText="Izaberite cenovnu grupu proizvoda"
                >
                    {priceGroups.map((cenovnaGrupa, index) => {
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
                    setRequestBody((prev) => {
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
                    setRequestBody((prev) => {
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
                    setRequestBody((prev) => {
                        return { ...prev, description: e.target.value }
                    })
                }}
                variant={`outlined`}
            />
            <Box
                sx={{
                    my: 2,
                    maxWidth: 600,
                }}
            >
                <ProizvodiIzmeniVarijacijeProizvoda productId={productId} />
            </Box>

            {searchKeywords === undefined && <LinearProgress />}
            {searchKeywords !== undefined &&
                productId !== undefined &&
                productId !== null && (
                    <Grid2
                        container
                        maxWidth={`400px`}
                        gap={1}
                        p={2}
                        component={Paper}
                    >
                        <Grid2 xs={12}>
                            <Typography>Fraze pretrage</Typography>
                        </Grid2>
                        <Grid2 xs={12}>
                            <Grid2 container gap={1}>
                                {searchKeywords !== null &&
                                    searchKeywords.map((keyword, index) => (
                                        <Chip
                                            key={index}
                                            label={keyword}
                                            disabled={searchKeywordDeleting}
                                            onDelete={() => {
                                                setSearchKeywordDeleting(true)
                                                adminApi
                                                    .delete(
                                                        ENDPOINTS_CONSTANTS.PRODUCTS.SEARCH_KEYWORDS(
                                                            productId?.toString()
                                                        ),
                                                        {
                                                            data: {
                                                                id: productId,
                                                                keyword:
                                                                    keyword,
                                                            },
                                                        }
                                                    )
                                                    .then(() => {
                                                        setSearchKeywords(
                                                            searchKeywords?.filter(
                                                                (item) =>
                                                                    item !==
                                                                    keyword
                                                            )
                                                        )
                                                        toast.success(
                                                            'Uspesno obrisana fraza pretrage!'
                                                        )
                                                    })
                                                    .catch(handleApiError)
                                                    .finally(() => {
                                                        setSearchKeywordDeleting(
                                                            false
                                                        )
                                                    })
                                            }}
                                        />
                                    ))}
                            </Grid2>
                        </Grid2>
                        <Grid2 xs={12}>
                            <Grid2 container>
                                <TextField
                                    sx={{ ml: 1, flex: 1 }}
                                    placeholder="Nova fraza pretrage..."
                                    value={newSearchKeyword}
                                    onChange={(e) => {
                                        setNewSearchKeyword(
                                            e.target.value ?? ''
                                        )
                                    }}
                                />
                                <IconButton
                                    type="button"
                                    sx={{ p: '10px' }}
                                    onClick={() => {
                                        adminApi
                                            .post(
                                                ENDPOINTS_CONSTANTS.PRODUCTS.SEARCH_KEYWORDS(
                                                    productId?.toString()
                                                ),
                                                {
                                                    id: productId,
                                                    keyword:
                                                        newSearchKeyword.toLowerCase(),
                                                }
                                            )
                                            .then(() => {
                                                setSearchKeywords((prev) => {
                                                    return !prev
                                                        ? [newSearchKeyword]
                                                        : prev?.concat(
                                                              newSearchKeyword.toLowerCase()
                                                          )
                                                })
                                                toast.success(
                                                    'Uspesno dodata fraza pretrage!'
                                                )
                                                setNewSearchKeyword('')
                                            })
                                            .catch(handleApiError)
                                    }}
                                >
                                    <Add />
                                </IconButton>
                            </Grid2>
                        </Grid2>
                    </Grid2>
                )}

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
                                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                                        .PROIZVODI.EDIT_ALL
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
                        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PROIZVODI
                            .EDIT_META_TAGS
                    )
                }
                metaTagTitle={requestBody.metaTitle}
                metaTagDescription={requestBody.metaDescription}
                onMetaTagTitleChange={(value) => {
                    setRequestBody((prev) => {
                        return { ...prev, metaTitle: value }
                    })
                }}
                onMetaTagDescriptionChange={(value) => {
                    setRequestBody((prev) => {
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

const isChecked = (groups, checkedGroups, id) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)
    const thisChecked = checkedGroups.find((item) => item == id) != null
    if (thisChecked || subGroups.length === 0) return thisChecked

    const results = subGroups.map((sg) => {
        return isChecked(groups, checkedGroups, sg.id)
    })

    return results.find((i) => i == true) != null
}

const decheck = (groups, setCheckedGroups, id) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)

    setCheckedGroups((prev) => [...prev.filter((item) => item !== id)])
    subGroups.map((sg) => {
        decheck(groups, setCheckedGroups, sg.id)
    })
}

const Group = (props) => {
    return props.groups
        .filter((item) => item.parentGroupId === props.parentId)
        .map((group) => {
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
                                        props.setCheckedGroups((prev) => [
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
                        (item) => item.parentGroupId == group.id
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
