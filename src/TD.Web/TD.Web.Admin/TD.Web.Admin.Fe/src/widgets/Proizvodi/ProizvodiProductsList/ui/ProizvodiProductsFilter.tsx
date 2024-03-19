import { Button, CircularProgress, Grid, LinearProgress, TextField } from "@mui/material"
import { useState } from "react"

export const ProizvodiProductsFilter = (props: any): JSX.Element => {

    const [text, setText] = useState<string>("")

    return (
        <Grid
            container
            alignItems={`center`}
            p={2}>
            <TextField
                disabled={props.isFetching}
                sx={{
                    minWidth: 400
                }}
                onChange={(e) => {
                    setText(e.target.value)
                }}
                onKeyDown={(e) => {
                    if (e.key === 'Enter' || e.key === 'Return') {
                        props.onPretrazi(text)
                    }
                }}
                placeholder="Pretraga..." />
            <Button variant={`contained`}
                disabled={props.isFetching}
                sx={{
                    m: 2
                }}
                onClick={() => {
                    props.onPretrazi(text)
                }}>Pretrazi</Button>
            <Grid item>
                {
                    props.isFetching && <CircularProgress />
                }
            </Grid>
        </Grid>
    )
}