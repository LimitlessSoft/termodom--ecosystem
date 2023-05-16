import styles from './index.module.css'
import Head from "next/head"
import Header from "../../../components/Header"
import { useEffect, useState } from 'react'
import { apiFetch } from '../../../api'
import Link from 'next/link'

const ListaProizvoda = () => {
    const [proizvodi, setProizvodi] = useState(null)

    const ucitajProizvode = () => {
        apiFetch('/webshop/proizvod/list', 'GET', null, null)
            .then((r) => {
                r.json().then(r => setProizvodi(r))
            })
            .catch((r) => {
                console.log(r)
            })
    }

    useEffect(() => {
        ucitajProizvode()
    }, [])

    const ListaRobe = (props) => {
        const proizvodi = props.proizvodi

        const [roba, setRoba] = useState(null)
        const [robaKojaNepostojiUEkosistemu, setRobaKojaNepostojiUEkosistemu] = useState(null)

        const ucitajRobu = () => {
            apiFetch('/roba/list', 'GET', null, null)
                .then((r) => {
                    r.json().then(r => setRoba(r))
                })
                .catch((r) => {
                    console.log(r)
                })
        }

        useEffect(() => {
            ucitajRobu()
        }, [])

        useEffect(() => {
            if(roba == null || proizvodi == null) {
                setRobaKojaNepostojiUEkosistemu(null)
                return
            }

            let f = roba.filter((x => proizvodi.find(y => y.robaid == x.robaid) == null))
            setRobaKojaNepostojiUEkosistemu(f)
        }, [roba, proizvodi])

        return (
            <div>
                {
                    robaKojaNepostojiUEkosistemu == null ? null :
                    <div className={styles.listaRobeWrapper}>
                        <h3>Lista robe koja postoji u ekosistemu,
                            ali ne postoji na sajtu kao proizvod</h3>
                        <div className={styles.innerWrapper}>
                            {
                                robaKojaNepostojiUEkosistemu.map(roba => {
                                    return (
                                        <div key={`robaekosistema-${roba.robaid}`}
                                            className={styles.item}>
                                                <span className={styles.robaListRobaID}>{roba.robaid}</span>
                                                <span className={styles.robaListKatBr}>{roba.katbr}</span>
                                                <span className={styles.robaListNaziv}>{roba.naziv}</span>
                                                <span className={styles.robaListJM}>{roba.jm}</span>
                                                <Link href={`/panel/proizvodi/novi?naziv=${roba.naziv}&robaid=${roba.robaid}&katbr=${roba.katbr}&jm=${roba.jm}`}>Insertuj kao proizvod</Link>
                                        </div>
                                    )
                                })
                            }
                        </div>
                    </div>
                }
            </div>
        )
    }

    return (
        <div>
            lista proizvoda
            <ListaRobe proizvodi={proizvodi} />
            <div>
                { JSON.stringify(proizvodi) }
            </div>
        </div>
    )
}

export default function Proizvodi() {
    return (
        <div>
            <Head>
                <title>Termodom - centar građevinskog materijala</title>
                <meta name="description" content="Termodom - centar građevinskog materijala" />
                <link rel="icon" href="/favicon.ico" />
            </Head>

            <Header />

            <div className={styles.innerWrapper}>
                <ListaProizvoda />
            </div>
        </div>
    )
}