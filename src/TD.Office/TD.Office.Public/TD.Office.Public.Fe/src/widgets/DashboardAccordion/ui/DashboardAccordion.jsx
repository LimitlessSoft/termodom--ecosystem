import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Badge,
    LinearProgress,
    Typography,
} from '@mui/material'
import { ArrowDownward } from '@mui/icons-material'
import { useEffect, useState } from 'react'

export const DashboardAccordion = ({ caption, component, badgeCount }) => {
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
            expanded={expanded === true}
            onChange={() => setExpanded(!expanded)}
        >
            <AccordionSummary expandIcon={<ArrowDownward />}>
                <Badge
                    badgeContent={badgeCount === 0 ? '' : badgeCount}
                    color={badgeCount === 0 ? 'success' : 'error'}
                    sx={{
                        '& .MuiBadge-badge': {
                            top: '50%',
                            right: '-1rem',
                        },
                    }}
                >
                    <Typography marginLeft={0}>{caption}</Typography>
                </Badge>
            </AccordionSummary>
            <AccordionDetails>
                {!comp && <LinearProgress />}
                {comp}
            </AccordionDetails>
        </Accordion>
    )
}
