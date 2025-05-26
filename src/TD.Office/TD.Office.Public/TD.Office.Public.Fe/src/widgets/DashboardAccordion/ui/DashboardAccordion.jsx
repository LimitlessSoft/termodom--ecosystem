import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Badge,
    Box,
    LinearProgress,
    Stack,
    Typography,
} from '@mui/material'
import { ArrowDownward } from '@mui/icons-material'
import { useEffect, useRef, useState } from 'react'
import { Zoomable } from '@/widgets'

export const DashboardAccordion = ({
    caption,
    component,
    badgeCount,
    disabled,
}) => {
    const [expanded, setExpanded] = useState(false)
    const [comp, setComp] = useState(null)

    useEffect(() => {
        if (!component) return
        if (!expanded) return
        if (comp) return
        setComp(component)
    }, [component, expanded, comp])

    return (
        <Accordion
            disabled={disabled}
            expanded={expanded === true}
            onChange={() => setExpanded(!expanded)}
        >
            <AccordionSummary
                expandIcon={
                    <Stack direction={`row`} alignItems={`center`} gap={2}>
                        <ArrowDownward />
                    </Stack>
                }
            >
                <Badge
                    badgeContent={badgeCount === 0 ? '' : badgeCount}
                    color={badgeCount === 0 ? 'success' : 'error'}
                    sx={{
                        '& .MuiBadge-badge': {
                            top: '50%',
                            right: '-1.5rem',
                        },
                    }}
                >
                    <Typography marginLeft={0}>{caption}</Typography>
                </Badge>
            </AccordionSummary>
            <AccordionDetails>
                {<Zoomable>{comp}</Zoomable> || <LinearProgress />}
            </AccordionDetails>
        </Accordion>
    )
}
