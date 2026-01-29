import {
    Box,
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    Divider,
    Grid,
    MenuItem,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import { useEffect, useRef, useState } from 'react'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { asUtcString } from '@/helpers/dateHelpers'
import dayjs from 'dayjs'
import moment from 'moment'
import { Delete, Lock, ShoppingCart, Analytics } from '@mui/icons-material'
import { useRouter } from 'next/router'

interface KorisnikOsnovniPodaciProps {
    user: any
    disabled: boolean
    onReloadRequest: () => void
}

export const KorisnikOsnovniPodaci = ({ user, disabled, onReloadRequest }: KorisnikOsnovniPodaciProps) => {
    const router = useRouter()
    const putUserRequest = useRef<any>({})

    const [professions, setProfessions] = useState<any[] | undefined>(undefined)
    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const [cities, setCities] = useState<any[] | undefined>(undefined)
    const [paymentTypes, setPaymentTypes] = useState<any[] | undefined>(undefined)
    const [userTypes, setUserTypes] = useState<any[] | undefined>(undefined)

    const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false)
    const [isPasswordDialogOpen, setIsPasswordDialogOpen] = useState(false)
    const [newPassword, setNewPassword] = useState('')

    useEffect(() => {
        Promise.all([
            adminApi.get(`/professions?sortColumn=Name`),
            adminApi.get(`/stores?sortColumn=Name`),
            adminApi.get(`/cities?sortColumn=Name`),
            adminApi.get(`/payment-types?sortColumn=Name`),
            adminApi.get(`/user-types`),
        ])
            .then(([professions, stores, cities, paymentTypes, userTypes]) => {
                setProfessions(professions.data)
                setStores(stores.data)
                setCities(cities.data)
                setPaymentTypes(paymentTypes.data)
                setUserTypes(userTypes.data)
            })
            .catch((err) => handleApiError(err))
    }, [])

    useEffect(() => {
        if (!user) return
        putUserRequest.current = {
            id: user.id,
            username: user.username,
            nickname: user.nickname,
            professionId: user.profession?.id,
            ppid: user.ppid,
            dateOfBirth: user.dateOfBirth,
            cityId: user.city?.id,
            address: user.address,
            mobile: user.mobile,
            mail: user.mail,
            favoriteStoreId: user.favoriteStore?.id,
            comment: user.comment,
            type: user.type,
            referentId: user.referentId,
            defaultPaymentTypeId: user.defaultPaymentTypeId,
        }
    }, [user])

    const handleSave = () => {
        adminApi
            .put(`/users`, putUserRequest.current)
            .then(() => toast.success('Korisnik uspešno ažuriran!'))
            .catch((err) => handleApiError(err))
    }

    const handleChangePassword = () => {
        if (!newPassword || newPassword.length < 6) {
            toast.error('Lozinka mora imati najmanje 6 karaktera')
            return
        }
        adminApi
            .put(`/users/${user.username}/password`, { username: user.username, password: newPassword })
            .then(() => {
                toast.success('Lozinka uspešno promenjena!')
                setIsPasswordDialogOpen(false)
                setNewPassword('')
            })
            .catch((err) => handleApiError(err))
    }

    const handleDelete = () => {
        adminApi
            .put(`/users/${user.username}/status/false`)
            .then(() => {
                toast.success('Korisnik uspešno obrisan!')
                router.push('/korisnici')
            })
            .catch((err) => handleApiError(err))
    }

    const handleApprove = () => {
        adminApi
            .put(`/users/${user.username}/approve`)
            .then(() => {
                toast.success('Korisnik uspešno odobren!')
                onReloadRequest()
            })
            .catch((err) => handleApiError(err))
    }

    const handleGetOwnership = () => {
        adminApi
            .put(`/users/${user.username}/get-ownership`)
            .then(() => {
                toast.success('Uspešno postavljen referent!')
                onReloadRequest()
            })
            .catch((err) => handleApiError(err))
    }

    const handleUserTypeChange = (newType: number) => {
        adminApi
            .put(`/users/${user.username}/type/${newType}`)
            .then(() => toast.success('Tip korisnika uspešno promenjen!'))
            .catch((err) => handleApiError(err))
    }

    const isLoading = !professions || !stores || !cities || !paymentTypes || !userTypes

    if (isLoading) {
        return (
            <Box display="flex" justifyContent="center" p={4}>
                <CircularProgress />
            </Box>
        )
    }

    return (
        <Grid container spacing={3}>
            {/* Left Column - Info & Actions */}
            <Grid item xs={12} md={4}>
                <Paper variant="outlined" sx={{ p: 2 }}>
                    <Typography variant="h6" gutterBottom>Informacije</Typography>
                    <Divider sx={{ mb: 2 }} />

                    <Stack spacing={1.5}>
                        <Typography variant="body2" color="text.secondary">
                            Datum kreiranja: {moment(asUtcString(user.createdAt)).format('DD.MM.YYYY HH:mm')}
                        </Typography>
                        <Typography
                            variant="body2"
                            color={user.processingDate ? 'text.secondary' : 'info.main'}
                            fontWeight={!user.processingDate ? 'bold' : 'normal'}
                        >
                            Datum odobrenja: {user.processingDate
                                ? moment(asUtcString(user.processingDate)).format('DD.MM.YYYY HH:mm')
                                : 'Nije odobren'}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            Poslednji put viđen: {user.lastTimeSeen
                                ? moment(asUtcString(user.lastTimeSeen)).format('DD.MM.YYYY HH:mm')
                                : 'Nikada'}
                        </Typography>
                    </Stack>

                    {user.isActive && (
                        <>
                            <Divider sx={{ my: 2 }} />
                            <Typography variant="h6" gutterBottom>Akcije</Typography>
                            <Stack spacing={1}>
                                {user.amIOwner && !user.processingDate && (
                                    <Button
                                        variant="contained"
                                        color="success"
                                        fullWidth
                                        onClick={handleApprove}
                                    >
                                        Odobri korisnika
                                    </Button>
                                )}
                                {user.referent === 'bez referenta' && (
                                    <Button
                                        variant="outlined"
                                        fullWidth
                                        onClick={handleGetOwnership}
                                    >
                                        Postani referent
                                    </Button>
                                )}
                                <Button
                                    variant="outlined"
                                    startIcon={<Lock />}
                                    fullWidth
                                    onClick={() => setIsPasswordDialogOpen(true)}
                                >
                                    Promeni lozinku
                                </Button>
                                <Button
                                    variant="outlined"
                                    startIcon={<ShoppingCart />}
                                    fullWidth
                                    onClick={() => router.push(`/korisnici/${user.username}/porudzbine`)}
                                >
                                    Porudžbine
                                </Button>
                                <Button
                                    variant="outlined"
                                    startIcon={<Analytics />}
                                    fullWidth
                                    onClick={() => router.push(`/korisnici/${user.username}/analiza`)}
                                >
                                    Analiza
                                </Button>
                                <Button
                                    variant="outlined"
                                    color="error"
                                    startIcon={<Delete />}
                                    fullWidth
                                    onClick={() => setIsDeleteDialogOpen(true)}
                                >
                                    Obriši korisnika
                                </Button>
                            </Stack>
                        </>
                    )}
                </Paper>
            </Grid>

            {/* Right Column - Form */}
            <Grid item xs={12} md={8}>
                <Paper variant="outlined" sx={{ p: 2 }}>
                    <Typography variant="h6" gutterBottom>Podaci korisnika</Typography>
                    <Divider sx={{ mb: 2 }} />

                    <Grid container spacing={2}>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                disabled={disabled}
                                label="Nadimak"
                                defaultValue={user.nickname}
                                onChange={(e) => putUserRequest.current.nickname = e.target.value}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                select
                                disabled={disabled}
                                label="Tip korisnika"
                                defaultValue={user.type}
                                onChange={(e) => handleUserTypeChange(parseInt(e.target.value))}
                            >
                                {userTypes.map((ut: any) => (
                                    <MenuItem key={ut.id} value={ut.id}>{ut.name}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                select
                                disabled={disabled}
                                label="Zanimanje"
                                defaultValue={user.profession?.id}
                                onChange={(e) => putUserRequest.current.professionId = e.target.value}
                            >
                                {professions.map((p: any) => (
                                    <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                disabled={disabled}
                                label="PPID"
                                defaultValue={user.ppid}
                                onChange={(e) => putUserRequest.current.ppid = e.target.value}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <DatePicker
                                disabled={disabled}
                                label="Datum rođenja"
                                defaultValue={dayjs(user.dateOfBirth)}
                                onChange={(e: any) => putUserRequest.current.dateOfBirth = e}
                                slotProps={{ textField: { fullWidth: true } }}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                select
                                disabled={disabled}
                                label="Mesto"
                                defaultValue={user.city?.id}
                                onChange={(e) => putUserRequest.current.cityId = e.target.value}
                            >
                                {cities.map((c: any) => (
                                    <MenuItem key={c.id} value={c.id}>{c.name}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                disabled={disabled}
                                label="Adresa"
                                defaultValue={user.address}
                                onChange={(e) => putUserRequest.current.address = e.target.value}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                disabled={disabled}
                                label="Mobilni"
                                defaultValue={user.mobile}
                                onChange={(e) => putUserRequest.current.mobile = e.target.value}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                disabled={disabled}
                                label="Email"
                                defaultValue={user.mail}
                                onChange={(e) => putUserRequest.current.mail = e.target.value}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                select
                                disabled={disabled}
                                label="Omiljena radnja"
                                defaultValue={user.favoriteStore?.id}
                                onChange={(e) => putUserRequest.current.favoriteStoreId = e.target.value}
                            >
                                {stores.map((s: any) => (
                                    <MenuItem key={s.id} value={s.id}>{s.name}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                fullWidth
                                select
                                disabled={disabled}
                                label="Podrazumevani način plaćanja"
                                defaultValue={user.defaultPaymentTypeId}
                                onChange={(e) => putUserRequest.current.defaultPaymentTypeId = e.target.value}
                            >
                                {paymentTypes.map((p: any) => (
                                    <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                multiline
                                minRows={3}
                                disabled={disabled}
                                label="Komentar"
                                defaultValue={user.comment}
                                onChange={(e) => putUserRequest.current.comment = e.target.value}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Button
                                variant="contained"
                                disabled={disabled}
                                onClick={handleSave}
                            >
                                Sačuvaj izmene
                            </Button>
                        </Grid>
                    </Grid>
                </Paper>
            </Grid>

            {/* Delete Dialog */}
            <Dialog open={isDeleteDialogOpen} onClose={() => setIsDeleteDialogOpen(false)}>
                <DialogTitle>Brisanje korisnika</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Da li ste sigurni da želite da obrišete korisnika {user.nickname}?
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setIsDeleteDialogOpen(false)}>Odustani</Button>
                    <Button onClick={handleDelete} color="error" variant="contained">
                        Obriši
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Password Dialog */}
            <Dialog open={isPasswordDialogOpen} onClose={() => setIsPasswordDialogOpen(false)}>
                <DialogTitle>Promena lozinke</DialogTitle>
                <DialogContent>
                    <DialogContentText sx={{ mb: 2 }}>
                        Unesite novu lozinku za korisnika {user.nickname}.
                    </DialogContentText>
                    <TextField
                        autoFocus
                        fullWidth
                        type="password"
                        label="Nova lozinka"
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.target.value)}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setIsPasswordDialogOpen(false)}>Odustani</Button>
                    <Button onClick={handleChangePassword} variant="contained">
                        Promeni
                    </Button>
                </DialogActions>
            </Dialog>
        </Grid>
    )
}
