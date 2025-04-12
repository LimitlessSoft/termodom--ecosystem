import {
    Box,
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    LinearProgress,
    Paper,
    Tab,
    Tabs,
    TextField,
    Typography,
} from '@mui/material'
import { Help } from '@mui/icons-material'
import { useEffect, useState } from 'react'
import { mainTheme } from '@/theme'
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { ENDPOINTS_CONSTANTS } from '@/constants'
import { moduleHelpsConstants } from '@/constants/moduleHelps/moduleHelpsConstants'
import { useRouter } from 'next/router'

export const LayoutHelp = () => {
    const router = useRouter()

    const [isOpen, setIsOpen] = useState(false)

    const [activeHelpTab, setActiveHelpTab] = useState(0)
    const [data, setData] = useState(undefined)

    const [userText, setUserText] = useState('')

    const [isUpdating, setIsUpdating] = useState(false)

    const [moduleHelpMap, setModuleHelpMap] = useState(null)

    useEffect(() => {
        if (!router) return

        setModuleHelpMap(moduleHelpsConstants.getByUrl(router.pathname))
    }, [router, router.pathname])

    useEffect(() => {
        if (moduleHelpMap === null) {
            setData(null)
            return
        }

        adminApi
            .get(ENDPOINTS_CONSTANTS.MODULES_HELPS.GET(moduleHelpMap.id))
            .then((res) => {
                setData(res.data)
                setUserText(res.data.userText)
            })
            .catch(handleApiError)
    }, [moduleHelpMap])

    return (
        data && (
            <Paper
                sx={{
                    position: `absolute`,
                    top: mainTheme.spacing(10),
                    right: mainTheme.spacing(2),
                    p: mainTheme.spacing(1),
                }}
            >
                <Dialog
                    open={isOpen}
                    onClose={() => {
                        if (isUpdating) return
                        setIsOpen(false)
                    }}
                >
                    <DialogContent>
                        <Typography sx={{ my: 2 }} variant={`h6`}>
                            Pomoć modula
                        </Typography>
                        <Tabs
                            onChange={(e, v) => setActiveHelpTab(v)}
                            value={activeHelpTab}
                            sx={{
                                my: 1,
                            }}
                        >
                            <Tab label={`FAQ`} value={0} />
                            <Tab label={`Korisnik beleška`} value={1} />
                        </Tabs>
                        <Box hidden={activeHelpTab !== 0}>
                            <TextField
                                onKeyDown={(e) => {
                                    toast.info(
                                        `Ovo je zakucana beleška. Koristite KORISNIK BELEŠKA.`
                                    )
                                }}
                                readOnly
                                value={data?.systemText}
                                multiline
                                rows={6}
                                sx={{
                                    minWidth: 400,
                                }}
                            />
                        </Box>
                        <Box hidden={activeHelpTab !== 1}>
                            <TextField
                                disabled={isUpdating}
                                value={userText}
                                onChange={(e) => setUserText(e.target.value)}
                                multiline
                                rows={6}
                                sx={{
                                    minWidth: 400,
                                }}
                            />
                        </Box>
                    </DialogContent>
                    <DialogActions>
                        <Button
                            disabled={isUpdating}
                            endIcon={
                                isUpdating && <CircularProgress size={`1em`} />
                            }
                            variant={`contained`}
                            onClick={() => {
                                setIsUpdating(true)
                                adminApi
                                    .put(
                                        ENDPOINTS_CONSTANTS.MODULES_HELPS.PUT,
                                        {
                                            id: data?.userHelpId ?? null,
                                            text: userText,
                                            module: moduleHelpMap.id,
                                        }
                                    )
                                    .then(() => {
                                        toast.success('Uspešno sačuvano')
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setIsUpdating(false)
                                    })
                            }}
                        >
                            <Typography>Sačuvaj</Typography>
                        </Button>
                        <Button
                            disabled={isUpdating}
                            onClick={() => {
                                setIsOpen(false)
                            }}
                        >
                            <Typography>Zatvori</Typography>
                        </Button>
                    </DialogActions>
                </Dialog>
                <Button
                    onClick={() => {
                        setIsOpen(true)
                    }}
                >
                    <Help />
                </Button>
            </Paper>
        )
    )
}
