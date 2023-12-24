import { Grid, Typography, styled } from '@mui/material';
import React from 'react';
import { KolicinaInputTitle } from './KolicinaInputTitle';
import { KolicinaInputFieldWrapper } from './KolicinaInputFieldWrapper';

export const KolicinaInput = (props: any): JSX.Element => {
  return (
    <Grid>
      <KolicinaInputTitle>
        {props.unit ?? 'unknown'}
      </KolicinaInputTitle>
      <KolicinaInputFieldWrapper value={props.value} onValueChange={props.onValueChange} />
    </Grid>
  )
}