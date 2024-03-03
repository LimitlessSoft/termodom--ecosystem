import { MenuItem, TextField } from "@mui/material"
import { KorisnikCeneItemWrapperStyled } from "./KorisnikCeneItemWrapperStyled"

export const KorisnikCenaItem = (props: any) => {

    return (
        <KorisnikCeneItemWrapperStyled
            item
            sm={2}>
            <TextField
                variant={`filled`}
                fullWidth
                select
                defaultValue={props.userLevels.find((ul: any) => ul.groupId === props.priceGroup.id)?.level ?? 0}
                label={props.priceGroup.name}
                onChange={() => {
                    
                }}>
                    <MenuItem value={0}>Iron</MenuItem>
                    <MenuItem value={1}>Silver</MenuItem>
                    <MenuItem value={2}>Gold</MenuItem>
                    <MenuItem value={3}>Platinum</MenuItem>
            </TextField>
        </KorisnikCeneItemWrapperStyled>
    )
}