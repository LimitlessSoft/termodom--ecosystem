import styles from './novi.module.css'
import Head from "next/head";
import Header from "../../../components/Header";
import Image from 'next/image';
import { useRouter } from 'next/router';
import { useEffect, useRef, useState } from 'react';
import { apiFetch } from '../../../api'

export default function NoviProizvod() {

    const [grupeProizvoda, setGrupeProizvoda] = useState(null)
    const [podgrupeProizvoda, setPodgrupeProizvoda] = useState(null)
    const [cenovneGrupeProizvoda, setCenovneGrupeProizvoda] = useState(null)
    const rout = useRouter()

    const [trenutnoSelektovanaGrupa, setTrenutnoSelektovanaGrupa] = useState(null)

    const grupaProizvodaInputRef = useRef(null)

    const ucitajGrupeProizvoda = () => {
        apiFetch('/webshop/proizvod/grupa/list', 'get', null, null)
            .then(r => {
                if(r.status == 200) {
                    r.json().then(r => {
                        setGrupeProizvoda(r)
                    })
                } else {
                    console.log("Greska ucitavanja grupe proizvoda sa API-ja")
                }
            })
    }

    const ucitajPodgrupeProizvoda = () => {
        apiFetch('/webshop/proizvod/podgrupa/list', 'get', null, null)
            .then(r => {
                if(r.status == 200) {
                    r.json().then(r => {
                        setPodgrupeProizvoda(r)
                    })
                } else {
                    console.log("Greska ucitavanja podgrupa proizvoda sa API-ja")
                }
            })
    }

    const ucitajCenovneGrupeProizvoda = () => {
        apiFetch('/webshop/proizvod/cenovnagrupa/list', 'get', null, null)
            .then(r => {
                if(r.status == 200) {
                    r.json().then(r => {
                        console.log(r)
                        setCenovneGrupeProizvoda(r)
                    })
                } else {
                    console.log("Greska ucitavanja cenovnih grupa proizvoda sa API-ja")
                }
            })
    }

    useEffect(() => {
        ucitajGrupeProizvoda()
        ucitajPodgrupeProizvoda()
        ucitajCenovneGrupeProizvoda()
    }, [])

    return (
        <div>
            <Head>
                <title>Termodom - centar građevinskog materijala</title>
                <meta name="description" content="Termodom - centar građevinskog materijala" />
                <link rel="icon" href="/favicon.ico" />
            </Head>

            <Header />

            <div className={styles.innerWrapper}>
                <input type='file' id='imageInput' accept='.jpg, .jpeg, .png' hidden />
                <label className={styles.pozadinaWrapper} htmlFor='imageInput'>
                    <Image src='/Termodom_Logo_White.svg' width={300} height={300} />
                </label>
                <div className={styles.inputDataWrapper}>
                    <label>RobaID:</label>
                    <input type='text' placeholder='RobaID' defaultValue={rout.query.robaid} />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Slika:</label>
                    <input type='text' placeholder='slikaPath' defaultValue={rout.query.slikaPath} />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Grupa:</label>
                    {
                        grupeProizvoda == null ? 
                            <label>ucitavanje...</label> :
                            <select ref={grupaProizvodaInputRef} onChange={(e) => {
                                setTrenutnoSelektovanaGrupa(e.currentTarget.value)
                            }}>
                                {
                                    grupeProizvoda.map(gp => {
                                        return (
                                            <option key={`1421fsaf-${gp.id}`} value={gp.id}>{gp.naziv}</option>
                                        )
                                    })
                                }
                            </select>
                    }
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Podgrupa:</label>
                    {
                        podgrupeProizvoda == null ? 
                            <label>ucitavanje...</label> :
                            <select>
                                {
                                    podgrupeProizvoda.map(pgp => {
                                        if(pgp.grupaid == trenutnoSelektovanaGrupa) {
                                            return (
                                                <option data-grupaid={pgp.grupaid} key={`1421fsaf-${pgp.id}`} value={pgp.id}>{pgp.naziv}</option>
                                            )
                                        } else {
                                            return null
                                        }
                                    })
                                }
                            </select>
                    }
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>PDV:</label>
                    <input type='text' placeholder='PDV' defaultValue={20} />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Display Index:</label>
                    <input type='text' placeholder='Display Index' defaultValue={0} />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Kratak Opis:</label>
                    <textarea defaultValue='Kratak opis'></textarea>
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Polazna Cena:</label>
                    <input type='text' placeholder='Polazna Cena' defaultValue={0} />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Prodajna Cena:</label>
                    <input type='text' placeholder='Prodajna Cena' defaultValue={0} />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Rel (link):</label>
                    <input type='text' placeholder='Rel' />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Detaljan Opis:</label>
                    <textarea defaultValue='Detaljan opis'></textarea>
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Klasifikacija:</label>
                    <select>
                        <option defaultValue={0}>Hobi</option>
                        <option defaultValue={1}>Standard</option>
                        <option defaultValue={2}>Profi</option>
                    </select>
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Transportno Pakovanje Kolicina:</label>
                    <input type='text' placeholder='Transportno Pakovanje Kolicina' />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Transportno Pakovanje JM:</label>
                    <input type='text' placeholder='Transportno Pakovanje JM' />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Kupovina Samo U Transportnom Pakovanju:</label>
                    <input type='text' placeholder='Kupovina samo u transportnom pakovanju' />
                </div>
                <div className={styles.inputDataWrapper}>
                    <label>Cenovna Grupa ID:</label>
                    {
                        cenovneGrupeProizvoda == null ? 
                            <label>ucitavanje...</label> :
                            <select>
                                {
                                    cenovneGrupeProizvoda.map(cg => {
                                        return (
                                            <option key={`21fafsf-${cg.id}`} value={cg.id}>{cg.naziv}</option>
                                        )
                                    })
                                }
                            </select>
                    }
                </div>

                <button onClick={() => {
                    
                }}>Kreiraj proizvod</button>
            </div>
        </div>
    )
}