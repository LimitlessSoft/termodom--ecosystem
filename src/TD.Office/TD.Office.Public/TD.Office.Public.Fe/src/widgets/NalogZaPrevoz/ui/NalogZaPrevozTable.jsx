import {
    Button,
    Grid,
    IconButton,
    Menu,
    MenuItem,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { formatNumber } from '@/helpers/numberHelpers'
import { PRINT_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { MoreVert, Print } from '@mui/icons-material'
import NextLink from 'next/link'
import moment from 'moment'
import { useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'

export const NalogZaPrevozTable = (props) => {
    const [anchorEl, setAnchorEl] = useState(null)
    const [selectedRow, setSelectedRow] = useState(null)

    const handleMenuOpen = (event, row) => {
        setAnchorEl(event.currentTarget)
        setSelectedRow(row)
    }

    const handleMenuClose = () => {
        setAnchorEl(null)
        setSelectedRow(null)
    }

    const handleStorniraj = () => {
        if (!selectedRow) return

        officeApi
            .patch(`/nalog-za-prevoz/${selectedRow.id}/status`, {
                status: 1, // Cancelled status
            })
            .then(() => {
                handleMenuClose()
                if (props.onReload) {
                    props.onReload()
                }
            })
            .catch((err) => handleApiError(err))
    }

    return (
        <Grid item sm={12}>
            {(props.data === undefined ||
                (props.data !== undefined && props.data.length === 0)) && (
                <Typography>Nema podataka za prikazati</Typography>
            )}

            {props.data !== undefined && props.data.length > 0 && (
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell align={`center`}>Broj naloga</TableCell>
                            <TableCell>Datum</TableCell>
                            <TableCell>Adresa</TableCell>
                            <TableCell>Mobilni</TableCell>
                            <TableCell>VrDok</TableCell>
                            <TableCell>BrDok</TableCell>
                            <TableCell>Napomena</TableCell>
                            <TableCell>Prevoznik</TableCell>
                            <TableCell>Cena prevoza bez PDV</TableCell>
                            <TableCell>Od toga mi kupcu naplatili</TableCell>
                            <TableCell
                                className={
                                    PRINT_CONSTANTS.PRINT_CLASSNAMES.NO_PRINT
                                }
                            ></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {props.data.map((row, index) => (
                            <TableRow
                                key={index}
                                sx={{
                                    borderLeft: `5px solid ${row.status === 0 ? '#4caf50' : '#9c27b0'}`,
                                    backgroundColor: row.status !== 0 ? '#f3e5f5' : 'transparent',
                                    opacity: row.status !== 0 ? 0.7 : 1,
                                    textDecoration: row.status !== 0 ? 'line-through' : 'none',
                                }}
                            >
                                <TableCell align={`center`}>{row.id}</TableCell>
                                <TableCell>
                                    {moment(row.createdAt).format(
                                        'DD.MM.yyyy (HH:mm)'
                                    )}
                                </TableCell>
                                <TableCell>{row.address}</TableCell>
                                <TableCell>{row.mobilni}</TableCell>
                                <TableCell>{row.vrDok}</TableCell>
                                <TableCell>{row.brDok}</TableCell>
                                <TableCell>{row.note}</TableCell>
                                <TableCell>{row.prevoznik}</TableCell>
                                <TableCell>
                                    {formatNumber(row.cenaPrevozaBezPdv)}{' '}
                                    {row.placenVirmanom && `(V)`}
                                </TableCell>
                                <TableCell>
                                    {formatNumber(row.miNaplatiliKupcuBezPdv)}
                                </TableCell>
                                <TableCell
                                    className={
                                        PRINT_CONSTANTS.PRINT_CLASSNAMES
                                            .NO_PRINT
                                    }
                                >
                                    <Button
                                        LinkComponent={NextLink}
                                        color={`secondary`}
                                        href={`/nalog-za-prevoz/${row.id}?noLayout=true`}
                                        target={`_blank`}
                                        disabled={
                                            row.status !== 0 ||
                                            !hasPermission(
                                                props.permissions,
                                                PERMISSIONS_CONSTANTS
                                                    .USER_PERMISSIONS
                                                    .NALOG_ZA_PREVOZ
                                                    .INDIVIDUAL_ORDER_PRINT
                                            )
                                        }
                                    >
                                        <Print />
                                    </Button>
                                    <IconButton
                                        onClick={(e) => handleMenuOpen(e, row)}
                                        size="small"
                                    >
                                        <MoreVert />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            )}
            <Menu
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={handleMenuClose}
            >
                <MenuItem
                    onClick={handleStorniraj}
                    disabled={selectedRow?.status !== 0}
                >
                    Storniraj
                </MenuItem>
            </Menu>
        </Grid>
    )
}
