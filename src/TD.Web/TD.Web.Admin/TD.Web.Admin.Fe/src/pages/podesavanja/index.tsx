import { mainTheme } from "@/app/theme"
import { CGP } from "@/widgets/Podesavanja/CGP"
import { GP } from "@/widgets/Podesavanja/GP"
import { JM } from "@/widgets/Podesavanja/JM"
import { Category, Inventory, PriceChange, QrCode, Workspaces } from "@mui/icons-material"
import { Box, Button, Grid, Stack, Typography } from "@mui/material"
import { useState } from "react"

const Podesavanja = (): JSX.Element => {

    const [currentTab, setCurrentTab] = useState<string>('CGP') 

    const handleSetCurrentTab = (tabName: string) => {
        setCurrentTab(tabName)
    }
    return (
        <Box sx={{
            position: 'fixed',
            left: '0',
            zIndex: '-1',
            height: '100vh',
            width: '100%',
            overflow: 'hidden'
        }}>
            <Stack direction={'row'} sx={{ height: '100%' }}>
                <Grid sx={{
                    minHeight: '100%',
                    p: 1,
                    overflowX: 'hidden',
                    backgroundColor: mainTheme.palette.info.main
                }}>
                    <Stack sx={{ height: '100%' }}>
                        <Button
                            variant='text'
                            size='large'
                            color='secondary'
                            startIcon={<QrCode />}
                            onClick={() => handleSetCurrentTab('CGP')}>CGP</Button>
                        <Button
                            variant='text'
                            size='large'
                            color='secondary'
                            startIcon={<Category />}
                            onClick={() => handleSetCurrentTab('GP')}>GP</Button>
                        <Button
                            variant='text'
                            size='large'
                            color='secondary'
                            startIcon={<Inventory />}
                            onClick={() => handleSetCurrentTab('JM')}>JM</Button>
                    </Stack>
                </Grid>
                <Grid sx={{ height: '100%', flex: 1, overflowY: 'auto' }}>
                    <Box>
                        <InnerBox currentTab={currentTab} />
                    </Box>
                </Grid>
            </Stack>
        </Box>
    )
}

const InnerBox = (props: any): JSX.Element => {
    console.log(props)

    switch(props.currentTab) {
        case 'CGP':
            return <CGP />
        case 'GP':
            return <GP />
        case 'JM':
            return <JM />
        default:
            return <Typography>ERROR</Typography>
    }
}

export default Podesavanja