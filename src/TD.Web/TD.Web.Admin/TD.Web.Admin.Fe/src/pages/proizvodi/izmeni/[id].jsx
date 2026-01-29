import {
    Alert,
    Box,
    Button,
    Card,
    CardMedia,
    Checkbox,
    Chip,
    CircularProgress,
    Collapse,
    Divider,
    FormControlLabel,
    Grid,
    IconButton,
    MenuItem,
    Paper,
    Stack,
    Tab,
    Tabs,
    TextField,
    Typography,
} from '@mui/material'
import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { useRouter } from 'next/router'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { getStatuses } from '@/helpers/productHelpers'
import {
    Add,
    Delete,
    ExpandMore,
    ExpandLess,
    Save,
    CloudUpload,
    Info,
} from '@mui/icons-material'
import { ProizvodiIzmeniVarijacijeProizvoda } from '@/widgets/Proizvodi/ProizvodiIzmeniVarijacijeProizvoda/ui/proizvodiIzmeniVarijacijeProizvoda'

const TabPanel = ({ children, value, index }) => (
    <div role="tabpanel" hidden={value !== index}>
        {value === index && <Box sx={{ py: 3 }}>{children}</Box>}
    </div>
)

const SectionTitle = ({ children }) => (
    <Typography
        variant="subtitle1"
        fontWeight="bold"
        color="text.secondary"
        sx={{ mb: 2 }}
    >
        {children}
    </Typography>
)

const SectionInfo = ({ children }) => (
    <Alert severity="info" icon={<Info fontSize="small" />} sx={{ mb: 3 }}>
        <Typography variant="body2">{children}</Typography>
    </Alert>
)

