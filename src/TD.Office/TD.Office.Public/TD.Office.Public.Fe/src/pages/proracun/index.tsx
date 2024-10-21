import { Box, IconButton } from '@mui/material'
import {
    HorizontalActionBar,
    HorizontalActionBarButton,
    ProracunNoviDialog,
    ProracunTable,
} from '@/widgets'
import { useRouter } from 'next/router'
import { AddCircle } from '@mui/icons-material'
import { useState } from 'react'
import { toast } from 'react-toastify'
import { ProracunFilters } from '@/widgets/Proracun/ProracunFilters/ui/ProracunFilters'

const ProracunPage = () => {
    const router = useRouter()

    const [noviProracunDialogOpen, setNoviProracunDialogOpen] = useState(false)

    return (
        <Box>
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text="Nazad"
                    onClick={() => router.push(`/korisnici`)}
                />
            </HorizontalActionBar>
            <HorizontalActionBar>
                <ProracunNoviDialog
                    open={noviProracunDialogOpen}
                    onClose={() => {
                        setNoviProracunDialogOpen(false)
                    }}
                    onCancel={() => {
                        setNoviProracunDialogOpen(false)
                    }}
                    onSuccess={() => {
                        toast.warning('Implement reload here')
                    }}
                />
                <IconButton
                    onClick={() => {
                        setNoviProracunDialogOpen(true)
                    }}
                >
                    <AddCircle color={`primary`} fontSize={`large`} />
                </IconButton>
            </HorizontalActionBar>
            <ProracunFilters />
            <ProracunTable />
        </Box>
    )
}

export default ProracunPage
