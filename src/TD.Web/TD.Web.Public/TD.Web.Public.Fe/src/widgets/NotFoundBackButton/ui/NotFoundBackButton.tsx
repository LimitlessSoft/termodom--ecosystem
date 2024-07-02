import { Box, Button, Typography } from "@mui/material"
import NextLink from 'next/link';


export const NotFoundBackButton = (): JSX.Element => {
    return (
      <Box sx={{m: 2}}>
        <Button variant={`contained`} LinkComponent={NextLink} href={`/`} color={`error`}>
            <Typography>Početna strana</Typography>
        </Button> 
      </Box>
    )
}