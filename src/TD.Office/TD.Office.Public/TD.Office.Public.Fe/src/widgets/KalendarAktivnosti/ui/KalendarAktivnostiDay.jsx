import { Box, Chip, Tooltip, Typography } from '@mui/material'
import { ODSUSTVO_CONSTANTS } from '@/constants/odsustvo/odsustvoConstants'

const TYPE_COLORS = {
    'Slava': '#ff9800',
    'Odmor': '#4caf50',
    'Sahrana': '#9e9e9e',
    'Ostalo': '#2196f3',
}

const getTypeColor = (tipNaziv) => {
    return TYPE_COLORS[tipNaziv] || '#2196f3'
}

export const KalendarAktivnostiDay = ({ date, odsustva, onCellClick, onOdsustvoClick, canEdit, isMobile }) => {
    const isToday = date && new Date().toDateString() === date.toDateString()
    const hasOdsustva = odsustva && odsustva.length > 0

    const handleCellClick = () => {
        if (onCellClick && date && canEdit) {
            onCellClick(date)
        }
    }

    const handleOdsustvoClick = (e, odsustvo) => {
        e.stopPropagation()
        if (onOdsustvoClick) {
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
                const isFullyRealized = o.realizovanoKorisnik && o.realizovanoOdobravac

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
                                bgcolor: getTypeColor(o.tipOdsustvaNaziv),
                                color: 'white',
                                fontSize: isMobile ? '0.55rem' : '0.65rem',
                                height: isMobile ? 16 : 18,
                                mb: 0.25,
                                width: '100%',
                                cursor: 'pointer',
                                border: isPending ? '2px dashed rgba(255,255,255,0.5)' : 'none',
                                opacity: isFullyRealized ? 0.6 : 1,
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
