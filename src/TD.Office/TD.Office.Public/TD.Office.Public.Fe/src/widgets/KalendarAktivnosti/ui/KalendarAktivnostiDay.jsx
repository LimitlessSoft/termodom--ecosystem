import { Box, Chip, Tooltip, Typography } from '@mui/material'
import { ODSUSTVO_CONSTANTS } from '@/constants/odsustvo/odsustvoConstants'

// Status-based colors for calendar items
const STATUS_COLORS = {
    PENDING: '#ffc107',    // Yellow for pending approval
    APPROVED: '#4caf50',   // Green for approved (not realized)
    REALIZED: '#2196f3',   // Blue for realized
}

const getOdsustvoColor = (odsustvo) => {
    const isPending = odsustvo.status === ODSUSTVO_CONSTANTS.STATUS.CEKA_ODOBRENJE
    const isRealized = odsustvo.realizovanoKorisnik && odsustvo.realizovanoOdobravac

    if (isPending) {
        return STATUS_COLORS.PENDING
    }
    if (isRealized) {
        return STATUS_COLORS.REALIZED
    }
    return STATUS_COLORS.APPROVED
}

export const KalendarAktivnostiDay = ({ date, odsustva, onCellClick, onOdsustvoClick, canEdit, isMobile }) => {
    const isToday = date && new Date().toDateString() === date.toDateString()
    const isSunday = date && date.getDay() === 0
    const hasOdsustva = odsustva && odsustva.length > 0

    const handleCellClick = () => {
        if (onCellClick && date && canEdit && !isSunday) {
            onCellClick(date)
        }
    }

    const handleOdsustvoClick = (e, odsustvo) => {
        e.stopPropagation()
        if (onOdsustvoClick && !isSunday) {
            onOdsustvoClick(odsustvo)
        }
    }

    if (!date) {
        return (
            <Box
                sx={{
                    minHeight: isMobile ? 60 : 80,
                    bgcolor: 'grey.100',
                    borderRadius: 1,
                }}
            />
        )
    }

    if (isSunday) {
        return (
            <Box
                sx={{
                    minHeight: isMobile ? 60 : 80,
                    p: isMobile ? 0.25 : 0.5,
                    bgcolor: 'grey.300',
                    border: '1px solid',
                    borderColor: 'grey.400',
                    borderRadius: 1,
                    cursor: 'default',
                }}
            >
                <Typography
                    variant="body2"
                    sx={{
                        fontWeight: 'normal',
                        color: 'text.disabled',
                        mb: 0.25,
                        fontSize: isMobile ? '0.7rem' : '0.875rem',
                    }}
                >
                    {date.getDate()}
                </Typography>
            </Box>
        )
    }

    return (
        <Box
            onClick={handleCellClick}
            sx={{
                minHeight: isMobile ? 60 : 80,
                p: isMobile ? 0.25 : 0.5,
                bgcolor: isToday ? 'primary.light' : 'background.paper',
                border: '1px solid',
                borderColor: isToday ? 'primary.main' : 'grey.300',
                borderRadius: 1,
                cursor: canEdit || hasOdsustva ? 'pointer' : 'default',
                '&:hover': {
                    bgcolor: canEdit || hasOdsustva ? 'action.hover' : 'background.paper',
                },
                overflow: 'hidden',
            }}
        >
            <Typography
                variant="body2"
                sx={{
                    fontWeight: isToday ? 'bold' : 'normal',
                    color: isToday ? 'primary.contrastText' : 'text.primary',
                    mb: 0.25,
                    fontSize: isMobile ? '0.7rem' : '0.875rem',
                }}
            >
                {date.getDate()}
            </Typography>
            {odsustva.map((o) => {
                const isPending = o.status === ODSUSTVO_CONSTANTS.STATUS.CEKA_ODOBRENJE
                const isRealized = o.realizovanoKorisnik && o.realizovanoOdobravac

                return (
                    <Tooltip
                        key={o.id}
                        title={
                            <Box>
                                <Typography variant="body2">{o.userNickname}</Typography>
                                <Typography variant="caption">{o.tipOdsustvaNaziv}</Typography>
                                <Typography variant="caption" display="block" sx={{ fontWeight: 'bold' }}>
                                    Status: {ODSUSTVO_CONSTANTS.STATUS_LABELS[o.status]}
                                </Typography>
                                {o.komentar && (
                                    <Typography variant="caption" display="block">
                                        {o.komentar}
                                    </Typography>
                                )}
                            </Box>
                        }
                    >
                        <Chip
                            label={o.userNickname || o.tipOdsustvaNaziv}
                            size="small"
                            onClick={(e) => handleOdsustvoClick(e, o)}
                            sx={{
                                bgcolor: getOdsustvoColor(o),
                                color: 'white',
                                fontSize: isMobile ? '0.55rem' : '0.65rem',
                                height: isMobile ? 16 : 18,
                                mb: 0.25,
                                width: '100%',
                                cursor: 'pointer',
                                opacity: isRealized ? 0.6 : 1,
                                '& .MuiChip-label': {
                                    px: isMobile ? 0.25 : 0.5,
                                },
                            }}
                        />
                    </Tooltip>
                )
            })}
        </Box>
    )
}
