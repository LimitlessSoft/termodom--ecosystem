import {
    Button,
    Divider,
    Grid,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import LogoLong from './assets/Logo_Long.png'
import Image from 'next/image'
import { mainTheme } from '@/app/theme'
import NextLink from 'next/link'
import { useEffect, useState } from 'react'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'
import { fetchMe } from '@/features/userSlice/userSlice'
import { useAppDispatch, useUser } from '@/app/hooks'
import { CustomHead } from '@/widgets/CustomHead'
import { ProfiKutakTitle } from '@/app/constants'
import { ZaboravljenaLozinkaDialog } from '@/widgets/Logovanje'
import { handleApiError, webApi } from '@/api/webApi'
import {
    ArrowRight,
    ForkRight,
    Info,
    SwipeRightAlt,
    Warning,
} from '@mui/icons-material'

const textFieldVariant = 'filled'

interface LoginRequest {
    username: string
    password: string
}

const Logovanje = (): JSX.Element => {
    const [loginRequest, setLoginRequest] = useState<LoginRequest>({
        username: '',
        password: '',
    })

    const [userToken, setUserToken] = useCookie('token', undefined)
    const user = useUser(false, true)
    const router = useRouter()
    const [isRefreshingData, setIsRefreshingData] = useState<boolean>(true)
    const dispatch = useAppDispatch()
    const [zaboravljenaLozinkaDialogOpen, setZaboravljenaLozinkaDialogOpen] =
        useState<boolean>(false)

    useEffect(() => {
        dispatch(fetchMe())
            .then((res) => {
                setIsRefreshingData(false)
            })
            .catch((err) => handleApiError(err))
    }, [dispatch])

    useEffect(() => {
        if (isRefreshingData) return

        if (user.isLogged) {
            router.push('/profi-kutak')
        }
    }, [isRefreshingData, user, router])

    return (
        <Grid
            position={`relative`}
            top={0}
            zIndex={0}
            left={0}
            px={1}
            my={6}
            container
            direction={`row`}
            justifyContent={'center'}
            alignItems={`center`}
        >
            <CustomHead title={ProfiKutakTitle} />
            <ZaboravljenaLozinkaDialog
                isOpen={zaboravljenaLozinkaDialogOpen}
                handleClose={() => {
                    setZaboravljenaLozinkaDialogOpen(false)
                }}
            />
            <Stack direction={`column`} gap={3}>
                <Stack alignItems={`center`}>
                    <Paper
                        sx={{
                            maxWidth: 300,
                            backgroundColor: mainTheme.palette.success.light,
                            p: 2,
                        }}
                    >
                        <Grid
                            container
                            alignItems={`center`}
                            gap={2}
                            justifyContent={`center`}
                        >
                            <Grid item>
                                <Info
                                    sx={{
                                        color: `white`,
                                    }}
                                />
                            </Grid>
                            <Grid item>
                                <Typography
                                    variant={`body2`}
                                    textAlign={`center`}
                                >
                                    Kupovinu{' '}
                                    <b
                                        style={{
                                            color: mainTheme.palette.success
                                                .contrastText,
                                        }}
                                    >
                                        sa popustom
                                    </b>{' '}
                                    možete izvršiti bez registracije!
                                </Typography>
                            </Grid>
                        </Grid>
                    </Paper>
                </Stack>
                <Button
                    LinkComponent={NextLink}
                    href={`/`}
                    variant={`contained`}
                    sx={{
                        p: 1,
                        my: 3,
                        backgroundColor: mainTheme.palette.info.main,
                        '&:hover': {
                            backgroundColor: mainTheme.palette.info.dark,
                        },
                        color: mainTheme.palette.info.contrastText,
                    }}
                >
                    Započni kupovinu bez registracije
                </Button>
                <Typography textAlign={`center`}>
                    ili se uloguj ispod
                </Typography>
                <Divider
                    sx={{
                        my: 2,
                    }}
                />
                <Stack alignItems={`center`}>
                    <Image
                        src={LogoLong.src}
                        width={216}
                        height={30}
                        alt={`Termodom logo`}
                    />
                    <Typography sx={{ m: 1 }} variant={`h5`} component={`h1`}>
                        PROFI KUTAK
                    </Typography>
                </Stack>
                <Stack direction={`column`}>
                    <TextField
                        required
                        sx={{ m: 1 }}
                        id="username"
                        label="Korisničko ime"
                        onChange={(e) => {
                            setLoginRequest((prev) => {
                                return { ...prev, username: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                    <TextField
                        required
                        type={`password`}
                        sx={{ m: 1 }}
                        id="password"
                        label="Lozinka"
                        onChange={(e) => {
                            setLoginRequest((prev) => {
                                return { ...prev, password: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                </Stack>
                <Button
                    variant={`contained`}
                    sx={{
                        width: 'auto',
                        color: mainTheme.palette.primary.contrastText,
                    }}
                    color={`secondary`}
                    onClick={() => {
                        webApi
                            .post('/login', loginRequest)
                            .then((response) => {
                                setUserToken(response.data)
                                router.reload()
                            })
                            .catch((err) => handleApiError(err))
                    }}
                >
                    Uloguj se
                </Button>
                <Divider />
                <Button
                    href="/registrovanje"
                    component={NextLink}
                    variant={`contained`}
                    sx={{
                        backgroundColor: '#4caf50',
                        '&:hover': { backgroundColor: '#3f9142' },
                    }}
                >
                    Postani profi kupac
                </Button>
                <Button
                    onClick={() => {
                        setZaboravljenaLozinkaDialogOpen(true)
                    }}
                    variant={`contained`}
                    sx={{
                        backgroundColor: '#6751ff',
                        '&:hover': { backgroundColor: '#9751ff' },
                    }}
                >
                    Zaboravljena lozinka
                </Button>
            </Stack>
        </Grid>
    )
}

export default Logovanje
