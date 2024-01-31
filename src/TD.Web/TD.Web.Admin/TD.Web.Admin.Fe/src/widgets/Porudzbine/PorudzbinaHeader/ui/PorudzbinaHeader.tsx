import { Box, Grid, MenuItem, Paper, TextField, Typography } from "@mui/material"
import { IPorudzbinaHeaderProps } from "../models/IPorudzbinaHeaderProps"
import { mainTheme } from "@/app/theme"
import moment from 'moment'
import { PorudzbinaHeaderDropdownStyled } from "./PorudzbinaHeaderDropdownStyled"

export const PorudzbinaHeader = (props: IPorudzbinaHeaderProps): JSX.Element => {

    const styles = (mainTheme: any) => ({
        notchedOutline: {
            borderColor: `${mainTheme.palette.secondary.contrastText} !important`
        }
    })

    const classes = styles(mainTheme)

    return (
        <Paper
            sx={{
                m: 2,
                p: 2,
                backgroundColor: mainTheme.palette.secondary.light,
                color: mainTheme.palette.secondary.contrastText
            }}>
                <Grid
                    container
                    width={`100%`}
                    spacing={1}>
                    <Grid
                        item
                        sm={3}>
                        <Typography>
                            WEB: {props.porudzbina.oneTimeHash.substring(0, 8)}
                        </Typography>
                        <Typography>
                            TD: 0
                        </Typography>
                        <Typography>
                            Datum: {moment(props.porudzbina.createdAt).format(`DD.MM.YYYY. HH:mm`)}
                        </Typography>
                        <Typography>
                            {props.porudzbina.user}
                        </Typography>
                    </Grid>
                    <Grid
                        item
                        sm={9}>
                        <Grid
                            container
                            spacing={1}
                            width={`100%`}>
                            <Grid
                                item
                                sm={4}>
                                <PorudzbinaHeaderDropdownStyled
                                    id='store'
                                    select
                                    onChange={(e) => {
                                        // setRequestBody((prev: any) => { return { ...prev, productPriceGroupId: e.target.value } })
                                    }}
                                    label='Mesto preuzimanja'
                                    helperText='Izaberite mesto preuzimanja'>
                                        <MenuItem value={0}>
                                            Smederevski Put 14
                                        </MenuItem>
                                </PorudzbinaHeaderDropdownStyled>
                            </Grid>
                            <Grid
                                item
                                sm={4}>
                                <PorudzbinaHeaderDropdownStyled
                                    id='status'
                                    select
                                    onChange={(e) => {
                                        // setRequestBody((prev: any) => { return { ...prev, productPriceGroupId: e.target.value } })
                                    }}
                                    label='Status'
                                    helperText='Izaberite status porudžbine'>
                                        <MenuItem value={0}>
                                            Open
                                        </MenuItem>
                                </PorudzbinaHeaderDropdownStyled>
                            </Grid>
                            <Grid
                                item
                                sm={4}>
                                <PorudzbinaHeaderDropdownStyled
                                    id='nacin-placanja'
                                    select
                                    color={`secondary`}
                                    onChange={(e) => {
                                        // setRequestBody((prev: any) => { return { ...prev, productPriceGroupId: e.target.value } })
                                    }}
                                    label='Način plaćanja'
                                    helperText='Izaberite način plaćanja'>
                                        <MenuItem value={0}>
                                            Gotovinom
                                        </MenuItem>
                                        <MenuItem value={0}>
                                            Karticom
                                        </MenuItem>
                                </PorudzbinaHeaderDropdownStyled>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
        </Paper>
    )
}