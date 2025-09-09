'use client'

import { KeyboardArrowLeft } from '@mui/icons-material'
import { Button, Grid, Stack } from '@mui/material'
import { useParams } from 'next/navigation'
import { UserActiveQuizzes, UserQuizzesResults } from '@/widgets/User'
import { useRouter } from 'next/navigation'
import UserData from '@/widgets/User/ui/UserData'

export default function UserPage() {
    const router = useRouter()
    const { id } = useParams()

    return (
        <Stack
            justifyContent={`center`}
            alignItems={`center`}
            spacing={2}
            margin={`auto`}
        >
            <Grid
                container
                justifyContent={`space-between`}
                alignItems={`center`}
                gap={20}
            >
                <Button
                    startIcon={<KeyboardArrowLeft />}
                    variant={`contained`}
                    onClick={router.back}
                >
                    Nazad
                </Button>
                <UserData
                    userId={id}
                    onUserSwitch={(newUserId) =>
                        router.replace(`/admin/users/${newUserId}`)
                    }
                />
            </Grid>
            <Grid container gap={2}>
                <Grid item>
                    <UserActiveQuizzes userId={id} />
                </Grid>
                <Grid item>
                    <UserQuizzesResults userId={id} />
                </Grid>
            </Grid>
        </Stack>
    )
}
