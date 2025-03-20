import { Grid, Paper, Button } from '@mui/material'
import { useRouter } from 'next/router'
import KeyboardBackspace from '@mui/icons-material/KeyboardBackspace'
import React from 'react'

export const HorizontalActionBar = (props) => {
    const router = useRouter()
    const childrenArray = React.Children.toArray(props.children)

    return (
        <Grid
            container
            sx={{
                position: 'sticky',
                top: 5,
                zIndex: 10000,
                px: 2,
            }}
        >
            <Paper
                elevation={4}
                sx={{
                    width: '100%',
                    p: '0.5rem',
                }}
            >
                <Grid container spacing={1}>
                    <Grid item xs={6} sm="auto">
                        <Button
                            variant="contained"
                            color="warning"
                            sx={{
                                color: 'inherit',
                                fontSize: { xs: '0.8rem', sm: '1rem' },
                                width: '100%',
                            }}
                            startIcon={<KeyboardBackspace />}
                            onClick={() => router.push(props.backButton.href)}
                        >
                            {props.backButton.title}
                        </Button>
                    </Grid>
                    {childrenArray.map((child, index) => (
                        <Grid item key={index} xs={6} sm="auto">
                            {React.cloneElement(child, {
                                sx: {
                                    ...(child.props.sx || {}),
                                },
                            })}
                        </Grid>
                    ))}
                </Grid>
            </Paper>
        </Grid>
    )
}
