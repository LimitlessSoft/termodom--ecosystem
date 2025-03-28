import { Button } from '@mui/material'
import React from 'react'
import NextLink from 'next/link'

const KorpaContinueShoppingButton = (props) => {
    return (
        <Button
            variant={`contained`}
            color={`warning`}
            component={NextLink}
            href={`/`}
            sx={props.sx}
        >
            Nastavi kupovinu
        </Button>
    )
}

export default KorpaContinueShoppingButton
