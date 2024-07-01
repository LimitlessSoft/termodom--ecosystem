import { ArrowBackIosNew } from "@mui/icons-material";
import { Box, Button, Link, Typography } from "@mui/material"
import NextLink from 'next/link';


export const BackButton = (): JSX.Element => {
    return (
      <Box sx={{m: 2}}>
        <Button variant="contained" LinkComponent={NextLink} href="/porudzbine" startIcon={<ArrowBackIosNew />}>
            <Typography>Nazad</Typography>
        </Button> 
      </Box>
    )
}
