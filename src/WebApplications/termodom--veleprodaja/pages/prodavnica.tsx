import { useEffect, useState } from "react"
import { apiGetAsync } from "../api/api"
import ProizvodCard from "../components/prodavnica/proizvodCard"

export default function MainPage() {

    const [proizvodi, setProizvodi] = useState<any[]>([])

    useEffect(() => {
        apiGetAsync("api/roba/list")
        .then(response => {
                response.json()
                    .then((apiProizvodi: ApiProizvod[]) => {
                        apiGetAsync("webshop/proizvod/list")
                        .then(response => {
                            response.json()
                                .then((webshopProizvodi: any[]) => {
                                    let list: any[] = []
                                    webshopProizvodi.map(webshopProizvod => {
                                        if(webshopProizvod.aktivan == 0)
                                            return

                                        var apiProizvod = apiProizvodi.find(x => x.id == webshopProizvod.robaID)
                                        var a = {
                                            ...apiProizvod,
                                            ...webshopProizvod
                                        }

                                        list.push(a)
                                    })
                                    setProizvodi(list)
                                })
                        })
                    })
            }
        )
        .catch(x => {
            console.log("Error getting api/roba/list")
        })
    }, [])

    return (
        <div className={`p-2 max-w-screen-lg mx-auto`}>
            {
                proizvodi == null ?
                "Ucitavanje" :
                <div className={`grid grid-cols-5 gap-4`}>
                    {
                        proizvodi.map(proizvod => {
                            return <ProizvodCard proizvod={proizvod} />
                        })
                    }
                </div>
            }
        </div>
    )
}