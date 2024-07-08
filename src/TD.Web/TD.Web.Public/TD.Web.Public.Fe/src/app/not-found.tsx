import {Box, Button, Grid, Typography} from '@mui/material'
import React from 'react'
import NextLink from "next/link";
import { ArrowBackIosNew } from '@mui/icons-material';

const NotFound = (): JSX.Element => {
  return (
    <Grid container justifyContent={`center`} alignItems={`center`} direction={`column`} spacing={2} height={`100vh`}>
      <Grid item>
        <Typography variant={`h1`}>
          404
        </Typography>
      </Grid>
      <Grid item>
        <Typography component={`p`} variant={`h5`}>
          Stranica nije pronađena.
        </Typography>
      </Grid>
      <Grid item>
        <Typography>Link koji ste pratili je neispravan ili je stranica uklonjena.</Typography>
      </Grid>
        <Grid item>
            <Box sx={{m: 2}}>
                <Button
                    variant={`contained`}
                    LinkComponent={NextLink}
                    href={"/"}
                    startIcon={<ArrowBackIosNew />}
                    color={`error`}>
                    <Typography>{`Početna strana`}</Typography>
                </Button>
            </Box>
        </Grid>
    </Grid>
  )
}

export default NotFound
