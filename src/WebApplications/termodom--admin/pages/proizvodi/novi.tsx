import { useRef } from "react"
import Input from "../../components/input"
import Button from "../../components/button"
import { apiPostAsync, apiPutAsync } from "../../api/api"
import { toast } from "react-toastify"

export default function ProizvodNovi() {
    return (
        <div className={`p-2 max-w-screen-lg mx-auto`}>
            <Content />
        </div>
    )
}

const Content = () => {

    const robaIdRef = useRef<HTMLInputElement>(null)

    return (
        <div>
            <div className={`text-lg m-1 font-medium`}>Dodavanje novog proizvoda na web</div>
            <Input forwardRef={robaIdRef} type="text" placeholder="RobaId" labelText="RobaId:" required={true} />
            <Button text="Dodaj" onClick={() => {

                if(robaIdRef?.current?.value == null) {
                    toast.error("Morate uneti RobaId!", {
                        position: toast.POSITION.TOP_CENTER
                    })
                    return
                }
                
                apiPutAsync("/products", {
                    "name": "Novi proizvod",
                    "thumbnailImagePath": "none",
                    "fullSizedImagePath": "none",
                    "sku": "Novi Proizvod",
                    "unit": "Novi"
                  })
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
                                toast.success("Proizvod uspešno dodat", {
                                    position: toast.POSITION.TOP_CENTER
                                })
                            }
                        })
                  })
            }} />
        </div>
    )
}