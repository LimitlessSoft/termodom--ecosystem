import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    Dialog,
    DialogContent,
    DialogTitle,
    Divider,
    Grid,
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
import { useState } from 'react'
import { formatNumber } from '@/app/helpers/numberHelpers'
import StandardSvg from '@/assets/Standard.svg'
import HobiSvg from '@/assets/Hobi.svg'
import ProfiSvg from '@/assets/Profi.svg'
import Image from 'next/image'
import { ArrowDownward } from '@mui/icons-material'

export const TermodomKalkulatorSuvaGradnja = () => {
    const [kvadratura, setKvadratura] = useState<number>(1)
    const [objasnjenjeKlasaShown, setObjasnjenjeKlasaShown] =
        useState<boolean>(false)

    const potrosnjaCellWidth = 100
    const classWidth = 100
    const classHeight = 200
    const classFilter = `drop-shadow( 3px 3px 2px rgba(0, 0, 0, .6))`

    return (
        <Grid container gap={2}>
            <Grid item sm={12}>
                <Typography>
                    Kalkulacija potrošnje materijala za suvu gradnju
                </Typography>
            </Grid>
            <Grid item sm={12}>
                <Stack gap={2}>
                    <TextField select defaultValue={1}>
                        <MenuItem value={0}>
                            Pregrađivanje (ploča sa 2 strane profila)
                        </MenuItem>
                        <MenuItem value={1}>
                            Oblaganje (ploča sa 1 strane profila)
                        </MenuItem>
                        <MenuItem value={2}>Spušteni plafon</MenuItem>
                    </TextField>
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
                                <TableRow>
                                    <TableCell>Gips karton ploča</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        <TextField
                                            value={kvadratura}
                                            onChange={(e) => {
                                                if (
                                                    Number.isNaN(
                                                        Number(e.target.value)
                                                    )
                                                )
                                                    setKvadratura(0)
                                                else
                                                    setKvadratura(
                                                        Number(e.target.value)
                                                    )
                                            }}
                                        />
                                    </TableCell>
                                    <TableCell>m2</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>UD Profil</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        {formatNumber(0.75 * kvadratura)}
                                    </TableCell>
                                    <TableCell>m</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>CD Profil</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        {formatNumber(2 * kvadratura)}
                                    </TableCell>
                                    <TableCell>m</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Ispuna</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        {formatNumber(0.25 * kvadratura)}
                                    </TableCell>
                                    <TableCell>kg</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Bandaž traka</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        {formatNumber(0.017 * kvadratura)}
                                    </TableCell>
                                    <TableCell>kom (45m)</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Distancer</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        {formatNumber(0.7 * kvadratura)}
                                    </TableCell>
                                    <TableCell>kom</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Sraf za ploče (TN)</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        {formatNumber(14 * kvadratura)}
                                    </TableCell>
                                    <TableCell>kom</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Sraf lim-lim (bubice)</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        {formatNumber(1.4 * kvadratura)}
                                    </TableCell>
                                    <TableCell>kom</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>Sraf tipl</TableCell>
                                    <TableCell
                                        sx={{
                                            textAlign: `right`,
                                        }}
                                    >
                                        {formatNumber(1.6 * kvadratura)}
                                    </TableCell>
                                    <TableCell>kom</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>
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
                            <Typography
                                sx={{
                                    color: `gray`,
                                }}
                            >
                                HOBI: {formatNumber(1000 * kvadratura)} RSD
                            </Typography>
                            <Typography
                                sx={{
                                    color: `green`,
                                }}
                            >
                                STANDARD:{' '}
                                {formatNumber(1000 * kvadratura * 1.1)} RSD
                            </Typography>
                            <Typography
                                sx={{
                                    color: `orange`,
                                }}
                            >
                                PROFI: {formatNumber(1000 * kvadratura * 1.5)}{' '}
                                RSD
                            </Typography>
                        </Stack>
                    </Paper>
                    <Divider
                        sx={{
                            my: 2,
                        }}
                    />
                    <Accordion>
                        <AccordionSummary expandIcon={<ArrowDownward />}>
                            <Typography variant={`subtitle1`}>
                                Dodaj u korpu
                            </Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            <Grid container textAlign={`center`} spacing={2}>
                                <Grid item sm={12}>
                                    <Typography variant={`subtitle1`} my={2}>
                                        Izaberi klasu fasadnog materijala da bi
                                        dodao u korpu
                                    </Typography>
                                    <Typography variant={`caption`} my={2}>
                                        (klik na ikonicu)
                                    </Typography>
                                </Grid>
                                <Grid item sm={4}>
                                    <Button
                                        color={`secondary`}
                                        sx={{
                                            filter: classFilter,
                                        }}
                                        onClick={() => {}}
                                    >
                                        <Image
                                            src={HobiSvg.src}
                                            alt={'td-hobi'}
                                            width={classWidth}
                                            height={classHeight}
                                        />
                                    </Button>
                                </Grid>
                                <Grid item sm={4}>
                                    <Button
                                        color={`secondary`}
                                        sx={{
                                            filter: classFilter,
                                        }}
                                    >
                                        <Image
                                            src={StandardSvg.src}
                                            alt={'td-standard'}
                                            width={classWidth}
                                            height={classHeight}
                                        />
                                    </Button>
                                </Grid>
                                <Grid item sm={4}>
                                    <Button
                                        color={`secondary`}
                                        sx={{
                                            filter: classFilter,
                                        }}
                                    >
                                        <Image
                                            src={ProfiSvg.src}
                                            alt={'td-profi'}
                                            width={classWidth}
                                            height={classHeight}
                                        />
                                    </Button>
                                </Grid>
                                <Grid item sm={12}>
                                    <Dialog
                                        open={objasnjenjeKlasaShown}
                                        onClose={() => {
                                            setObjasnjenjeKlasaShown(false)
                                        }}
                                    >
                                        <DialogTitle>
                                            Objašnjenje Termodom Klasa
                                        </DialogTitle>
                                        <DialogContent>
                                            <Stack gap={2}>
                                                <Accordion>
                                                    <AccordionSummary
                                                        sx={{
                                                            backgroundColor: `#aaa`,
                                                        }}
                                                        expandIcon={
                                                            <ArrowDownward />
                                                        }
                                                    >
                                                        <Typography
                                                            variant={`subtitle1`}
                                                        >
                                                            Hobi
                                                        </Typography>
                                                    </AccordionSummary>
                                                    <AccordionDetails>
                                                        <List>
                                                            <ListItem>
                                                                Fasadni stiropor
                                                                13-15kg /m3
                                                            </ListItem>
                                                            <ListItem>
                                                                Staklena mrežica
                                                                125g /m2
                                                            </ListItem>
                                                            <ListItem>
                                                                Lepak za
                                                                stiropor
                                                                građevinski
                                                                (grub, slabo
                                                                lepi, teško se
                                                                nanosi mrežica)
                                                            </ListItem>
                                                            <ListItem>
                                                                Tiplovi
                                                                plastični
                                                            </ListItem>
                                                        </List>
                                                    </AccordionDetails>
                                                </Accordion>
                                                <Accordion>
                                                    <AccordionSummary
                                                        sx={{
                                                            backgroundColor: `green`,
                                                        }}
                                                        expandIcon={
                                                            <ArrowDownward />
                                                        }
                                                    >
                                                        <Typography
                                                            variant={`subtitle1`}
                                                        >
                                                            Standard
                                                        </Typography>
                                                    </AccordionSummary>
                                                    <AccordionDetails>
                                                        <List>
                                                            <ListItem>
                                                                Fasadni stiropor
                                                                17kg /m3
                                                            </ListItem>
                                                            <ListItem>
                                                                Staklena mrežica
                                                                145g-150g /m2
                                                            </ListItem>
                                                            <ListItem>
                                                                Lepak za
                                                                stiropor
                                                                građevinski
                                                                Termodom FIX
                                                                (sitne
                                                                granulacije,
                                                                lako se nanosi
                                                                mrežica)
                                                            </ListItem>
                                                            <ListItem>
                                                                Tiplovi
                                                                plastični
                                                            </ListItem>
                                                        </List>
                                                    </AccordionDetails>
                                                </Accordion>
                                                <Accordion>
                                                    <AccordionSummary
                                                        sx={{
                                                            backgroundColor: `orange`,
                                                        }}
                                                        expandIcon={
                                                            <ArrowDownward />
                                                        }
                                                    >
                                                        <Typography
                                                            variant={`subtitle1`}
                                                        >
                                                            Profi
                                                        </Typography>
                                                    </AccordionSummary>
                                                    <AccordionDetails>
                                                        <List>
                                                            <ListItem>
                                                                Fasadni stiropor
                                                                20kg /m3
                                                            </ListItem>
                                                            <ListItem>
                                                                Staklena mrežica
                                                                170g /m2
                                                            </ListItem>
                                                            <ListItem>
                                                                Lepak za
                                                                stiropor i vunu
                                                                građevinski
                                                                (sitne
                                                                granulacije,
                                                                lako se nanosi
                                                                mrežica)
                                                            </ListItem>
                                                            <ListItem>
                                                                Tiplovi sa
                                                                metalnim jezgrom
                                                            </ListItem>
                                                        </List>
                                                    </AccordionDetails>
                                                </Accordion>
                                            </Stack>
                                        </DialogContent>
                                    </Dialog>
                                    <Button
                                        variant={`outlined`}
                                        sx={{
                                            my: 2,
                                        }}
                                        onClick={() => {
                                            setObjasnjenjeKlasaShown(true)
                                        }}
                                    >
                                        Koji materijal pripada kojoj grupi?
                                    </Button>
                                </Grid>
                            </Grid>
                        </AccordionDetails>
                    </Accordion>
                </Stack>
            </Grid>
        </Grid>
    )
}
