import { useRouter } from "next/router"
import { useEffect, useState } from "react"
import { apiGetAsync, apiPutAsync } from "../../../api/api"
import { toast } from "react-toastify"
import LoadAnim from "../../../components/loadAnim"
import Input from "../../../components/input"
import Button from "../../../components/button"

export default function ProizvodiIzmeni() {
    const router = useRouter()
    const [proizvod, setProizvod] = useState(null)
    const Spinner = require('react-spinkit')

    useEffect(() => {
        apiGetAsync(`/products?id=${router.query.id}`)
            .then(response => {
                if(response.status != 200) {
                    console.error(response.status)
                    toast.error("Doslo je do greške prilikom komunikacije sa API-jem", {
                        position: toast.POSITION.TOP_CENTER
                    })
                    return
                }

                response.json()
                    .then((lsResponse: any) => {
                        if(lsResponse.status == 400) {
                            lsResponse.errors.map((err: string) => {
                                toast.error(err, {
                                    position: toast.POSITION.TOP_CENTER
                                })
                            })
                        } else if(lsResponse.status != 200) {
                            console.error(lsResponse.status)
                        } else {
                            setProizvod(lsResponse.payload[0])
                        }
                    })
            })
    }, [router.query.id])
    
    return (
        <div className={`p-2 max-w-screen-lg mx-auto`}>
            {
                proizvod ?
                    <Content proizvod={proizvod} setProizvod={setProizvod} /> :
                    <LoadAnim />
            }
        </div>
    )
}

const Content = ({ proizvod, setProizvod }: any) => {

    const [pendingSave, setPendingSave] = useState(false)
    const [saveInProgress, setSaveInProgress] = useState(false)
    const [deactivateInProgress, setDeactivateInProgress] = useState(false)
    const [potvrdiDeaktivaciju, setPotvrdiDeaktivaciju] = useState(false)

    console.log(proizvod)
    return (
        <div>
            <div className={`font-medium text-xl my-5`}>Uređivanje proizvoda</div>
                <img src={proizvod.fullSizedImagePath} />
                <input type="file" />
            <div>
            </div>
            <div>
                <Input labelText="Id:"
                    readOnly={true}
                    type={"text"}
                    defaultValue={proizvod.id} />
                
                <Input labelText="Naziv:"
                    type={"text"}
                    readOnly={saveInProgress}
                    defaultValue={proizvod.name}
                    onKeyUp={(event: any) => {
                        setPendingSave(true)
                        setProizvod((prev: any) => ({...prev, name: event.target.value }))
                        }} />
                
                <Input labelText="Kat. Br.:"
                    type={"text"}
                    readOnly={saveInProgress}
                    defaultValue={proizvod.sku}
                    onKeyUp={(event: any) => {
                        setPendingSave(true)
                        setProizvod((prev: any) => ({...prev, sku: event.target.value }))
                        }} />
                
                <Input labelText="JM:"
                    type={"text"}
                    readOnly={saveInProgress}
                    defaultValue={proizvod.unit}
                    onKeyUp={(event: any) => {
                        setPendingSave(true)
                        setProizvod((prev: any) => ({...prev, unit: event.target.value }))
                        }} />

                {
                    pendingSave ?
                        <Button
                            disabled={saveInProgress}
                            text="Sačuvaj"
                            additionalClasses={`${saveInProgress ? 'animate-pulse' : '' }`}
                            onClick={() => {
                                setSaveInProgress(true)
                                apiPutAsync("/products", proizvod)
                                .then(response => {
                                    if(response.status != 200) {
                                        console.error(response.status)
                                        toast.error("Doslo je do greške prilikom komunikacije sa API-jem", {
                                            position: toast.POSITION.TOP_CENTER
                                        })
                                        setSaveInProgress(false)
                                        return
                                    }
                
                                    response.json()
                                        .then((lsResponse: any) => {
                                            if(lsResponse.status == 400) {
                                                lsResponse.errors.map((err: string) => {
                                                    toast.error(err, {
                                                        position: toast.POSITION.TOP_CENTER
                                                    })
                                                })
                                            } else if(lsResponse.status != 200) {
                                                console.error(lsResponse.status)
                                            } else {
                                                toast.success("Proizvod uspešno izmenjen", {
                                                    position: toast.POSITION.TOP_CENTER
                                                })
                                                setPendingSave(false)
                                            }
                                            setSaveInProgress(false)
                                        })
                                })
                            }}/> : ""
                }
            </div>
            <div>
                <Button
                    disabled={deactivateInProgress}
                    text="Deaktiviraj proizvod"
                    onClick={() => {
                        toast.warning("Ako sigurno zelite deaktivirati proizvod, pritisnite ponovo dugme ispod!", {
                            position: toast.POSITION.TOP_CENTER
                        })
                        setPotvrdiDeaktivaciju(true)
                    }} />
                    
                {
                    potvrdiDeaktivaciju ?
                    <Button
                        disabled={deactivateInProgress}
                        additionalClasses={deactivateInProgress ? `animate-pulse` : ''}
                        text="Potvrdi deaktivaciju proizvoda"
                        onClick={() => {
                            toast.error("Ovo jos uvek nije implementirano", {
                                position: toast.POSITION.TOP_CENTER
                            })
                            setDeactivateInProgress(true)
                            setPotvrdiDeaktivaciju(false)
                        }} /> : ""
                }
            </div>
        </div>
    )
}