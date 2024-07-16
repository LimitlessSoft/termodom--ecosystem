import {PodesavanjaStyled} from "@/widgets/Podesavanja/styled/PodesavanjaStyled";
import {Box, Button, Grid, Stack, Typography} from "@mui/material"
import {Category, Inventory, QrCode} from "@mui/icons-material"
import {CGP} from "@/widgets/Podesavanja/CGP"
import {GP} from "@/widgets/Podesavanja/GP"
import {JM} from "@/widgets/Podesavanja/JM"
import {mainTheme} from "@/app/theme"
import {useState} from "react"

const InnerBox = (props: any): JSX.Element => {
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

export const Podesavanja = () => {

    const [currentTab, setCurrentTab] = useState<string>('CGP')

    const handleSetCurrentTab = (tabName: string) => {
        setCurrentTab(tabName)
    }
    
    return (
        <PodesavanjaStyled>
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
        </PodesavanjaStyled>
    )
}