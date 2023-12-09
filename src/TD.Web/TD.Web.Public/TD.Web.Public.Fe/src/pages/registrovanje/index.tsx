import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { mainTheme } from "@/app/theme"
import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { Button, LinearProgress, MenuItem, Stack, TextField, Typography } from "@mui/material"
import { DatePicker } from "@mui/x-date-pickers"
import { useEffect, useState } from "react"
import { toast } from "react-toastify"

const textFieldVariant = 'outlined'
const itemMaxWidth = '350px'
const itemM = 0.5

interface NewUser {
    username?: string,
    password?: string,
    nickname?: string,
    address?: string,
    mobile?: string,
    dateOfBirth?: Date,
    cityId?: number,
    mail?: string
    favoriteStoreId?: number
}

const Registrovanje = (): JSX.Element => {
    
    const [cities, setCities] = useState<any | undefined>(null)
    
    const [newUser, setNewUser] = useState<NewUser>({})
    const [password1, setPassword1] = useState<string>("")
    const [password2, setPassword2] = useState<string>("")

    const [isNicknameValid, setIsNicknameValid] = useState(false)
    const [isUsernameValid, setIsUsernameValid] = useState(false)
    const [isPasswordValid, setIsPasswordValid] = useState(false)
    const [isMobileValid, setIsMobileValid] = useState(false)
    const [isAddressValid, setIsAddressValid] = useState(false)
    const [isCityIdValid, setIsCityIdValid] = useState(false)
    const [isStoreIdValid, setIsStoreIdValid] = useState(false)
    const [isMailValid, setIsMailValid] = useState(false)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/cities`, {
            method: `GET`
        }).then((res) => {
            setCities(res)
        })
    }, [])

    const isAllValid = () => {
        return (
            isNicknameValid &&
            isUsernameValid &&
            isPasswordValid &&
            isMobileValid &&
            isAddressValid &&
            isCityIdValid &&
            isStoreIdValid &&
            isMailValid)
    }

    useEffect(() => {
        setIsNicknameValid(!(newUser?.nickname == null ||
            newUser?.nickname == undefined ||
            newUser!.nickname.length < 6 ||
            newUser!.nickname.length > 32))
    }, [newUser, newUser.nickname])

    useEffect(() => {
        setIsUsernameValid(!(newUser?.username == null ||
            newUser?.username == undefined ||
            newUser!.username.length < 6 ||
            newUser!.username.length > 32))
    }, [newUser, newUser.username])

    useEffect(() => {
        let isOk = !(password1 == null ||
            password1 == undefined ||
            password1.length < 8 ||
            password2 == null ||
            password2 == undefined ||
            password2.length < 8 ||
            password1 != password2)
        setIsPasswordValid(isOk)

        if(isOk)
            setNewUser((prev) => { return { ...prev, password: password1 }})
    }, [password1, password2])

    useEffect(() => {
        setIsMobileValid(!(
            newUser?.mobile == null ||
            newUser?.mobile == undefined ||
            newUser?.mobile.length < 9 ||
            newUser?.mobile.length > 10
        ))
    }, [newUser, newUser.mobile])

    useEffect(() => {
        setIsAddressValid(!(
            newUser?.address == null ||
            newUser?.address == undefined ||
            newUser?.address.length < 5
        ))
    }, [newUser, newUser.address])

    useEffect(() => {
        setIsCityIdValid(!(
            newUser?.cityId == null ||
            newUser?.cityId == undefined
        ))
    }, [newUser, newUser.cityId])

    useEffect(() => {
        setIsStoreIdValid(!(
            newUser?.favoriteStoreId == null ||
            newUser?.favoriteStoreId == undefined
        ))
    }, [newUser, newUser.favoriteStoreId])

    useEffect(() => {
        setIsMailValid(!(
            newUser?.mail == null ||
            newUser?.mail == undefined ||
            newUser.mail.length < 5 ||
            newUser.mail.indexOf('@') < 0
        ))
    }, [newUser, newUser.mail])

    return (
        <CenteredContentWrapper>
            <Stack
                direction={`column`}
                alignItems={`center`}
                sx={{ py: 2 }}>
                    <Typography
                        sx={{ my: 2 }}
                        variant={`h6`}>
                        Postani profi kupac - registracija
                    </Typography>
                    <TextField
                        required
                        error={ !isNicknameValid }
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='nickname'
                        label='Puno ime i prezime'
                        onChange={(e) => {
                            setNewUser((prev) => { return { ...prev, nickname: e.target.value }})
                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        error={ !isUsernameValid }
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='username'
                        label='Korisničko ime'
                        onChange={(e) => {
                            setNewUser((prev) => { return { ...prev, username: e.target.value }})
                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        error={ !isPasswordValid }
                        type={`password1`}
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='password1'
                        label='Lozinka'
                        onChange={(e) => {
                            setPassword1(e.target.value)
                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        error={ !isPasswordValid }
                        type={`password2`}
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='password2'
                        label='Ponovi lozinku'
                        onChange={(e) => {
                            setPassword2(e.target.value)
                        }}
                        variant={textFieldVariant} />
                    <Stack sx={{ m: itemM * 2 }}>
                        <Typography>
                            Datum rođenja
                        </Typography>
                        <DatePicker
                            sx={{ maxWidth: itemMaxWidth }}
                            onChange={(e: any) => {
                                setNewUser((prev) => { return { ...prev, dateOfBirth: new Date(e.$d) }})
                            }}/>
                    </Stack>
                    <TextField
                        required
                        error={ !isMobileValid }
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='mobile'
                        label='Mobilni telefon'
                        onChange={(e) => {
                            setNewUser((prev) => { return { ...prev, mobile: e.target.value }})
                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        error={ !isAddressValid }
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='address'
                        label='Adresa stanovanja'
                        onChange={(e) => {
                            setNewUser((prev) => { return { ...prev, address: e.target.value }})
                        }}
                        variant={textFieldVariant} />
                        {
                            cities == null || cities.length == 0 ?
                                <LinearProgress /> :
                                <TextField
                                    id='cityId'
                                    select
                                    required
                                    label='Mesto stanovanja'
                                    sx={{ minWidth: 350 }}
                                    onChange={(e) => {
                                        setNewUser((prev) => { return { ...prev, cityId: Number.parseInt(e.target.value) }})
                                    }}
                                    helperText='Izaberite mesto stanovanja'>
                                        {
                                            cities.map((city: any) => {
                                                return (
                                                    <MenuItem key={city.id} value={city.id}>
                                                        {city.name}
                                                    </MenuItem>
                                                )
                                            })
                                        }
                                </TextField>
                        }
                    <TextField
                        required
                        error={ !isStoreIdValid }
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='favoriteStore'
                        label='Ovo treba dropdown radnji'
                        onChange={(e) => {
                            setNewUser((prev) => { return { ...prev, favoriteStoreId: Number.parseInt(e.target.value) }})
                        }}
                        variant={textFieldVariant} />
                    <TextField
                        required
                        error={ !isMailValid }
                        sx={{ m: itemM, maxWidth: itemMaxWidth, width: `100%` }}
                        id='email'
                        label='Važeća email adresa'
                        onChange={(e) => {
                            setNewUser((prev) => { return { ...prev, mail: e.target.value }})
                        }}
                        variant={textFieldVariant} />
                    
                    {
                        isAllValid() ? null :
                        <Typography
                            color={mainTheme.palette.error.light}
                            sx={{ m: 2 }}>
                            Morate ispravno popuniti sva polja!
                        </Typography>
                    }
                    <Button
                        disabled={!isAllValid()}
                        sx={{ m: itemM, maxWidth: itemMaxWidth }}
                        variant={`contained`}
                        onClick={() => {
                            fetchApi(ApiBase.Main, `/register`, {
                                method: `PUT`,
                                contentType: ContentType.ApplicationJson,
                                body: newUser
                            }).then(() => {
                                toast("Zahtev za registraciju uspešno kreiran. Bićete obavešteni o aktivaciji naloga ubrzo.", {
                                    type: "success",
                                    autoClose: false
                                })
                            })
                        }}>
                            Podnesi zahtev za registraciju
                    </Button>
                
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Registrovanje