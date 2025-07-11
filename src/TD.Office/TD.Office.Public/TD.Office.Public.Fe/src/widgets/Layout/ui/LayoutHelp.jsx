import { useEffect, useState } from 'react'
import {
    Box,
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    Paper,
    Tab,
    Tabs,
    TextField,
    Typography,
} from '@mui/material'
import { useRouter } from 'next/router'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { MODULE_HELP_CONSTANTS, ENDPOINTS_CONSTANTS } from '@/constants'
import { Help } from '@mui/icons-material'
import { toast } from 'react-toastify'

const LayoutHelp = () => {
    const router = useRouter()

    const [data, setData] = useState()
    const [isOpen, setIsOpen] = useState(false)
    const [isUpdating, setIsUpdating] = useState(false)
    const [activeTab, setActiveTab] = useState(0)
    const [tabText, setTabText] = useState('')
    const [moduleHelpMap, setModuleHelpMap] = useState(null)

    useEffect(() => {
        if (!router) return

        setModuleHelpMap(MODULE_HELP_CONSTANTS.getByUrl(router.pathname))
    }, [router, router.pathname])

    useEffect(() => {
        if (moduleHelpMap === null) {
            setData(null)
            return
        }

        officeApi
            .get(ENDPOINTS_CONSTANTS.MODULES_HELPS.GET(moduleHelpMap.id))
            .then((res) => {
                setData(res.data)
                setTabText(res.data.userText)
            })
            .catch(handleApiError)
    }, [moduleHelpMap])

    const handleCloseDialog = () => {
        if (isUpdating) return
        setIsOpen(false)
    }

    const handleSaveTabValue = () => {
        setIsUpdating(true)

        officeApi
            .put(ENDPOINTS_CONSTANTS.MODULES_HELPS.PUT, {
                id: data?.userHelpId ?? null,
                text: tabText,
                module: moduleHelpMap.id,
            })
            .then(() => {
                toast.success('Uspešno sačuvano')
            })
            .catch(handleApiError)
            .finally(() => {
                setIsUpdating(false)
            })
    }

    return (
        data && (
            <Paper
                sx={(theme) => ({
                    position: `absolute`,
                    top: theme.spacing(8),
                    right: theme.spacing(2),
                    p: theme.spacing(1),
                    zIndex: 9999,
                })}
            >
                <Dialog open={isOpen} onClose={handleCloseDialog}>
                    <DialogContent>
                        <Typography>Pomoć modula</Typography>
                        <Tabs
                            onChange={(e, v) => setActiveTab(v)}
                            value={activeTab}
                            sx={{
                                my: 1,
                            }}
                        >
                            <Tab label={`FAQ`} value={0} />
                            <Tab label={`Korisnik beleška`} value={1} />
                        </Tabs>
                        <Box hidden={activeTab !== 0}>
                            <TextField
                                onKeyDown={() => {
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
                        <Box hidden={activeTab !== 1}>
                            <TextField
                                disabled={isUpdating}
                                value={tabText}
                                onChange={(e) => setTabText(e.target.value)}
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
                            onClick={handleSaveTabValue}
                        >
                            <Typography>Sačuvaj</Typography>
                        </Button>
                        <Button
                            disabled={isUpdating}
                            onClick={handleCloseDialog}
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

export default LayoutHelp
