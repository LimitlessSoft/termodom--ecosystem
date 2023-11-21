import { ApiBase, fetchApi } from "@/app/api"
import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { Button, Stack, TextField, Typography } from "@mui/material"
import { DatePicker } from "@mui/x-date-pickers"

const textFieldVariant = 'outlined'
const itemMaxWidth = '350px'
const itemM = 0.5

const Registrovanje = (): JSX.Element => {
    return (
        <CenteredContentWrapper>
            <Stack
                direction={`column`}
                alignItems={`center`}
                sx={{ py: 2 }}>
                    <Typography
                        variant={`h6`}>
                        Postani profi kupac - registracija
                    </Typography>
                    <TextField
                        required
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='username'
                        label='Korisničko ime'
                        onChange={(e) => {

                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        type={`password1`}
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='password1'
                        label='Lozinka'
                        onChange={(e) => {

                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        type={`password2`}
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='password2'
                        label='Ponovi lozinku'
                        onChange={(e) => {

                        }}
                        variant={textFieldVariant} />
                    <Stack sx={{ m: itemM * 2 }}>
                        <Typography>
                            Datum rođenja
                        </Typography>
                        <DatePicker sx={{ maxWidth: itemMaxWidth }} />
                    </Stack>
                    <TextField
                        required
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='mobile'
                        label='Mobilni telefon'
                        onChange={(e) => {

                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='address'
                        label='Adresa stanovanja'
                        onChange={(e) => {

                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='city'
                        label='Ovo treba dropdown gradova'
                        onChange={(e) => {

                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='favoriteStore'
                        label='Ovo treba dropdown radnji'
                        onChange={(e) => {

                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='email'
                        label='Važeća email adresa'
                        onChange={(e) => {

                        }}
                        variant={textFieldVariant} />
                    
                    <Button
                        sx={{ m: itemM, maxWidth: itemMaxWidth }}
                        variant={`contained`}
                        onClick={() => {
                            fetchApi(ApiBase.Main, `/register`, {
                                method: `PUT`,
                                body: JSON.stringify({
                                    username: 'NoviUser',
                                    password: 'password',
                                    nickname: 'Neki nickname',
                                    mobile: '0693691472',
                                    address: 'adresa neka',
                                    cityId: 1,
                                    favoriteStoreId: 12,
                                    mail: 'nekiemail@mail.com'
                                })
                            }).then((response) => {
                                console.log(response)
                            })
                        }}>
                            Podnesi zahtev za registraciju
                    </Button>
                
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Registrovanje