import { useRouter } from 'next/router'
import { IProizvodiFilterButtonProps } from '../models/IProizvodiFilterButtonProps'
import { Button } from '@mui/material'
import { ProizvodiFilterButtonStyled } from './ProizvodiFilterButtonStyled'
import NextLink from 'next/link'

export const ProizvodiFilterButton = (props: IProizvodiFilterButtonProps) => {
    const router = useRouter()

    return (
        <ProizvodiFilterButtonStyled item>
            <Button
                variant={'contained'}
                LinkComponent={NextLink}
                onClick={() => {
                    let route =
                        router.asPath === '/'
                            ? `/${props.group.name.toLowerCase()}`
                            : `${router.asPath.split('?')[0]}/${props.group.name.toLowerCase()}`

                    router.push(route)
                }}
            >
                {props.group.name}
            </Button>
        </ProizvodiFilterButtonStyled>
    )
}
