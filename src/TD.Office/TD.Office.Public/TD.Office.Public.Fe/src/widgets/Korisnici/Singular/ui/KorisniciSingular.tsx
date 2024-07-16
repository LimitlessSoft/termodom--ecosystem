import { IKorisniciSingularProps } from "../interfaces/IKorisniciSingularProps"
import { KorisniciSingularDataField } from "./KorisniciSingularDataField"
import {ApiBase, ContentType, fetchApi} from "@/api"
import { Grid } from "@mui/material"
import { toast } from "react-toastify"

export const KorisniciSingular = (props: IKorisniciSingularProps): JSX.Element => {
    return (
        <Grid container p={2} maxWidth={500}>
            <KorisniciSingularDataField defaultValue={props.user.id} preLabel={`Id:`} />
            <KorisniciSingularDataField defaultValue={props.user.username} preLabel={`Username:`} />
            <KorisniciSingularDataField editable defaultValue={props.user.nickname} preLabel={`Nadimak:`}
                onSave={(v) => new Promise<void>((resolve, reject) => {
                    fetchApi(ApiBase.Main, `/users/${props.user.id}/nickname`, {
                        method: `PUT`,
                        body: {
                            id: props.user.id,
                            nickname: v
                        },
                        contentType: ContentType.ApplicationJson
                    })
                    .then(_ => {
                        toast.success(`Nadimak je uspešno sačuvan`)
                        resolve()
                    }).catch(_ => {
                        reject()
                    })
                })}/>
        </Grid>
    )
}