const ProizvodIzmeni = () => {
    const router = useRouter()
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PRODUCTS
    )
    const productId = router.query.id

    const [tabValue, setTabValue] = useState(0)
    const [units, setUnits] = useState([])
    const [groups, setGroups] = useState([])
    const [priceGroups, setPriceGroups] = useState([])
    const [stockTypes, setStockTypes] = useState([])
    const [imagePreviewUrl, setImagePreviewUrl] = useState('')
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
                    unitsRes,
                    groupsRes,
                    priceGroupsRes,
                    stockTypesRes,
                    productRes,
                ]) => {
                    setUnits(unitsRes.data)
                    setGroups(groupsRes.data)
                    setPriceGroups(priceGroupsRes.data)
                    setStockTypes(stockTypesRes.data)

                    const productData = productRes.data
                    setHasAlternateUnit(productData.alternateUnitId != null)
                    setRequestBody(productData)
                    setCheckedGroups(productData.groups)
                    setSearchKeywords(productData.searchKeywords)
                    setIsLoaded(true)
                }
            )
            .catch(handleApiError)
    }, [productId])

    useEffect(() => {
        if (!isLoaded || !requestBody.image) return

        adminApi
            .get(`/images?image=${requestBody.image}&quality=600`)
            .then((response) => {
                const dataUrl = `data:${response.data.contentType};base64,${response.data.data}`
                setImagePreviewUrl(dataUrl)
                setImageToUpload(dataURLtoFile(dataUrl, 'file'))
            })
            .catch(handleApiError)
    }, [isLoaded, requestBody.image])

    const dataURLtoFile = (dataurl, filename) => {
        const arr = dataurl.split(',')
        const mime = arr[0].match(/:(.*?);/)?.[1] || ''
        const bstr = atob(arr[arr.length - 1])
        let n = bstr.length
        const u8arr = new Uint8Array(n)
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
                headers: { 'Content-Type': 'multipart/form-data' },
            })
            .then((imageResponse) => {
                toast.success('Slika uspešno uploadovana!')
                return adminApi.put(`/products`, {
                    ...updatedRequestBody,
                    image: imageResponse.data,
                })
            })
            .then(() => toast.success(`Proizvod uspešno izmenjen!`))
            .catch(handleApiError)
            .finally(() => setIsCreating(false))
    }

    const handleImageChange = (evt) => {
        const files = evt.target.files
        if (!files) {
            toast.error('Greška pri uploadu slike.')
            return
        }
        setImageToUpload(files[0])
        if (FileReader && files.length) {
            const fr = new FileReader()
            fr.onload = () => {
                setImagePreviewUrl(fr.result)
            }
            fr.readAsDataURL(files[0])
        }
    }

    const handleAddSearchKeyword = () => {
        if (!newSearchKeyword.trim()) return
        adminApi
            .post(
                ENDPOINTS_CONSTANTS.PRODUCTS.SEARCH_KEYWORDS(
                    productId?.toString() || ''
                ),
                {
                    id: productId,
                    keyword: newSearchKeyword.toLowerCase(),
                }
            )
            .then(() => {
                setSearchKeywords((prev) =>
                    prev
                        ? [...prev, newSearchKeyword.toLowerCase()]
                        : [newSearchKeyword.toLowerCase()]
                )
                toast.success('Fraza pretrage uspešno dodata!')
                setNewSearchKeyword('')
            })
            .catch(handleApiError)
    }

    const handleDeleteSearchKeyword = (keyword) => {
        setSearchKeywordDeleting(true)
        adminApi
            .delete(
                ENDPOINTS_CONSTANTS.PRODUCTS.SEARCH_KEYWORDS(
                    productId?.toString() || ''
                ),
                {
                    data: { id: productId, keyword },
                }
            )
            .then(() => {
                setSearchKeywords((prev) =>
                    prev?.filter((item) => item !== keyword)
                )
                toast.success('Fraza pretrage uspešno obrisana!')
            })
            .catch(handleApiError)
            .finally(() => setSearchKeywordDeleting(false))
    }

    const updateField = (field, value) => {
        setRequestBody((prev) => ({ ...prev, [field]: value }))
    }

    const getStatusLabel = (status) => {
        const statuses = getStatuses()
        const key = Object.keys(statuses).find((k) => statuses[k] === status)
        return (
            key
                ?.split(/(?=[A-Z])/)
                .join(' ')
                .toUpperCase() || 'Nepoznat'
        )
    }

    const getStatusColor = (status) => {
        switch (status) {
            case 0:
                return 'success' // vidljiv
            case 1:
                return 'warning' // NoviNaObradi
            case 2:
                return 'warning' // NoviCekaOdobrenje
            case 3:
                return 'info' // AzuriranjeNaObradi
            case 4:
                return 'info' // AzuriranjeCekaOdobrenje
            case 5:
                return 'error' // Sakriven
            default:
                return 'default'
        }
    }

    if (!isLoaded) {
        return (
            <Box
                display="flex"
                justifyContent="center"
                alignItems="center"
                minHeight="50vh"
            >
                <CircularProgress />
            </Box>
        )
    }

    const canEditAll = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PROIZVODI.EDIT_ALL
    )
    const canEditSrc = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PROIZVODI.EDIT_SRC
    )
    const canEditMetaTags = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PROIZVODI.EDIT_META_TAGS
    )

    return (
        <Box sx={{ p: 2 }}>
            {/* Header */}
            <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
                <Stack
                    direction="row"
                    spacing={2}
                    alignItems="center"
                    flexWrap="wrap"
                    justifyContent="space-between"
                >
                    <Stack
                        direction="row"
                        spacing={2}
                        alignItems="center"
                        flexWrap="wrap"
                    >
                        <Typography variant="h5" fontWeight="bold">
                            {requestBody.name || 'Bez naziva'}
                        </Typography>
                        <Chip label={`ID: ${requestBody.id}`} size="small" />
                        <Chip
                            label={requestBody.catalogId || 'Bez kataloškog'}
                            variant="outlined"
                            size="small"
                        />
                        <Chip
                            label={getStatusLabel(requestBody.status)}
                            color={getStatusColor(requestBody.status)}
                            size="small"
                        />
                    </Stack>
                    <Button
                        variant="contained"
                        size="large"
                        startIcon={
                            isCreating ? (
                                <CircularProgress size={20} color="inherit" />
                            ) : (
                                <Save />
                            )
                        }
                        disabled={isCreating}
                        onClick={handleEditProduct}
                    >
                        Sačuvaj izmene
                    </Button>
                </Stack>
            </Paper>

            {/* Main Content */}
            <Paper elevation={2}>
                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                    <Tabs
                        value={tabValue}
                        onChange={(_, newValue) => setTabValue(newValue)}
                        variant="scrollable"
                        scrollButtons="auto"
                    >
                        <Tab label="Osnovni podaci" />
                        <Tab label="Jedinice i lager" />
                        <Tab label="Cene" />
                        <Tab label="Grupe" />
                        <Tab label="SEO" />
                    </Tabs>
                </Box>

                <Box sx={{ p: 3 }}>
                    {/* Tab 0: Basic Info */}
                    <TabPanel value={tabValue} index={0}>
                        <SectionInfo>
                            Ovde unosite osnovne informacije o proizvodu koje će
                            biti vidljive kupcima na sajtu. Naziv i opis su
                            ključni za pretragu i prezentaciju proizvoda.
                        </SectionInfo>

                        <Grid container spacing={4}>
                            <Grid item xs={12} md={5}>
                                <SectionTitle>Slika proizvoda</SectionTitle>
                                <Typography
                                    variant="body2"
                                    color="text.secondary"
                                    sx={{ mb: 2 }}
                                >
                                    Glavna slika proizvoda koja se prikazuje u
                                    katalogu i na stranici proizvoda.
                                    Preporučena rezolucija je 800x800 piksela.
                                    Podržani formati: JPG, PNG, WebP.
                                </Typography>
                                <Card sx={{ position: 'relative' }}>
                                    <CardMedia
                                        component="img"
                                        sx={{
                                            objectFit: 'contain',
                                            height: 350,
                                            bgcolor: 'grey.100',
                                        }}
                                        src={
                                            imagePreviewUrl ||
                                            'https://termodom.rs/img/gallery/source/cdddv.jpg'
                                        }
                                        alt={requestBody.name}
                                    />
                                    <Button
                                        fullWidth
                                        variant="contained"
                                        component="label"
                                        startIcon={<CloudUpload />}
                                        sx={{ borderRadius: 0 }}
                                    >
                                        Promeni sliku
                                        <input
                                            type="file"
                                            accept="image/*"
                                            onChange={handleImageChange}
                                            hidden
                                        />
                                    </Button>
                                </Card>
                            </Grid>

                            <Grid item xs={12} md={7}>
                                <SectionTitle>
                                    Informacije o proizvodu
                                </SectionTitle>
                                <Stack spacing={3}>
                                    <TextField
                                        fullWidth
                                        required
                                        label="Naziv proizvoda"
                                        value={requestBody.name}
                                        onChange={(e) =>
                                            updateField('name', e.target.value)
                                        }
                                        helperText="Pun naziv proizvoda koji će se prikazivati kupcima. Budite precizni i opisni."
                                    />

                                    <Stack
                                        direction={{ xs: 'column', sm: 'row' }}
                                        spacing={2}
                                    >
                                        <TextField
                                            fullWidth
                                            required
                                            disabled={!canEditSrc}
                                            label="URL putanja"
                                            value={requestBody.src}
                                            onChange={(e) =>
                                                updateField(
                                                    'src',
                                                    e.target.value
                                                )
                                            }
                                            helperText="Jedinstvena putanja u URL-u (npr. 'lepak-za-plocice'). Bez razmaka i specijalnih karaktera."
                                        />
                                        <TextField
                                            fullWidth
                                            required
                                            label="Kataloški broj"
                                            value={requestBody.catalogId}
                                            onChange={(e) =>
                                                updateField(
                                                    'catalogId',
                                                    e.target.value
                                                )
                                            }
                                            helperText="Interni kataloški broj ili šifra proizvoda iz sistema."
                                        />
                                    </Stack>

                                    <TextField
                                        fullWidth
                                        select
                                        required
                                        label="Status"
                                        value={requestBody.status}
                                        onChange={(e) =>
                                            updateField(
                                                'status',
                                                Number(e.target.value)
                                            )
                                        }
                                        helperText="Vidljiv = prikazan kupcima. Na obradi/Čeka odobrenje = u procesu obrade. Sakriven = nije vidljiv kupcima."
                                    >
                                        {Object.entries(getStatuses()).map(
                                            ([key, value]) => (
                                                <MenuItem
                                                    key={value}
                                                    value={value}
                                                >
                                                    {key
                                                        .split(/(?=[A-Z])/)
                                                        .join(' ')
                                                        .toUpperCase()}
                                                </MenuItem>
                                            )
                                        )}
                                    </TextField>

                                    <TextField
                                        fullWidth
                                        label="Kratak opis"
                                        value={requestBody.shortDescription}
                                        onChange={(e) =>
                                            updateField(
                                                'shortDescription',
                                                e.target.value
                                            )
                                        }
                                        helperText="Sažet opis (1-2 rečenice) koji se prikazuje u listi proizvoda i rezultatima pretrage."
                                    />

                                    <TextField
                                        fullWidth
                                        multiline
                                        rows={6}
                                        label="Pun opis proizvoda"
                                        value={requestBody.description}
                                        onChange={(e) =>
                                            updateField(
                                                'description',
                                                e.target.value
                                            )
                                        }
                                        helperText="Detaljan opis proizvoda sa specifikacijama, načinom upotrebe i prednostima. Možete koristiti HTML formatiranje."
                                    />

                                    <TextField
                                        fullWidth
                                        type="number"
                                        label="Prioritetni indeks"
                                        value={requestBody.priorityIndex}
                                        onChange={(e) =>
                                            updateField(
                                                'priorityIndex',
                                                Number(e.target.value)
                                            )
                                        }
                                        helperText="Određuje redosled prikaza u kategoriji. Veći broj = proizvod se prikazuje ranije. Podrazumevano: 0."
                                    />
                                </Stack>
                            </Grid>
                        </Grid>

                        <Divider sx={{ my: 4 }} />

                        <SectionTitle>Varijacije proizvoda</SectionTitle>
                        <Typography
                            variant="body2"
                            color="text.secondary"
                            sx={{ mb: 2 }}
                        >
                            Povežite proizvode koji predstavljaju varijacije
                            istog artikla (npr. različite boje, veličine ili
                            pakovanja). Povezani proizvodi će se prikazivati
                            zajedno na stranici proizvoda, omogućavajući kupcima
                            lak izbor.
                        </Typography>
                        <ProizvodiIzmeniVarijacijeProizvoda
                            productId={productId}
                        />

                        <Divider sx={{ my: 4 }} />

                        <SectionTitle>Fraze pretrage</SectionTitle>
                        <Typography
                            variant="body2"
                            color="text.secondary"
                            sx={{ mb: 2 }}
                        >
                            Dodatne ključne reči po kojima kupci mogu pronaći
                            ovaj proizvod kroz pretragu na glavnoj strani sajta.
                            Primer je da na termodom disperziji upisemo MAXIMA,
                            te bilo ko ko pretrazuje maxima, izbacice mu i
                            termodom disperziju.
                        </Typography>

                        <Alert severity="info" sx={{ mb: 2 }}>
                            <Typography variant="body2">
                                <strong>Saveti:</strong> Dodajte varijante
                                naziva (npr. "lepak" i "ljepilo"), brendove,
                                dimenzije, boje i druge atribute po kojima bi
                                kupci mogli tražiti proizvod.
                            </Typography>
                        </Alert>

                        <Stack direction="row" spacing={1} sx={{ mb: 2 }}>
                            <TextField
                                fullWidth
                                size="small"
                                placeholder="Nova fraza pretrage..."
                                value={newSearchKeyword}
                                onChange={(e) =>
                                    setNewSearchKeyword(e.target.value)
                                }
                                onKeyDown={(e) =>
                                    e.key === 'Enter' &&
                                    handleAddSearchKeyword()
                                }
                                helperText="Unesite frazu i pritisnite Enter ili kliknite + za dodavanje."
                            />
                            <IconButton
                                color="primary"
                                onClick={handleAddSearchKeyword}
                                sx={{ mt: -1 }}
                            >
                                <Add />
                            </IconButton>
                        </Stack>

                        <Box
                            sx={{
                                display: 'flex',
                                flexWrap: 'wrap',
                                gap: 1,
                            }}
                        >
                            {searchKeywords?.map((keyword, index) => (
                                <Chip
                                    key={index}
                                    label={keyword}
                                    disabled={searchKeywordDeleting}
                                    onDelete={() =>
                                        handleDeleteSearchKeyword(keyword)
                                    }
                                    deleteIcon={<Delete />}
                                />
                            ))}
                            {(!searchKeywords ||
                                searchKeywords.length === 0) && (
                                <Typography
                                    variant="body2"
                                    color="text.secondary"
                                >
                                    Nema dodatnih fraza pretrage. Dodajte
                                    ključne reči za bolju pronalazivost.
                                </Typography>
                            )}
                        </Box>
                    </TabPanel>

                    {/* Tab 1: Units & Stock */}
                    <TabPanel value={tabValue} index={1}>
                        <SectionInfo>
                            Podesite kako se proizvod meri i prodaje.
                            Alternativna jedinica mere omogućava prodaju u
                            pakovanjima (npr. kutija od 10 komada) dok se cena
                            računa po osnovnoj jedinici.
                        </SectionInfo>

                        <Grid container spacing={4}>
                            <Grid item xs={12} md={6}>
                                <SectionTitle>Jedinica mere</SectionTitle>
                                <Typography
                                    variant="body2"
                                    color="text.secondary"
                                    sx={{ mb: 2 }}
                                >
                                    Osnovna jedinica mere određuje kako se
                                    proizvod prodaje i kako se prikazuje cena.
                                    Npr. komad (KOM), kilogram (KG), metar (M),
                                    litar (L).
                                </Typography>
                                <Stack spacing={3}>
                                    <TextField
                                        fullWidth
                                        select
                                        required
                                        label="Primarna jedinica mere"
                                        value={requestBody.unitId}
                                        onChange={(e) =>
                                            updateField(
                                                'unitId',
                                                Number(e.target.value)
                                            )
                                        }
                                        helperText="Osnovna jedinica po kojoj se formira cena proizvoda."
                                    >
                                        {units.map((unit) => (
                                            <MenuItem
                                                key={unit.id}
                                                value={unit.id}
                                            >
                                                {unit.name}
                                            </MenuItem>
                                        ))}
                                    </TextField>

                                    <FormControlLabel
                                        control={
                                            <Checkbox
                                                checked={hasAlternateUnit}
                                                onChange={(e) => {
                                                    setHasAlternateUnit(
                                                        e.target.checked
                                                    )
                                                    if (e.target.checked) {
                                                        updateField(
                                                            'alternateUnitId',
                                                            units[0]?.id || 0
                                                        )
                                                        updateField(
                                                            'oneAlternatePackageEquals',
                                                            1
                                                        )
                                                    } else {
                                                        setRequestBody(
                                                            (prev) => ({
                                                                ...prev,
                                                                alternateUnitId: 0,
                                                                oneAlternatePackageEquals: 0,
                                                            })
                                                        )
                                                    }
                                                }}
                                            />
                                        }
                                        label="Ima alternativnu jedinicu mere (pakovanje)"
                                    />

                                    <Collapse in={hasAlternateUnit}>
                                        <Alert severity="info" sx={{ mb: 2 }}>
                                            <Typography variant="body2">
                                                Alternativna jedinica omogućava
                                                prodaju u pakovanjima. Na
                                                primer, ako prodajete pločice po
                                                kvadratnom metru, ali ih
                                                isporučujete u kutijama,
                                                postavite kutiju kao
                                                alternativnu jedinicu i
                                                definišite koliko m² ima u
                                                jednoj kutiji.
                                            </Typography>
                                        </Alert>
                                        <Stack spacing={2} sx={{ pl: 4 }}>
                                            <TextField
                                                fullWidth
                                                select
                                                required
                                                label="Alternativna jedinica mere"
                                                value={
                                                    requestBody.alternateUnitId ||
                                                    ''
                                                }
                                                onChange={(e) =>
                                                    updateField(
                                                        'alternateUnitId',
                                                        Number(e.target.value)
                                                    )
                                                }
                                                helperText="Jedinica pakovanja (npr. kutija, paleta, vreća)."
                                            >
                                                {units.map((unit) => (
                                                    <MenuItem
                                                        key={unit.id}
                                                        value={unit.id}
                                                    >
                                                        {unit.name}
                                                    </MenuItem>
                                                ))}
                                            </TextField>

                                            <TextField
                                                fullWidth
                                                type="number"
                                                label={`Količina osnovne jedinice u pakovanju`}
                                                value={
                                                    requestBody.oneAlternatePackageEquals
                                                }
                                                onChange={(e) =>
                                                    updateField(
                                                        'oneAlternatePackageEquals',
                                                        Number(e.target.value)
                                                    )
                                                }
                                                helperText={`Koliko ${units.find((u) => u.id === requestBody.unitId)?.name || 'jedinica'} sadrži jedna ${units.find((u) => u.id === requestBody.alternateUnitId)?.name || 'pakovanje'}?`}
                                            />
                                        </Stack>
                                    </Collapse>
                                </Stack>
                            </Grid>

                            <Grid item xs={12} md={6}>
                                <SectionTitle>
                                    Lager i klasifikacija
                                </SectionTitle>
                                <Typography
                                    variant="body2"
                                    color="text.secondary"
                                    sx={{ mb: 2 }}
                                >
                                    Tip lagera određuje kako se prikazuje
                                    dostupnost proizvoda. Ovo odredjuje hoce li
                                    se prikazivati u korpi ikonica da je moguce
                                    da nema na stanju itd.
                                </Typography>
                                <Stack spacing={3}>
                                    <TextField
                                        fullWidth
                                        select
                                        required
                                        label="Tip lagera"
                                        value={requestBody.stockType}
                                        onChange={(e) =>
                                            updateField(
                                                'stockType',
                                                Number(e.target.value)
                                            )
                                        }
                                        helperText="Određuje kako se prikazuje stanje zaliha kupcima (na stanju, po porudžbini, itd.)."
                                    >
                                        {stockTypes.map((st) => (
                                            <MenuItem key={st.id} value={st.id}>
                                                {st.name}
                                            </MenuItem>
                                        ))}
                                    </TextField>

                                    <TextField
                                        fullWidth
                                        select
                                        required
                                        label="Klasifikacija"
                                        value={requestBody.classification}
                                        onChange={(e) =>
                                            updateField(
                                                'classification',
                                                Number(e.target.value)
                                            )
                                        }
                                        helperText="Hobi = za povremenu upotrebu, Standard = za redovnu upotrebu, Profi = za profesionalce i zahtevne projekte."
                                    >
                                        <MenuItem value={0}>Hobi</MenuItem>
                                        <MenuItem value={1}>Standard</MenuItem>
                                        <MenuItem value={2}>Profi</MenuItem>
                                    </TextField>
                                </Stack>
                            </Grid>
                        </Grid>
                    </TabPanel>

                    {/* Tab 2: Pricing */}
                    <TabPanel value={tabValue} index={2}>
                        <SectionInfo>
                            Podesite poreske i cenovne parametre proizvoda.
                            Cenovna grupa određuje rabatnu politiku i pravila
                            formiranja cena za različite tipove kupaca.
                        </SectionInfo>

                        <Grid container spacing={4}>
                            <Grid item xs={12} md={6}>
                                <SectionTitle>Cenovni podaci</SectionTitle>
                                <Typography
                                    variant="body2"
                                    color="text.secondary"
                                    sx={{ mb: 2 }}
                                >
                                    PDV se automatski obračunava pri formiranju
                                    maloprodajne cene. Cenovna grupa određuje
                                    koje rabatne politike važe za ovaj proizvod.
                                </Typography>
                                <Stack spacing={3}>
                                    <TextField
                                        fullWidth
                                        type="number"
                                        required
                                        label="PDV (%)"
                                        value={requestBody.vat}
                                        onChange={(e) =>
                                            updateField(
                                                'vat',
                                                Number(e.target.value)
                                            )
                                        }
                                        helperText="Stopa poreza na dodatu vrednost. Standardna stopa u Srbiji je 20%. Za određene proizvode može biti 10% ili 0%."
                                    />

                                    <TextField
                                        fullWidth
                                        select
                                        required
                                        label="Cenovna grupa"
                                        value={requestBody.productPriceGroupId}
                                        onChange={(e) =>
                                            updateField(
                                                'productPriceGroupId',
                                                Number(e.target.value)
                                            )
                                        }
                                        helperText="Grupa koja određuje rabatnu politiku i pravila formiranja cena za različite kategorije kupaca."
                                    >
                                        {priceGroups.map((pg) => (
                                            <MenuItem key={pg.id} value={pg.id}>
                                                {pg.name}
                                            </MenuItem>
                                        ))}
                                    </TextField>
                                </Stack>
                            </Grid>
                        </Grid>
                    </TabPanel>

                    {/* Tab 3: Groups */}
                    <TabPanel value={tabValue} index={3}>
                        <SectionInfo>
                            Dodelite proizvod odgovarajućim kategorijama.
                            Proizvod može pripadati više kategorija istovremeno.
                            Pravilna kategorizacija pomaže kupcima da lakše
                            pronađu proizvod.
                        </SectionInfo>

                        <SectionTitle>Kategorije proizvoda</SectionTitle>
                        <Typography
                            variant="body2"
                            color="text.secondary"
                            sx={{ mb: 2 }}
                        >
                            Označite sve kategorije i podkategorije kojima ovaj
                            proizvod pripada. Ako označite nadkategoriju,
                            proizvod će se automatski prikazivati i u njoj.
                            Preporučuje se označavanje najspecifičnije
                            podkategorije.
                        </Typography>
                        <Paper
                            variant="outlined"
                            sx={{ p: 2, maxHeight: 500, overflow: 'auto' }}
                        >
                            <GroupTree
                                disabled={!canEditAll}
                                groups={groups}
                                checkedGroups={checkedGroups}
                                setCheckedGroups={setCheckedGroups}
                                parentId={null}
                            />
                        </Paper>
                        {!canEditAll && (
                            <Alert severity="warning" sx={{ mt: 2 }}>
                                Nemate dozvolu za izmenu kategorija proizvoda.
                                Obratite se administratoru.
                            </Alert>
                        )}
                    </TabPanel>

                    {/* Tab 4: SEO */}
                    <TabPanel value={tabValue} index={4}>
                        <SectionInfo>
                            SEO (Search Engine Optimization) podešavanja pomažu
                            da se proizvod bolje rangira u pretraživačima poput
                            Google-a. Dobro optimizovan proizvod privlači više
                            organskog saobraćaja.
                        </SectionInfo>

                        <Grid container spacing={4}>
                            <Grid item xs={12} md={6}>
                                <SectionTitle>Meta tagovi</SectionTitle>
                                <Alert severity={`warning`}>
                                    NE DIRAJ META TAGOVI SEKCIJU!
                                </Alert>
                                <Typography
                                    variant="body2"
                                    color="text.secondary"
                                    sx={{ mb: 2 }}
                                >
                                    Meta tagovi se prikazuju u rezultatima
                                    pretrage. Naslov (title) treba da bude
                                    kratak i privlačan, a opis (description)
                                    treba da sadrži ključne informacije i poziv
                                    na akciju.
                                </Typography>
                                <Stack spacing={3}>
                                    <TextField
                                        fullWidth
                                        disabled={!canEditMetaTags}
                                        label="Meta Title"
                                        value={requestBody.metaTitle}
                                        onChange={(e) =>
                                            updateField(
                                                'metaTitle',
                                                e.target.value
                                            )
                                        }
                                        helperText={`Naslov za pretraživače. Optimalna dužina: 50-60 karaktera. Trenutno: ${requestBody.metaTitle?.length || 0} karaktera.`}
                                        inputProps={{ maxLength: 70 }}
                                    />

                                    <TextField
                                        fullWidth
                                        disabled={!canEditMetaTags}
                                        multiline
                                        rows={3}
                                        label="Meta Description"
                                        value={requestBody.metaDescription}
                                        onChange={(e) =>
                                            updateField(
                                                'metaDescription',
                                                e.target.value
                                            )
                                        }
                                        helperText={`Opis za pretraživače. Optimalna dužina: 150-160 karaktera. Trenutno: ${requestBody.metaDescription?.length || 0} karaktera.`}
                                        inputProps={{ maxLength: 170 }}
                                    />
                                </Stack>
                                {!canEditMetaTags && (
                                    <Alert severity="warning" sx={{ mt: 2 }}>
                                        Nemate dozvolu za izmenu meta tagova.
                                        Obratite se administratoru.
                                    </Alert>
                                )}
                            </Grid>
                        </Grid>
                    </TabPanel>
                </Box>
            </Paper>
        </Box>
    )
}

