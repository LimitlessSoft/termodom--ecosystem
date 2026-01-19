import { useState, useEffect, useCallback } from 'react'
import {
    Box,
    Button,
    Grid,
    IconButton,
    Paper,
    Typography,
    CircularProgress,
    useTheme,
    useMediaQuery,
} from '@mui/material'
import { ChevronLeft, ChevronRight, Add } from '@mui/icons-material'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { OdsustvoDialog } from './OdsustvoDialog'
import { KalendarAktivnostiDay } from './KalendarAktivnostiDay'

const DAYS_OF_WEEK = ['Pon', 'Uto', 'Sre', 'Čet', 'Pet', 'Sub', 'Ned']
const MONTHS = [
    'Januar', 'Februar', 'Mart', 'April', 'Maj', 'Jun',
    'Jul', 'Avgust', 'Septembar', 'Oktobar', 'Novembar', 'Decembar'
]

export const KalendarAktivnosti = () => {
    const theme = useTheme()
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'))

    const [currentDate, setCurrentDate] = useState(new Date())
    const [odsustva, setOdsustva] = useState([])
    const [tipoviOdsustva, setTipoviOdsustva] = useState([])
    const [loading, setLoading] = useState(true)
    const [dialogOpen, setDialogOpen] = useState(false)
    const [selectedOdsustvo, setSelectedOdsustvo] = useState(null)
    const [selectedDate, setSelectedDate] = useState(null)

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.KALENDAR_AKTIVNOSTI
    )

    const canWrite = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KALENDAR_AKTIVNOSTI.WRITE
    )

    const canEditAll = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KALENDAR_AKTIVNOSTI.EDIT_ALL
    )

    const canApprove = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KALENDAR_AKTIVNOSTI.APPROVE
    )

    const fetchOdsustva = useCallback(async () => {
        setLoading(true)
        try {
            const month = currentDate.getMonth() + 1
            const year = currentDate.getFullYear()
            const response = await officeApi.get(
                ENDPOINTS_CONSTANTS.ODSUSTVO.CALENDAR(month, year)
            )
            setOdsustva(response.data)
        } catch (err) {
            handleApiError(err)
        } finally {
            setLoading(false)
        }
    }, [currentDate])

    const fetchTipoviOdsustva = useCallback(async () => {
        try {
            const response = await officeApi.get(
                ENDPOINTS_CONSTANTS.TIP_ODSUSTVA.GET_MULTIPLE
            )
            setTipoviOdsustva(response.data)
        } catch (err) {
            handleApiError(err)
        }
    }, [])

    useEffect(() => {
        fetchOdsustva()
    }, [fetchOdsustva])

    useEffect(() => {
        fetchTipoviOdsustva()
    }, [fetchTipoviOdsustva])

    const handlePrevMonth = () => {
        setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 1))
    }

    const handleNextMonth = () => {
        setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 1))
    }

    const handleCellClick = (date) => {
        setSelectedDate(date)
        setSelectedOdsustvo(null)
        setDialogOpen(true)
    }

    const handleOdsustvoClick = (odsustvo) => {
        if (canEditAll || canWrite) {
            setSelectedOdsustvo(odsustvo)
            setDialogOpen(true)
        }
    }

    const handleAddNew = () => {
        setSelectedOdsustvo(null)
        setSelectedDate(new Date())
        setDialogOpen(true)
    }

    const handleDialogClose = () => {
        setDialogOpen(false)
        setSelectedOdsustvo(null)
        setSelectedDate(null)
    }

    const handleDialogSave = async () => {
        handleDialogClose()
        await fetchOdsustva()
    }

    const getDaysInMonth = () => {
        const year = currentDate.getFullYear()
        const month = currentDate.getMonth()
        const firstDay = new Date(year, month, 1)
        const lastDay = new Date(year, month + 1, 0)
        const daysInMonth = lastDay.getDate()

        let startDayOfWeek = firstDay.getDay()
        startDayOfWeek = startDayOfWeek === 0 ? 6 : startDayOfWeek - 1

        const days = []

        for (let i = 0; i < startDayOfWeek; i++) {
            days.push(null)
        }

        for (let i = 1; i <= daysInMonth; i++) {
            days.push(new Date(year, month, i))
        }

        return days
    }

    const getOdsustvaForDate = (date) => {
        if (!date) return []
        return odsustva.filter((o) => {
            const datumOd = new Date(o.datumOd)
            const datumDo = new Date(o.datumDo)
            datumOd.setHours(0, 0, 0, 0)
            datumDo.setHours(23, 59, 59, 999)
            const checkDate = new Date(date)
            checkDate.setHours(12, 0, 0, 0)
            return checkDate >= datumOd && checkDate <= datumDo
        })
    }

    const days = getDaysInMonth()

    return (
        <Box sx={{ p: isMobile ? 1 : 2 }}>
            <Paper sx={{ p: isMobile ? 1 : 2 }}>
                <Box sx={{
                    display: 'flex',
                    flexDirection: isMobile ? 'column' : 'row',
                    justifyContent: 'space-between',
                    alignItems: isMobile ? 'stretch' : 'center',
                    gap: isMobile ? 1 : 0,
                    mb: 2
                }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1 }}>
                        <IconButton onClick={handlePrevMonth} size={isMobile ? 'small' : 'medium'}>
                            <ChevronLeft />
                        </IconButton>
                        <Typography
                            variant={isMobile ? 'h6' : 'h5'}
                            sx={{ minWidth: isMobile ? 150 : 200, textAlign: 'center' }}
                        >
                            {MONTHS[currentDate.getMonth()]} {currentDate.getFullYear()}
                        </Typography>
                        <IconButton onClick={handleNextMonth} size={isMobile ? 'small' : 'medium'}>
                            <ChevronRight />
                        </IconButton>
                    </Box>
                    {(canWrite || canEditAll) && (
                        <Button
                            variant="contained"
                            startIcon={<Add />}
                            onClick={handleAddNew}
                            size={isMobile ? 'small' : 'medium'}
                            fullWidth={isMobile}
                        >
                            Novo odsustvo
                        </Button>
                    )}
                </Box>

                {loading ? (
                    <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
                        <CircularProgress />
                    </Box>
                ) : (
                    <Box sx={{
                        overflowX: isMobile ? 'auto' : 'visible',
                        WebkitOverflowScrolling: 'touch',
                    }}>
                        <Box sx={{
                            display: 'grid',
                            gridTemplateColumns: 'repeat(7, 1fr)',
                            gap: 0.5,
                            minWidth: isMobile ? 500 : 'auto',
                        }}>
                            {DAYS_OF_WEEK.map((day) => (
                                <Box
                                    key={day}
                                    sx={{
                                        p: isMobile ? 0.5 : 1,
                                        textAlign: 'center',
                                        fontWeight: 'bold',
                                        bgcolor: 'primary.main',
                                        color: 'primary.contrastText',
                                        fontSize: isMobile ? '0.75rem' : '0.875rem',
                                    }}
                                >
                                    {day}
                                </Box>
                            ))}
                            {days.map((date, index) => (
                                <KalendarAktivnostiDay
                                    key={index}
                                    date={date}
                                    odsustva={getOdsustvaForDate(date)}
                                    onCellClick={handleCellClick}
                                    onOdsustvoClick={handleOdsustvoClick}
                                    canEdit={canWrite || canEditAll}
                                    isMobile={isMobile}
                                />
                            ))}
                        </Box>
                    </Box>
                )}
            </Paper>

            <OdsustvoDialog
                open={dialogOpen}
                onClose={handleDialogClose}
                onSave={handleDialogSave}
                odsustvo={selectedOdsustvo}
                initialDate={selectedDate}
                tipoviOdsustva={tipoviOdsustva}
                canEditAll={canEditAll}
                canApprove={canApprove}
            />
        </Box>
    )
}
