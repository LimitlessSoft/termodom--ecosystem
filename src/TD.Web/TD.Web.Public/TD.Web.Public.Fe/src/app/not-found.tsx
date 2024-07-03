import { Grid, Typography } from '@mui/material'
import React from 'react'
import {BackButton} from "@/widgets/BackButton";

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
        <BackButton color={`error`} href={`/`} disableStartIcon text={`Početna strana`} />
      </Grid>
    </Grid>
  )
}

export default NotFound
