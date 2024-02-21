import { useUser } from "@/app/hooks"
import { Button, Card, Typography } from "@mui/material"
import NextLink from 'next/link'
import React from "react"

export const ModKupovinePoruka = (): JSX.Element => {
    
    const user = useUser()

    return (
        user == null || user.isLogged == true ?
        <React.Fragment /> :
        <Card
            variant={`outlined`}
            sx={{
                p: 2,
                m: 2
            }}>
            <Typography
                fontWeight={`bold`}
                textAlign={`center`}>
                Trenutno se nalazite u modu jednokratne kupovine.
                <Button
                    href="/logovanje"
                    component={NextLink}
                    sx={{
                        fontWeight: 'bold'
                    }}>
                    Prebaci se na profi kupovinu!
                </Button>
            </Typography>
        </Card>
    )
}