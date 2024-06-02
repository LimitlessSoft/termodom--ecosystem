import { Autocomplete, Button, CircularProgress, Grid, TextField, Typography } from "@mui/material"
import { NalogZaPrevozTable } from "./NalogZaPrevozTable"
import { DatePicker } from "@mui/x-date-pickers"
import { ApiBase, fetchApi } from "@/app/api"
import { useEffect, useState } from "react"
import { useUser } from "@/app/hooks"
import dayjs from "dayjs"
import { Add } from "@mui/icons-material"
import { NalogZaPrevozNoviDialog } from "./NalogZaPrevozNoviDialog"

export const NalogZaPrevozWrapper = (): JSX.Element => {

    const [selectedDate, setSelectedDate] = useState<Date>(new Date())
    const [selectedStore, setSelectedStore] = useState<any | null>(null)
    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const user = useUser(false)

    const [newDialogOpened, setNewDialogOpened] = useState<boolean>(false)

    useEffect(() => {
        fetchApi(ApiBase.Main, "/stores")
        .then((response) => { 
            setStores(response)
            setSelectedStore(response.find((store: any) => store.id === user.data?.storeId))
        })
    }, [])

    useEffect(() => {
        // Load files for selected store
    }, [selectedStore])

    return (
        <Grid container spacing={2} p={2}>
            <NalogZaPrevozNoviDialog open={newDialogOpened} store={selectedStore}
                onClose={() => {
                    setNewDialogOpened(false)
                }}/>
            <Grid item xs={12}>
                <Grid container spacing={2} alignItems={`center`}>
                    <Grid item>
                        <Typography variant={`h4`}>Nalog za prevoz</Typography>
                    </Grid>
                    <Grid item>
                        <Button variant={`contained`} startIcon={<Add />} disabled={selectedStore === null} onClick={() => {
                            setNewDialogOpened(true)
                        }}>Novi</Button>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <Grid container spacing={1}>
                    <Grid item xs={12} sm={6}>
                        { stores === undefined && <CircularProgress /> }
                        { stores !== undefined && stores.length === 0 && <h2>Nema dostupnih prodavnica</h2>}
                        { stores !== undefined && stores.length > 0 && <Autocomplete
                            defaultValue={stores!.find((store) => store.id === user.data?.storeId)}
                            options={stores!}
                            onChange={(event, value) => {
                                setSelectedStore(value)
                            }}
                            getOptionLabel={(option) => { return `[ ${option.id} ] ${option.name}` }}
                            renderInput={(params) => <TextField {...params} label={`magacin`}/>}
                        />}
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <DatePicker
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e: any) => {
                                setSelectedDate(e)
                            }}
                        />
                    </Grid>
                </Grid>
            </Grid>
            <NalogZaPrevozTable />
        </Grid>
    )
}