// Helper functions
const isChecked = (groups, checkedGroups, id) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)
    const thisChecked = checkedGroups.includes(id)
    if (thisChecked || subGroups.length === 0) return thisChecked
    return subGroups.some((sg) => isChecked(groups, checkedGroups, sg.id))
}

const decheck = (groups, setCheckedGroups, id) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)
    setCheckedGroups((prev) => prev.filter((item) => item !== id))
    subGroups.forEach((sg) => decheck(groups, setCheckedGroups, sg.id))
}

const GroupTree = ({
    disabled,
    groups,
    checkedGroups,
    setCheckedGroups,
    parentId,
    level = 0,
}) => {
    const [expanded, setExpanded] = useState({})
    const children = groups.filter((item) => item.parentGroupId === parentId)

    if (children.length === 0) return null

    return (
        <Box sx={{ ml: level > 0 ? 3 : 0 }}>
            {children.map((group) => {
                const hasChildren = groups.some(
                    (g) => g.parentGroupId === group.id
                )
                const isExpanded = expanded[group.id] ?? true

                return (
                    <Box key={group.id}>
                        <Stack direction="row" alignItems="center" spacing={0}>
                            {hasChildren && (
                                <IconButton
                                    size="small"
                                    onClick={() =>
                                        setExpanded((prev) => ({
                                            ...prev,
                                            [group.id]: !isExpanded,
                                        }))
                                    }
                                >
                                    {isExpanded ? (
                                        <ExpandLess fontSize="small" />
                                    ) : (
                                        <ExpandMore fontSize="small" />
                                    )}
                                </IconButton>
                            )}
                            {!hasChildren && <Box sx={{ width: 28 }} />}
                            <FormControlLabel
                                label={group.name}
                                control={
                                    <Checkbox
                                        disabled={disabled}
                                        checked={isChecked(
                                            groups,
                                            checkedGroups,
                                            group.id
                                        )}
                                        onChange={(e) => {
                                            if (e.target.checked) {
                                                setCheckedGroups((prev) => [
                                                    ...prev,
                                                    group.id,
                                                ])
                                            } else {
                                                decheck(
                                                    groups,
                                                    setCheckedGroups,
                                                    group.id
                                                )
                                            }
                                        }}
                                    />
                                }
                            />
                        </Stack>
                        <Collapse in={isExpanded}>
                            <GroupTree
                                disabled={disabled}
                                groups={groups}
                                checkedGroups={checkedGroups}
                                setCheckedGroups={setCheckedGroups}
                                parentId={group.id}
                                level={level + 1}
                            />
                        </Collapse>
                    </Box>
                )
            })}
        </Box>
    )
}

export default ProizvodIzmeni
