import { IAzuriranjeCenaPovezanCellProps } from "../models/IAzuriranjeCenaPovezanCellProps"
import { AzuriranjeCenaPovezanRobaIdDialog } from "./AzuriranjeCenaPovezanRobaIdDialog"
import { Button, CircularProgress, Grid, Typography } from "@mui/material"
import { toast } from "react-toastify"
import { useState } from "react"
import {officeApi} from "@/apis/officeApi";

export const AzuriranjeCenaPovezanCell = (props: IAzuriranjeCenaPovezanCellProps): JSX.Element => {

    const [isDialogOpened, setIsDialogOpened] = useState<boolean>(false)
    const [data, setData] = useState(props.data)
    const [isUpdating, setIsUpdating] = useState<boolean>(false)

    return (
        <Grid>
            <AzuriranjeCenaPovezanRobaIdDialog
                currentRobaId={data.linkRobaId ?? 0}
                isOpen={isDialogOpened}
                naziv={data.naziv ?? 'undefined'}
                handleClose={(value: number | null) => {
                if(value != null) {
                    setIsUpdating(true)
                    officeApi.put(`/web-azuriraj-cene-komercijalno-poslovanje-povezi-proizvode`, {
                        id: data.linkId,
                        robaId: value,
                        webId: data.id
                    }).then(() => {
                        setData((prev) => {
                            return {
                                ...prev,
                                linkRobaId: value
                            }
                        })
                        toast.success(`Uspešno ažuriran povezan RobaId!`)
                        props.onSuccessUpdate()
                    })
                    .catch(() => {
                        props.onErrorUpdate()
                    })
                    .finally(() => {
                        setIsUpdating(false)
                    })
                }
                setIsDialogOpened(false)
            }} />
            <Button
                disabled={isUpdating || props.disabled}
                startIcon={isUpdating ? <CircularProgress size={`1em`} /> : null}
                color={`info`} variant={`contained`} onClick={() => {
                setIsDialogOpened(true)
            }}>
                {
                    data.linkRobaId == null ?
                        'Nije povezan' :
                        <Typography>RobaId: {data.linkRobaId}</Typography>
                }
            </Button>
        </Grid>
    )
}
