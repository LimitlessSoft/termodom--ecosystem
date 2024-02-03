import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material"
import { ProfiKutakPanelBase } from "../../ProfiKutakSkorasnjePorudzbinePanelBase"
import { formatNumber } from "@/app/helpers/numberHelpers"
import { mainTheme } from "@/app/theme"

export const ProfiKutakSkorasnjePorudzbinePanel = (): JSX.Element => {
    return (
        <ProfiKutakPanelBase title={`Skorašnje porudžbine`}>
            <TableContainer component={Paper}>
                <Table sx={{ width: `100%` }} aria-label='Korpa'>
                    <TableHead>
                        <TableRow>
                            <TableCell>Broj</TableCell>
                            <TableCell>Datum</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Vrednost sa PDV</TableCell>
                            <TableCell>Ušteda</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <TableRow>
                            <TableCell>7CBBAC32</TableCell>
                            <TableCell>01.02.2024.</TableCell>
                            <TableCell>
                                <Typography
                                    sx={{
                                        color: mainTheme.palette.warning.main,
                                        fontWeight: `600`
                                    }}>
                                    Obrađuje se
                                </Typography>
                            </TableCell>
                            <TableCell>{formatNumber(27859.57)}</TableCell>
                            <TableCell>
                                <Typography
                                    sx={{
                                        color: mainTheme.palette.success.main,
                                        fontWeight: `600`
                                    }}>
                                    {formatNumber(2859.24)}
                                </Typography>
                            </TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>87GPI8AH</TableCell>
                            <TableCell>01.02.2024.</TableCell>
                            <TableCell>
                                <Typography
                                    sx={{
                                        color: mainTheme.palette.success.main,
                                        fontWeight: `600`
                                    }}>
                                    Preuzeto
                                </Typography>
                            </TableCell>
                            <TableCell>{formatNumber(27859.57)}</TableCell>
                            <TableCell>
                                <Typography
                                    sx={{
                                        color: mainTheme.palette.success.main,
                                        fontWeight: `600`
                                    }}>
                                    {formatNumber(2859.24)}
                                </Typography>
                            </TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>87GPI8AH</TableCell>
                            <TableCell>01.02.2024.</TableCell>
                            <TableCell>
                                <Typography
                                    sx={{
                                        color: mainTheme.palette.success.main,
                                        fontWeight: `600`
                                    }}>
                                    Za preuzimanje
                                </Typography>
                            </TableCell>
                            <TableCell>{formatNumber(27859.57)}</TableCell>
                            <TableCell>
                                <Typography
                                    sx={{
                                        color: mainTheme.palette.success.main,
                                        fontWeight: `600`
                                    }}>
                                    {formatNumber(2859.24)}
                                </Typography>
                            </TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
        </ProfiKutakPanelBase>
    )
}