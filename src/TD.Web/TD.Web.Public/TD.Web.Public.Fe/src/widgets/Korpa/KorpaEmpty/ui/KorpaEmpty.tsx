import { KorpaTitle } from "@/app/constants"
import { CustomHead } from "@/widgets/CustomHead"
import { Box, Button, Grid, Typography } from "@mui/material"
import NextLink from 'next/link'

export const KorpaEmpty = (): JSX.Element => {
    return (
        <Grid
            container
            py={`30vh`}
            justifyContent={`center`}>
            <CustomHead title={KorpaTitle} />
            <Grid
                item>
                <Grid>
                    <Box
                        my={3}>
                        <Typography
                            variant={`h4`}>
                            Vaša korpa je prazna
                        </Typography>
                    </Box>
                    <Box
                        my={3}
                        textAlign={`center`}>
                        <Button
                            href={`/`}
                            component={NextLink}
                            variant={`contained`}>
                            Idi u prodavnicu
                        </Button>
                    </Box>
                </Grid>
            </Grid>
        </Grid>
    )
}