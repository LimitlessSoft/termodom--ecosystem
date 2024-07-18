import { MenuItem, TextField } from '@mui/material'
import { KorisnikCeneItemWrapperStyled } from './KorisnikCeneItemWrapperStyled'
import { ApiBase, ContentType, fetchApi } from '@/api'
import { toast } from 'react-toastify'
import { useState } from 'react'

export const KorisnikCenaItem = (props: any) => {
    const [isUpdating, setIsUpdating] = useState<boolean>(false)

    return (
        <KorisnikCeneItemWrapperStyled item sm={2}>
            <TextField
                variant={`filled`}
                fullWidth
                select
                disabled={isUpdating}
                defaultValue={
                    props.userLevels.find(
                        (ul: any) => ul.groupId === props.priceGroup.id
                    )?.level ?? 0
                }
                label={props.priceGroup.name}
                onChange={(e) => {
                    fetchApi(ApiBase.Main, `/users-product-price-levels`, {
                        method: 'PUT',
                        body: {
                            userId: props.userId,
                            productPriceGroupId: props.priceGroup.id,
                            level: e.target.value,
                        },
                        contentType: ContentType.ApplicationJson,
                    })
                        .then(() => {
                            toast.success(
                                `Uspešno promenjen nivo ${props.priceGroup.name}`
                            )
                        })
                        .finally(() => {
                            setIsUpdating(false)
                        })
                }}
            >
                <MenuItem value={0}>Iron</MenuItem>
                <MenuItem value={1}>Silver</MenuItem>
                <MenuItem value={2}>Gold</MenuItem>
                <MenuItem value={3}>Platinum</MenuItem>
            </TextField>
        </KorisnikCeneItemWrapperStyled>
    )
}
