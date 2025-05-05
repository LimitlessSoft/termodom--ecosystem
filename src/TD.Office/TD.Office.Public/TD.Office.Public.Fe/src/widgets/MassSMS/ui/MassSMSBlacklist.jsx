import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Button,
    LinearProgress,
    Paper,
    Stack,
    TextField,
    Typography,
    useTheme,
} from '@mui/material'
import { ArrowDownward } from '@mui/icons-material'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { useState } from 'react'
import { toast } from 'react-toastify'

export const MassSMSBlacklist = () => {
    const theme = useTheme()
    const [fetching, setFetching] = useState(false)
    const [number, setNumber] = useState(``)

    const checkIfBlacklistedHandler = async () => {
        if (number === ``) {
            toast.error(`Unesite broj`)
            return
        }
        setFetching(true)
        officeApi
            .get(ENDPOINTS_CONSTANTS.MASS_SMS.IS_BLACKLISTED(number))
            .then((response) => {
                if (response.data) {
                    toast.warning(`Broj ${number} jeste na crnoj listi`)
                } else {
                    toast.success(`Broj ${number} nije na crnoj listi`)
                }
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    }

    const addToBlacklistHandler = async () => {
        if (number === ``) {
            toast.error(`Unesite broj`)
            return
        }
        setFetching(true)
        officeApi
            .post(ENDPOINTS_CONSTANTS.MASS_SMS.ADD_TO_BLACKLIST(number))
            .then(() => {
                toast.success(`Broj ${number} je dodat u crnu listu`)
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    }
    return (
        <Accordion
            sx={{
                backgroundColor: theme.palette.grey[900],
                color: theme.palette.common.white,
            }}
        >
            <AccordionSummary
                expandIcon={
                    <ArrowDownward sx={{ color: theme.palette.common.white }} />
                }
            >
                <Typography fontWeight={`bold`}>
                    Upravljaj crnom listom
                </Typography>
            </AccordionSummary>
            <AccordionDetails>
                <Paper
                    sx={{
                        p: 2,
                    }}
                >
                    <Stack gap={2}>
                        <Stack direction={`row`} spacing={2}>
                            <TextField
                                disabled={fetching}
                                value={number}
                                onChange={(e) => setNumber(e.target.value)}
                                variant={`filled`}
                                placeholder={`Broj`}
                                size={`small`}
                                sx={{
                                    minWidth: 300,
                                }}
                            />
                            <Button
                                disabled={fetching}
                                variant={`contained`}
                                color={`info`}
                                onClick={checkIfBlacklistedHandler}
                            >
                                Proveri da li je blokiran
                            </Button>
                            <Button
                                variant={`contained`}
                                color={`warning`}
                                disabled={fetching}
                                onClick={addToBlacklistHandler}
                            >
                                Dodaj u blokirane
                            </Button>
                        </Stack>
                        {fetching && <LinearProgress />}
                    </Stack>
                </Paper>
            </AccordionDetails>
        </Accordion>
    )
}
