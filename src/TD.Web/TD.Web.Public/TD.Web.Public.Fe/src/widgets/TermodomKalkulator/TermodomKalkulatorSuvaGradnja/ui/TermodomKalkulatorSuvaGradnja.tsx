import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    CircularProgress,
    Dialog,
    DialogContent,
    DialogTitle,
    Divider,
    Grid,
    LinearProgress,
    List,
    ListItem,
    MenuItem,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { formatNumber } from '@/app/helpers/numberHelpers'
import StandardSvg from '@/assets/Standard.svg'
import HobiSvg from '@/assets/Hobi.svg'
import ProfiSvg from '@/assets/Profi.svg'
import Image from 'next/image'
import { ArrowDownward } from '@mui/icons-material'
import { handleApiError, webApi } from '@/api/webApi'
import { CustomHead } from '@/widgets/CustomHead'

export const TermodomKalkulatorSuvaGradnja = () => {
    const [kvadratura, setKvadratura] = useState<number>(1)
    const [objasnjenjeKlasaShown, setObjasnjenjeKlasaShown] =
        useState<boolean>(false)

    const potrosnjaCellWidth = 100

    const [items, setItems] = useState<any>(undefined)
    const [selectedType, setSelectedType] = useState<any>(
        'SuvaGradnja_Oblaganje'
    )

    useEffect(() => {
        if (!selectedType) return

        webApi
            .get(`/calculator-items?Type=${selectedType}`)
            .then((resposne: any) => {
                setItems(resposne.data)
            })
            .catch(handleApiError)
    }, [selectedType])

    const [summary, setSummary] = useState<any>(undefined)

    useEffect(() => {
        if (kvadratura === 0 || !selectedType) return
        webApi
            .get(`/calculator?Type=${selectedType}&Quantity=${kvadratura}`)
            .then((response) => {
                setSummary(response.data)
            })
            .catch(handleApiError)
    }, [selectedType, kvadratura])

    return (
        <Grid container gap={2}>
            <CustomHead
                title={
                    'Kalkulacija potrošnje materijala za suvu gradnju - Termodom Kalkulator'
                }
                description={'Kalkulacija potrošnje materijala za suvu gradnju'}
            />
            <Grid item sm={12}>
                <Typography>
                    Kalkulacija potrošnje materijala za suvu gradnju
                </Typography>
            </Grid>
            <Grid item sm={12}>
                <Stack gap={2}>
                    <TextField
                        select
                        defaultValue={'SuvaGradnja_Oblaganje'}
                        onChange={(e) => {
                            setSelectedType(e.target.value)
                        }}
                    >
                        <MenuItem value={'SuvaGradnja_Pregradjivanje'}>
                            Pregrađivanje (ploča sa 2 strane profila)
                        </MenuItem>
                        <MenuItem value={'SuvaGradnja_Oblaganje'}>
                            Oblaganje (ploča sa 1 strane profila)
                        </MenuItem>
                        <MenuItem value={'SuvaGradnja_SpusteniPlafon'}>
                            Spušteni plafon
                        </MenuItem>
                    </TextField>
                    {items && !items.find((x: any) => x.isPrimary === true) && (
                        <TextField
                            sx={{
                                my: 3,
                                maxWidth: 200,
                                '& input': {
                                    backgroundColor: `white`,
                                    textAlign: `center`,
                                },
                            }}
                            variant={`outlined`}
                            label={`Preračunaj za kvadraturu:`}
                            value={kvadratura}
                            onChange={(e) => {
                                if (Number.isNaN(Number(e.target.value)))
                                    setKvadratura(0)
                                else setKvadratura(Number(e.target.value))
                            }}
                        />
                    )}
                    {items === undefined ? (
                        <LinearProgress />
                    ) : (
                        <TableContainer component={Paper}>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Proizvod</TableCell>
                                        <TableCell width={potrosnjaCellWidth}>
                                            Potrošnja
                                        </TableCell>
                                        <TableCell>JM</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {items.map((item: any) => (
                                        <TableRow key={item.id}>
                                            <TableCell>
                                                {item.productName}
                                            </TableCell>
                                            <TableCell
                                                sx={{
                                                    textAlign: `right`,
                                                }}
                                            >
                                                {item.isPrimary && (
                                                    <TextField
                                                        value={kvadratura}
                                                        onChange={(e) => {
                                                            if (
                                                                Number.isNaN(
                                                                    Number(
                                                                        e.target
                                                                            .value
                                                                    )
                                                                )
                                                            )
                                                                setKvadratura(0)
                                                            else
                                                                setKvadratura(
                                                                    Number(
                                                                        e.target
                                                                            .value
                                                                    )
                                                                )
                                                        }}
                                                    />
                                                )}
                                                {!item.isPrimary &&
                                                    formatNumber(
                                                        item.quantity *
                                                            kvadratura
                                                    )}
                                            </TableCell>
                                            <TableCell>{item.unit}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    )}
                    <Paper
                        sx={{
                            p: 2,
                            textAlign: `right`,
                        }}
                    >
                        <Stack gap={1}>
                            <Typography>
                                Ukupna vrednost materijala (sa PDV-om):
                            </Typography>
                            <Typography variant={`caption`}>
                                Cene se obračunavaju sa popustom jednokratnog
                                kupca (veća količina = veći popust)
                            </Typography>
                            {!summary ||
                                (summary.hobiValueWithVAT === undefined && (
                                    <LinearProgress />
                                ))}
                            {summary && summary.hobiValueWithVAT !== 0 && (
                                <Typography
                                    sx={{
                                        color: `gray`,
                                    }}
                                >
                                    HOBI:{' '}
                                    {formatNumber(summary.hobiValueWithVAT)} RSD
                                </Typography>
                            )}
                            {!summary ||
                                (summary.standardValueWithVAT === undefined && (
                                    <LinearProgress />
                                ))}
                            {summary && summary.standardValueWithVAT !== 0 && (
                                <Typography
                                    sx={{
                                        color: `green`,
                                    }}
                                >
                                    STANDARD:{' '}
                                    {formatNumber(summary.standardValueWithVAT)}{' '}
                                    RSD
                                </Typography>
                            )}
                            {!summary ||
                                (summary.profiValueWithVAT === undefined && (
                                    <LinearProgress />
                                ))}
                            {summary && summary.profiValueWithVAT !== 0 && (
                                <Typography
                                    sx={{
                                        color: `orange`,
                                    }}
                                >
                                    PROFI:{' '}
                                    {formatNumber(summary.profiValueWithVAT)}{' '}
                                    RSD
                                </Typography>
                            )}
                        </Stack>
                    </Paper>
                    {/*<Divider*/}
                    {/*    sx={{*/}
                    {/*        my: 2,*/}
                    {/*    }}*/}
                    {/*/>*/}
                    {/*<Accordion>*/}
                    {/*    <AccordionSummary expandIcon={<ArrowDownward />}>*/}
                    {/*        <Typography variant={`subtitle1`}>*/}
                    {/*            Dodaj u korpu*/}
                    {/*        </Typography>*/}
                    {/*    </AccordionSummary>*/}
                    {/*    <AccordionDetails>*/}
                    {/*        <Grid container textAlign={`center`} spacing={2}>*/}
                    {/*            <Grid item sm={12}>*/}
                    {/*                <Typography variant={`subtitle1`} my={2}>*/}
                    {/*                    Izaberi klasu fasadnog materijala da bi*/}
                    {/*                    dodao u korpu*/}
                    {/*                </Typography>*/}
                    {/*                <Typography variant={`caption`} my={2}>*/}
                    {/*                    (klik na ikonicu)*/}
                    {/*                </Typography>*/}
                    {/*            </Grid>*/}
                    {/*            <Grid item sm={4}>*/}
                    {/*                <Button*/}
                    {/*                    color={`secondary`}*/}
                    {/*                    sx={{*/}
                    {/*                        filter: classFilter,*/}
                    {/*                    }}*/}
                    {/*                    onClick={() => {}}*/}
                    {/*                >*/}
                    {/*                    <Image*/}
                    {/*                        src={HobiSvg.src}*/}
                    {/*                        alt={'td-hobi'}*/}
                    {/*                        width={classWidth}*/}
                    {/*                        height={classHeight}*/}
                    {/*                    />*/}
                    {/*                </Button>*/}
                    {/*            </Grid>*/}
                    {/*            <Grid item sm={4}>*/}
                    {/*                <Button*/}
                    {/*                    color={`secondary`}*/}
                    {/*                    sx={{*/}
                    {/*                        filter: classFilter,*/}
                    {/*                    }}*/}
                    {/*                >*/}
                    {/*                    <Image*/}
                    {/*                        src={StandardSvg.src}*/}
                    {/*                        alt={'td-standard'}*/}
                    {/*                        width={classWidth}*/}
                    {/*                        height={classHeight}*/}
                    {/*                    />*/}
                    {/*                </Button>*/}
                    {/*            </Grid>*/}
                    {/*            <Grid item sm={4}>*/}
                    {/*                <Button*/}
                    {/*                    color={`secondary`}*/}
                    {/*                    sx={{*/}
                    {/*                        filter: classFilter,*/}
                    {/*                    }}*/}
                    {/*                >*/}
                    {/*                    <Image*/}
                    {/*                        src={ProfiSvg.src}*/}
                    {/*                        alt={'td-profi'}*/}
                    {/*                        width={classWidth}*/}
                    {/*                        height={classHeight}*/}
                    {/*                    />*/}
                    {/*                </Button>*/}
                    {/*            </Grid>*/}
                    {/*            <Grid item sm={12}>*/}
                    {/*                <Dialog*/}
                    {/*                    open={objasnjenjeKlasaShown}*/}
                    {/*                    onClose={() => {*/}
                    {/*                        setObjasnjenjeKlasaShown(false)*/}
                    {/*                    }}*/}
                    {/*                >*/}
                    {/*                    <DialogTitle>*/}
                    {/*                        Objašnjenje Termodom Klasa*/}
                    {/*                    </DialogTitle>*/}
                    {/*                    <DialogContent>*/}
                    {/*                        <Stack gap={2}>*/}
                    {/*                            <Accordion>*/}
                    {/*                                <AccordionSummary*/}
                    {/*                                    sx={{*/}
                    {/*                                        backgroundColor: `#aaa`,*/}
                    {/*                                    }}*/}
                    {/*                                    expandIcon={*/}
                    {/*                                        <ArrowDownward />*/}
                    {/*                                    }*/}
                    {/*                                >*/}
                    {/*                                    <Typography*/}
                    {/*                                        variant={`subtitle1`}*/}
                    {/*                                    >*/}
                    {/*                                        Hobi*/}
                    {/*                                    </Typography>*/}
                    {/*                                </AccordionSummary>*/}
                    {/*                                <AccordionDetails>*/}
                    {/*                                    <List>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Fasadni stiropor*/}
                    {/*                                            13-15kg /m3*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Staklena mrežica*/}
                    {/*                                            125g /m2*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Lepak za*/}
                    {/*                                            stiropor*/}
                    {/*                                            građevinski*/}
                    {/*                                            (grub, slabo*/}
                    {/*                                            lepi, teško se*/}
                    {/*                                            nanosi mrežica)*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Tiplovi*/}
                    {/*                                            plastični*/}
                    {/*                                        </ListItem>*/}
                    {/*                                    </List>*/}
                    {/*                                </AccordionDetails>*/}
                    {/*                            </Accordion>*/}
                    {/*                            <Accordion>*/}
                    {/*                                <AccordionSummary*/}
                    {/*                                    sx={{*/}
                    {/*                                        backgroundColor: `green`,*/}
                    {/*                                    }}*/}
                    {/*                                    expandIcon={*/}
                    {/*                                        <ArrowDownward />*/}
                    {/*                                    }*/}
                    {/*                                >*/}
                    {/*                                    <Typography*/}
                    {/*                                        variant={`subtitle1`}*/}
                    {/*                                    >*/}
                    {/*                                        Standard*/}
                    {/*                                    </Typography>*/}
                    {/*                                </AccordionSummary>*/}
                    {/*                                <AccordionDetails>*/}
                    {/*                                    <List>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Fasadni stiropor*/}
                    {/*                                            17kg /m3*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Staklena mrežica*/}
                    {/*                                            145g-150g /m2*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Lepak za*/}
                    {/*                                            stiropor*/}
                    {/*                                            građevinski*/}
                    {/*                                            Termodom FIX*/}
                    {/*                                            (sitne*/}
                    {/*                                            granulacije,*/}
                    {/*                                            lako se nanosi*/}
                    {/*                                            mrežica)*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Tiplovi*/}
                    {/*                                            plastični*/}
                    {/*                                        </ListItem>*/}
                    {/*                                    </List>*/}
                    {/*                                </AccordionDetails>*/}
                    {/*                            </Accordion>*/}
                    {/*                            <Accordion>*/}
                    {/*                                <AccordionSummary*/}
                    {/*                                    sx={{*/}
                    {/*                                        backgroundColor: `orange`,*/}
                    {/*                                    }}*/}
                    {/*                                    expandIcon={*/}
                    {/*                                        <ArrowDownward />*/}
                    {/*                                    }*/}
                    {/*                                >*/}
                    {/*                                    <Typography*/}
                    {/*                                        variant={`subtitle1`}*/}
                    {/*                                    >*/}
                    {/*                                        Profi*/}
                    {/*                                    </Typography>*/}
                    {/*                                </AccordionSummary>*/}
                    {/*                                <AccordionDetails>*/}
                    {/*                                    <List>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Fasadni stiropor*/}
                    {/*                                            20kg /m3*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Staklena mrežica*/}
                    {/*                                            170g /m2*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Lepak za*/}
                    {/*                                            stiropor i vunu*/}
                    {/*                                            građevinski*/}
                    {/*                                            (sitne*/}
                    {/*                                            granulacije,*/}
                    {/*                                            lako se nanosi*/}
                    {/*                                            mrežica)*/}
                    {/*                                        </ListItem>*/}
                    {/*                                        <ListItem>*/}
                    {/*                                            Tiplovi sa*/}
                    {/*                                            metalnim jezgrom*/}
                    {/*                                        </ListItem>*/}
                    {/*                                    </List>*/}
                    {/*                                </AccordionDetails>*/}
                    {/*                            </Accordion>*/}
                    {/*                        </Stack>*/}
                    {/*                    </DialogContent>*/}
                    {/*                </Dialog>*/}
                    {/*                <Button*/}
                    {/*                    variant={`outlined`}*/}
                    {/*                    sx={{*/}
                    {/*                        my: 2,*/}
                    {/*                    }}*/}
                    {/*                    onClick={() => {*/}
                    {/*                        setObjasnjenjeKlasaShown(true)*/}
                    {/*                    }}*/}
                    {/*                >*/}
                    {/*                    Koji materijal pripada kojoj grupi?*/}
                    {/*                </Button>*/}
                    {/*            </Grid>*/}
                    {/*        </Grid>*/}
                    {/*    </AccordionDetails>*/}
                    {/*</Accordion>*/}
                </Stack>
            </Grid>
        </Grid>
    )
}
