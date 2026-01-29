import { CircularProgress, Box, Paper, Tab, Tabs, Typography, Chip, Stack } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { KorisnikOsnovniPodaci } from '@/widgets/Korisnici/Korisnik/ui/KorisnikOsnovniPodaci'
import { KorisnikCene } from '@/widgets/Korisnici/Korisnik'
import { KorisnikAdminSettings } from '@/widgets/Korisnici/Korisnik/ui/KorisnikAdminSettings'
import { mainTheme } from '@/theme'

interface TabPanelProps {
    children?: React.ReactNode
    index: number
    value: number
}

const TabPanel = (props: TabPanelProps) => {
    const { children, value, index, ...other } = props
    return (
        <div role="tabpanel" hidden={value !== index} {...other}>
            {value === index && <Box sx={{ py: 3 }}>{children}</Box>}
        </div>
    )
}

const Korisnik = () => {
    const router = useRouter()
    const username = router.query.username

    const [loading, setLoading] = useState<boolean>(true)
    const [user, setUser] = useState<any | undefined>(undefined)
    const [tabValue, setTabValue] = useState(0)

    const reloadData = (un: string) => {
        setLoading(true)
        adminApi
            .get(`/users/${un}`)
            .then((response) => {
                setLoading(false)
                setUser(response.data)
            })
            .catch((err) => handleApiError(err))
    }

    useEffect(() => {
        if (username === undefined) return
        reloadData(username.toString())
    }, [username])

    const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
        setTabValue(newValue)
    }

    const getStatusColor = (status: string) => {
        switch (status) {
            case 'Na obradi':
                return 'info'
            case 'Aktivan':
                return 'success'
            default:
                return 'error'
        }
    }

    if (user === undefined) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="50vh">
                <CircularProgress />
            </Box>
        )
    }

    const isAdmin = user.type === 1 || user.type === 2

    return (
        <Box sx={{ p: 2 }}>
            <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
                <Stack direction="row" spacing={2} alignItems="center" flexWrap="wrap">
                    <Typography variant="h5" fontWeight="bold">
                        {user.nickname}
                    </Typography>
                    <Chip label={`@${user.username}`} variant="outlined" size="small" />
                    <Chip label={`ID: ${user.id}`} size="small" />
                    <Chip
                        label={user.status}
                        color={getStatusColor(user.status)}
                        size="small"
                    />
                    {user.referent && user.referent !== 'bez referenta' && (
                        <Chip
                            label={`Referent: ${user.referent}`}
                            variant="outlined"
                            size="small"
                        />
                    )}
                </Stack>
            </Paper>

            <Paper elevation={2}>
                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                    <Tabs value={tabValue} onChange={handleTabChange}>
                        <Tab label="Osnovni podaci" />
                        <Tab label="Cene" />
                        {isAdmin && user.isActive && <Tab label="Admin podešavanja" />}
                    </Tabs>
                </Box>

                <Box sx={{ p: 2 }}>
                    <TabPanel value={tabValue} index={0}>
                        <KorisnikOsnovniPodaci
                            user={user}
                            disabled={loading}
                            onReloadRequest={() => reloadData(user.username)}
                        />
                    </TabPanel>

                    <TabPanel value={tabValue} index={1}>
                        <KorisnikCene
                            user={user}
                            disabled={loading}
                        />
                    </TabPanel>

                    {isAdmin && user.isActive && (
                        <TabPanel value={tabValue} index={2}>
                            <KorisnikAdminSettings
                                username={username}
                                disabled={loading}
                            />
                        </TabPanel>
                    )}
                </Box>
            </Paper>
        </Box>
    )
}

export default Korisnik
