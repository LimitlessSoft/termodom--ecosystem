import styles from './podgrupe.module.css'
import Head from "next/head";
import Header from "../../../components/Header";
import { apiFetch } from "../../../api"
import { useEffect, useState } from 'react';

export default function Podgrupe() {

    const [grupeProizvoda, setGrupeProizvoda] = useState(null)
    const [podgrupeProizvoda, setPodgrupeProizvoda] = useState(null)

    const ucitajGrupeProizvoda = () => {
        return new Promise((resolve, reject) => {
            apiFetch('/webshop/proizvod/grupa/list', 'get', null, null)
                .then(r => {
                    if(r.status == 200) {
                        r.json().then(r => {
                            setGrupeProizvoda(r)
                        })
                    } else {
                        console.log("Greska ucitavanja grupe proizvoda sa API-ja")
                    }
                    resolve()
                })
        })
    }

    const ucitajPodgrupeProizvoda = () => {
        return new Promise((resolve, reject) => {
            apiFetch('/webshop/proizvod/podgrupa/list', 'get', null, null)
                .then(r => {
                    if(r.status == 200) {
                        r.json().then(r => {
                            setPodgrupeProizvoda(r)
                        })
                    } else {
                        console.log("Greska ucitavanja grupe proizvoda sa API-ja")
                    }
                    resolve()
                })
        })
    }

    useEffect(() => {
        ucitajGrupeProizvoda()
        ucitajPodgrupeProizvoda()
    }, [])

    return (
        <div>
            <Head>
                <title>Termodom - centar građevinskog materijala</title>
                <meta name="description" content="Termodom - centar građevinskog materijala" />
                <link rel="icon" href="/favicon.ico" />
            </Head>

            <Header />

            <div className={styles.mainWrapper}>
                {
                    grupeProizvoda == null ? <div>ucitavanje grupa...</div> :
                        <div>
                            {
                                grupeProizvoda.map((gp) => {
                                    return (
                                        <div key={`12fwqfsz-${gp.id}`}>
                                            <div className={styles.grupaHeaderWrapper}>
                                                <h3>{gp.naziv}</h3>
                                                {
                                                    podgrupeProizvoda == null || podgrupeProizvoda.find(x => x.grupaid == gp.id) != null ? null :
                                                    <button onClick={() => {
                                                        apiFetch('/webshop/proizvod/grupa/delete', 'delete', 'application/json', JSON.stringify({
                                                            id: gp.id
                                                        }))
                                                        .then(r => {
                                                            if(r.status == 200) {
                                                                ucitajGrupeProizvoda()
                                                                ucitajPodgrupeProizvoda()
                                                                return
                                                            }
            
                                                            if(r.status == 409) {
                                                                r.json()
                                                                .then(r => {
                                                                    alert(r.msg)
                                                                    alert('Podgrupe koje pripadaju ovoj grupi su: ' + JSON.stringify(r.listaPodgrupa))
                                                                })
                                                                return
                                                            }
                                                            
                                                            if(r.status == 500) {
                                                                console.log('greska na apiju')
                                                                alert('Greska komunikacije sa API-jem!')
                                                                return
                                                            }
                                                            
                                                            console.log(`Nepoznata greska ${r.status}`)
                                                            alert('Nepoznata greska prilikom brisanja grupe!')
                                                        })
                                                    }}>Ukloni grupu</button>
                                                }
                                            </div>
                                            {
                                                podgrupeProizvoda == null ? <div>ucitavanje podgrupa...</div> :
                                                    <div>
                                                        {
                                                            podgrupeProizvoda.filter(x => x.grupaid == gp.id).map((pgp) => {
                                                                return (
                                                                    <div key={`2fafa2rq-${pgp.id}`}>
                                                                        <span>{pgp.naziv}</span>
                                                                        <button onClick={(e) => {
                                                                            const btnEl = e.currentTarget
                                                                            btnEl.setAttribute('disabled', true)
                                                                            
                                                                            apiFetch('/webshop/proizvod/podgrupa/delete', 'delete', 'application/json', JSON.stringify({ id: pgp.id }))
                                                                            .then(r => {
                                                                                if(r.status == 200) {
                                                                                    ucitajPodgrupeProizvoda().then(() => {
                                                                                        btnEl.removeAttribute('disabled')
                                                                                    })
                                                                                } else {
                                                                                    if(r.status == 204) {
                                                                                        alert('Greska 204!')
                                                                                    } else if (r.status == 400) {
                                                                                        alert('Greska 400')
                                                                                        r.text().then(r => console.log(r))
                                                                                    } else {
                                                                                        alert(`Nepoznata greska ${r.status}!`)
                                                                                    }
                                                                                    btnEl.removeAttribute('disabled')
                                                                                }
                                                                            })
                                                                        }}>ukloni</button>
                                                                    </div>
                                                                )
                                                            })
                                                        }
                                                        <div>
                                                            <input type='text' placeholder='Nova podgrupa naziv' />
                                                            <button onClick={(e) => {
                                                                const btnEl = e.currentTarget
                                                                const inputEl = e.currentTarget.parentElement.querySelector('input')
                                                                btnEl.setAttribute('disabled', true)
                                                                inputEl.setAttribute('disabled', true)
                                                                apiFetch('/webshop/proizvod/podgrupa/insert', 'post', 'application/json', JSON.stringify({
                                                                grupaid: gp.id,
                                                                naziv: inputEl.value,
                                                                displayIndex: 0
                                                                }))
                                                                .then(r => {
                                                                    if(r.status == 201) {
                                                                        ucitajPodgrupeProizvoda()
                                                                        .then(() => {
                                                                            btnEl.removeAttribute('disabled')
                                                                            inputEl.removeAttribute('disabled')
                                                                        })
                                                                    } else {
                                                                        if (r.status == 400) {
                                                                            alert("Greska 400!")
                                                                            console.log('Greska 400')
                                                                            r.text().then(r => console.log(r))
                                                                        } else {
                                                                            console.log(`Nepoznata greska ${r.status}`)
                                                                            alert('Greska prilikom kreiranja podgrupe!')
                                                                        }
                                                                        btnEl.removeAttribute('disabled')
                                                                        inputEl.removeAttribute('disabled')
                                                                    }
                                                                })
                                                            }}>Kreiraj podgrupu</button>
                                                        </div>
                                                    </div>
                                            }
                                        </div>
                                    )
                                })
                            }
                            <div className={styles.novaGrupaWrapper}>
                                <input type='text' placeholder='Nova grupa naziv' />
                                <button onClick={(e) => {
                                    const nazivInput = e.currentTarget.parentElement.querySelector('input')

                                    apiFetch('/webshop/proizvod/grupa/insert', 'post', 'application/json', JSON.stringify({
                                        naziv: nazivInput.value,
                                        displayIndex: 0
                                    }))
                                    .then(r => {
                                        if(r.status == 201) {
                                            ucitajGrupeProizvoda()
                                        } else {
                                            console.log('greska')
                                        }
                                    })
                                    .catch(r => {
                                        console.log(r)
                                    })
                                }}>Kreiraj grupu</button>
                            </div>
                        </div>
                }
            </div>
        </div>
    )
}