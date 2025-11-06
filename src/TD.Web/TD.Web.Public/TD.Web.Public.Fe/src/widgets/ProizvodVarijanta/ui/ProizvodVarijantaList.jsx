import { Stack } from '@mui/material'
import { ProizvodVarijanta } from '@/widgets/ProizvodVarijanta/ui/ProizvodVarijanta'
import { useRouter } from 'next/router'

export const ProizvodVarijantaList = ({ product }) => {
    const router = useRouter()
    if (!product || !product.links || product.links.length === 0) return null
    return (
        <Stack spacing={1}>
            {Object.entries(product.links).map(([key, value]) => (
                <ProizvodVarijanta
                    key={key}
                    text={value}
                    href={key}
                    current={
                        router.asPath.split('/').filter(Boolean).pop() === key
                    }
                />
            ))}
        </Stack>
    )
}
