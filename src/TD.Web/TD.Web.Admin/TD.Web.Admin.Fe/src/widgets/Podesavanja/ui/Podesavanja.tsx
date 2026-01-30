import { Box, List, ListItemButton, ListItemIcon, ListItemText, Paper } from '@mui/material'
import {
    CalculateRounded,
    Category,
    Inventory,
    QrCode,
    SmartToy,
} from '@mui/icons-material'
import { CGP } from '@/widgets/Podesavanja/CGP'
import { GP } from '@/widgets/Podesavanja/GP'
import { JM } from '@/widgets/Podesavanja/JM'
import { PodesavanjaKalkulator } from '@/widgets'
import { PodesavanjaAI } from '@/widgets/Podesavanja/PodesavanjaAI'
import { useState } from 'react'

const tabs = [
    { id: 'CGP', label: 'Cenovne grupe', icon: <QrCode /> },
    { id: 'GP', label: 'Grupe proizvoda', icon: <Category /> },
    { id: 'JM', label: 'Jedinice mere', icon: <Inventory /> },
    { id: 'Kalkulator', label: 'Kalkulator', icon: <CalculateRounded /> },
    { id: 'AI', label: 'AI Podešavanja', icon: <SmartToy /> },
]

const TabContent = ({ currentTab }: { currentTab: string }): JSX.Element => {
    switch (currentTab) {
        case 'CGP':
            return <CGP />
        case 'GP':
            return <GP />
        case 'JM':
            return <JM />
        case 'Kalkulator':
            return <PodesavanjaKalkulator />
        case 'AI':
            return <PodesavanjaAI />
        default:
            return <Box>Izaberite opciju</Box>
    }
}

export const Podesavanja = () => {
    const [currentTab, setCurrentTab] = useState<string>('CGP')

    return (
        <Box sx={{ display: 'flex', height: 'calc(100vh - 64px)', overflow: 'hidden' }}>
            {/* Sidebar */}
            <Paper
                elevation={2}
                sx={{
                    width: 220,
                    minWidth: 220,
                    borderRadius: 0,
                    borderRight: '1px solid',
                    borderColor: 'divider',
                }}
            >
                <List component="nav" sx={{ py: 1 }}>
                    {tabs.map((tab) => (
                        <ListItemButton
                            key={tab.id}
                            selected={currentTab === tab.id}
                            onClick={() => setCurrentTab(tab.id)}
                            sx={{ py: 1.5 }}
                        >
                            <ListItemIcon sx={{ minWidth: 40 }}>
                                {tab.icon}
                            </ListItemIcon>
                            <ListItemText primary={tab.label} />
                        </ListItemButton>
                    ))}
                </List>
            </Paper>

            {/* Content */}
            <Box
                sx={{
                    flex: 1,
                    overflow: 'auto',
                    minWidth: 0,
                }}
            >
                <TabContent currentTab={currentTab} />
            </Box>
        </Box>
    )
}
