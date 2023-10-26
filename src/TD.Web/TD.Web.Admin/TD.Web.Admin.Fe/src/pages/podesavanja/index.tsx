import { mainTheme } from "@/app/theme"
import { CGP } from "@/widgets/Podesavanja/CGP"
import { PriceChange } from "@mui/icons-material"
import { Box, Button, Grid, Typography } from "@mui/material"
import { useState } from "react"

const Podesavanja = (): JSX.Element => {

    const [currentTab, setCurrentTab] = useState<string>('CGP') 

    return (
        <Box sx={{
            position: 'fixed',
            left: '0',
            zIndex: '-1',
            height: '100vh',
            widows: '100vw',
            overflow: 'hidden'
        }}>
            <Grid container sx={{ height: '100%' }}>
                <Grid sx={{
                    p: 1,
                    overflowX: 'hidden',
                    backgroundColor: mainTheme.palette.info.main
                }}>
                    <Button
                        variant='text'
                        size='large'
                        color='secondary'
                        startIcon={<PriceChange />}>CGP</Button>
                </Grid>
                <Grid sx={{ height: '100%', p: 2 }}>
                    <Box>
                        {
                            currentTab == 'CGP' ?
                                <CGP /> :
                                'ERROR'
                        }
                    </Box>
                </Grid>
            </Grid>
        </Box>
    )
}

export default Podesavanja