import { Autocomplete, Button, CircularProgress, Grid, TextField, Typography } from "@mui/material"
import { NalogZaPrevozTable } from "./NalogZaPrevozTable"
import { DatePicker } from "@mui/x-date-pickers"
import { ApiBase, fetchApi } from "@/app/api"
import { useEffect, useState } from "react"
import { useUser } from "@/app/hooks"
import dayjs from "dayjs"
import { Add, Print } from "@mui/icons-material"
import { NalogZaPrevozNoviDialog } from "./NalogZaPrevozNoviDialog"

export const NalogZaPrevozWrapper = (): JSX.Element => {

    const [reload, setReload] = useState<boolean>(false)
    const [isLoadingData, setIsLoadingData] = useState<boolean>(false)
    const [data, setData] = useState<any[] | undefined>(undefined)
    const [selectedFromDate, setSelectedFromDate] = useState<Date>(new Date())
    const [selectedToDate, setSelectedToDate] = useState<Date>(new Date())
    const [selectedStore, setSelectedStore] = useState<any | null>(null)
    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const user = useUser(false)

    const [newDialogOpened, setNewDialogOpened] = useState<boolean>(false)

    useEffect(() => {
        fetchApi(ApiBase.Main, "/stores")
        .then((response) => { 
            response.json().then((response: any) => {
                setStores(response)
                setSelectedStore(response.find((store: any) => store.id === user.data?.storeId))
            })
        })
    }, [])

    useEffect(() => {
        if(selectedStore === null) return

        setIsLoadingData(true)
        fetchApi(ApiBase.Main, `/nalog-za-prevoz?storeId=${selectedStore.id}&dateFrom=${dayjs(selectedFromDate).format("YYYY-MM-DD")}&dateTo=${dayjs(selectedToDate).format("YYYY-MM-DD")}`)
        .then((response) => {
            response.json().then((response: any) =>
                setData(response)
            )
        })
        .finally(() => {
            setIsLoadingData(false)
        })
    }, [selectedStore, selectedFromDate, selectedToDate, reload])

    return (
        <Grid container spacing={2} p={2}>
            <NalogZaPrevozNoviDialog open={newDialogOpened} store={selectedStore}
                onClose={() => {
                    setNewDialogOpened(false)
                    setReload(!reload)
                }}/>
            <Grid item xs={12}>
                <Grid container spacing={2} alignItems={`center`}>
                    <Grid item>
                        <Typography variant={`h4`}>Nalog za prevoz</Typography>
                    </Grid>
                    <Grid item>
                        <Button variant={`contained`} startIcon={<Add />} disabled={selectedStore === null || isLoadingData} onClick={() => {
                            setNewDialogOpened(true)
                        }}>Novi</Button>
                    </Grid>
                    <Grid item>
                        <Button variant={`outlined`} startIcon={<Print />} disabled={selectedStore === null || isLoadingData} onClick={() => {
                            var css = '@page { size: landscape; }',
                            head = document.head || document.getElementsByTagName('head')[0],
                            style: any = document.createElement('style');
                        
                            style.type = 'text/css';
                            style.media = 'print';
                            
                            if (style.styleSheet){
                                style.styleSheet.cssText = css;
                            } else {
                                style.appendChild(document.createTextNode(css));
                            }
                            
                            head.appendChild(style);
                            window.print()
                        }}>Print</Button>
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
                            disabled // TODO: Change this with permissions
                            getOptionLabel={(option) => { return `[ ${option.id} ] ${option.name}` }}
                            renderInput={(params) => <TextField {...params} label={`magacin`}/>}
                        />}
                    </Grid>
                    <Grid item>
                        <DatePicker
                            disabled={selectedStore === null || isLoadingData}
                            label="Od datuma"
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e: any) => {
                                setSelectedFromDate(e)
                            }}
                        />
                    </Grid>
                    <Grid item>
                        <DatePicker
                            disabled={selectedStore === null || isLoadingData}
                            label="Do datuma"
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e: any) => {
                                setSelectedToDate(e)
                            }}
                        />
                    </Grid>
                </Grid>
            </Grid>
            <NalogZaPrevozTable data={data} />
        </Grid>
    )
}