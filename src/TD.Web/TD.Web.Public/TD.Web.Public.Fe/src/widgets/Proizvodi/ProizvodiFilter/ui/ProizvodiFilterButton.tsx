import { useRouter } from 'next/router'
import { IProizvodiFilterButtonProps } from '../models/IProizvodiFilterButtonProps'
import { Button } from '@mui/material'
import { ProizvodiFilterButtonStyled } from './ProizvodiFilterButtonStyled'

export const ProizvodiFilterButton = (
    props: IProizvodiFilterButtonProps
): JSX.Element => {
    const router = useRouter()

    return (
        <ProizvodiFilterButtonStyled item>
            <Button
                variant={'contained'}
                onClick={() => {
                    router.push({
                        pathname: router.pathname,
                        query: {
                            ...router.query,
                            grupa: props.group.name,
                            pretraga: null,
                        },
                    })
                }}
            >
                {props.group.name}
            </Button>
        </ProizvodiFilterButtonStyled>
    )
}
