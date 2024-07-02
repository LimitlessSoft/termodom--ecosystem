import { ArrowBackIosNew } from "@mui/icons-material";
import { Box, Button, Link, Typography } from "@mui/material"
import NextLink from 'next/link';
import { IBackButtonProps } from "../interfaces/IBackButtonProps";


export const BackButton = (props: IBackButtonProps): JSX.Element => {
    return (
      <Box sx={{m: 2}}>
        <Button variant={`contained`} LinkComponent={NextLink} href={props.href} startIcon={<ArrowBackIosNew />}>
            <Typography>Nazad</Typography>
        </Button> 
      </Box>
    )
}
