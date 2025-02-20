import { useZCompanyTypes } from '../../../zStore'
import { Autocomplete, TextField } from '@mui/material'

export const CompanyTypesDropdown = ({ onChange }) => {
    const companyTypes = useZCompanyTypes()
    
    if (companyTypes === undefined) return null
    
    return (
        <Autocomplete
            onChange={(event, value) => {
                onChange(value)
            }}
            getOptionLabel={(option) => `${companyTypes[option]} (${option})`}
            renderInput={(params) => {
                return <TextField {...params} label={'Tip partnera'} />
            }}
            options={Object.keys(companyTypes)} />
    )
}