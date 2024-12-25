import { Autocomplete, CircularProgress, TextField } from '@mui/material'
import { toast } from 'react-toastify'
import { useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'

export const PartneriDropdown = ({ disabled, onChange }) => {
    const [partneri, setPartneri] = useState([])
    const [fetching, setFetching] = useState(false)
    const [partneriSearch, setPartneriSearch] = useState('')
    const [selectedPartner, setSelectedPartner] = useState(null)

    if (partneri === undefined) return <CircularProgress />
    return (
        <Autocomplete
            sx={{
                minWidth: 300,
                maxWidth: 500,
            }}
            disabled={disabled}
            options={partneri}
            noOptionsText={`Unesi pretragu i pritisni enter...`}
            onInputChange={(event, value) => {
                setPartneriSearch(value ?? '')
            }}
            value={selectedPartner || null}
            onKeyDown={(event) => {
                if (event.key === 'Enter') {
                    if (partneriSearch.length < 5) {
                        toast.error('Pretraga mora imati bar 5 karaktera')
                        return
                    }

                    setFetching(true)
                    officeApi
                        .get(`/partners?searchKeyword=${partneriSearch}`)
                        .then((response) => {
                            setPartneri(response.data.payload)
                        })
                        .catch(handleApiError)
                        .finally(() => {
                            setFetching(false)
                        })

                    event.preventDefault()
                }
            }}
            loading={fetching}
            loadingText={`Pretraga partnera...`}
            onChange={(event, value) => {
                setSelectedPartner(value)
                onChange(value)
            }}
            getOptionLabel={(option) => {
                return `${option.naziv}`
            }}
            renderInput={(params) => (
                <TextField
                    {...params}
                    label={selectedPartner ? `Partner` : `Nema partnera`}
                />
            )}
        />
    )
}
