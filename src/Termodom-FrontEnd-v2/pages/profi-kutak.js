import Head from "next/head";
import React, { useContext, useEffect, useState } from "react";
import Header from "../components/Header";
import styles from './profi-kutak.module.css'
import { KorisnikContext } from "../KorisnikContext";
import LogoLong from '../assets/includes/Logo_Long.png'
import { apiFetch } from "../api"
import Moment from 'moment'
import Cookies from 'universal-cookie'
import NumberFormat from 'react-number-format'
import Link from 'next/link'

const cookies = new Cookies();

export default function ProfiKutak() {

    const korisnikContext = useContext(KorisnikContext)
    useEffect(() => {

    })

    const Logovanje = () => {
        return (
            <div className={styles.logovanjeWrapper}>
                <div className={styles.logovanjeImageWrapper}>
                    <img src={LogoLong.src} />
                </div>
                <h2>PROFI KUTAK</h2>
                <input id='korisnicko-ime-input' type='text' placeholder="Korisnicko Ime" />
                <input id='sifra-input' type='password' placeholder="Lozinka" />
                <button className={styles.logovanjeButton} onClick={() => {
                    var username = document.getElementById('korisnicko-ime-input').value
                    var sifra = document.getElementById('sifra-input').value

                    var error_el = document.getElementById('error-text-label')

                    if(username == null || username.trim().length == 0) {
                        error_el.innerHTML = "Neispravno korisničko ime!"
                        error_el.style.display = 'block'
                        return
                    }

                    if(sifra == null || sifra.trim().length == 0) {
                        error_el.innerHTML = "Neispravna šifra!"
                        error_el.style.display = 'block'
                        return
                    }

                    apiFetch('/webshop/session/register', 'POST', 'application/json',
                    JSON.stringify({
                        username: username,
                        password: sifra
                    }))
                        .then((r) => {
                            if(r.status == 400) {
                                r.text().then((r) => {
                                    error_el.innerHTML = r
                                    error_el.style.display = 'block'
                                    return
                                })
                            } else if (r.status == 200) {
                                r.text().then((r) => {
                                    console.log(r)
                                    cookies.set('ARToken', r)
                                    apiFetch('/webshop/korisnik/get?naziv=' + username, 'GET', null, null)
                                      .then((r) => {
                                        if(r.status == 200) {
                                          r.json().then((r) => {
                                            korisnikContext?.set(r)
                                          })
                                        }
                                      })
                                    return
                                })
                            } else if(r.status == 403) {
                                error_el.innerHTML = "Pogresno korisnicko ime ili lozinka!"
                                error_el.style.display = 'block'
                                return
                            } else {
                                error_el.innerHTML = "Sistemska greška!"
                                error_el.style.display = 'block'
                                return
                            }
                        })
                        .catch((e) => {
                            error_el.innerHTML = "Sistemska greška!"
                            error_el.style.display = 'block'
                            return
                        })
                }}>Logovanje</button>
                <label id='error-text-label' className={styles.errorTextLabel}></label>
                <button className={styles.prebaciSeNaJednokratnuButton}>Prebaci se na jednokratnu kupovinu</button>
                <button className={styles.postaniProfiKupacButton}>Postani Profi Kupac</button>
            </div>
        )
    }

    const AdminAkcije = () => {
        return (
            <div className={styles.adminAkcijeWrapper}>
                <div className={styles.actionBar}>
                    <Link href='/panel/proizvodi'>Upravljaj proizvodima</Link>
                    <Link href='/panel/proizvodi/podgrupe'>Upravljaj grupama i podgrupama proizvoda</Link>
                </div>
            </div>
        )
    }

    const PregledNaloga = () => {
        const [porudzbineKorisnika, setPorudzbineKorisnika] = useState(null)
        useEffect(() => {
            if(korisnikContext == null || korisnikContext.value == null) {
                return
            }

            apiFetch('/webshop/porudzbina/list?korisnik=' + korisnikContext.value.naziv, 'GET', null, null)
                .then(r => {
                    if(r.status == 200) {
                        r.json().then(r => setPorudzbineKorisnika(r))
                    }
                })
        }, [korisnikContext])
        return (
            <div>
                {
                    korisnikContext?.value ?
                    <div>
                        <div className={styles.actionBar}>
                            <div className={styles.zdravoPoruka}>Zdravo {korisnikContext.value.nadimak}</div>
                            <div>
                                <button onClick={() => {
                                    cookies.remove('ARToken')
                                    window.location.reload()
                                }}>Izloguj se</button>
                            </div>
                        </div>
                        {
                            korisnikContext.value.tip == 1 ?
                            <AdminAkcije /> :
                            null
                        }
                        <div>
                            {
                                porudzbineKorisnika?.length > 0 ?
                                <div className={styles.porudzbinaListWrapper}>
                                    <div className={styles.porudzbinaListTitle}>Ovo su tvoje dosadašnje porudžbine!</div>
                                    {
                                        porudzbineKorisnika?.map((p) => {
                                            return (
                                                <Link href={'/porudzbina/' + p.id} key={'porudzbina-id-' + p.id}>
                                                    <div className={styles.porudzbinaItem}>
                                                        <div>Web Broj: {p.id}</div>
                                                        {
                                                            p.br_dok_komercijalno == null ?
                                                            <div>Porudzbina jos uvek nije obradjena</div> :
                                                            <div>Broj Za Radnju: {p.br_dok_komercijalno}</div>
                                                        }
                                                        <div>{Moment(p.datum).format('DD.MM.YYYY HH:mm:ss')}</div>
                                                        <div>
                                                            <NumberFormat 
                                                                className={styles.porudzbinaItemUsteda}
                                                                prefix="Ušteda: "
                                                                value={parseFloat(p.usteda_korisnik).toFixed(2)}
                                                                displayType={'text'}
                                                                thousandSeparator={true} />
                                                        </div>
                                                    </div>
                                                </Link>
                                            )
                                        })
                                    }
                                </div> :
                                <div>
                                    <div>Za sada nemate ni jednu porudzbinu!</div>
                                </div>
                            }
                        </div>
                    </div>
                    : null
                }
            </div>
        )
    }

    return (
        <div>
            <Head>
                <title>Termodom - centar građevinskog materijala</title>
                <meta name="description" content="Termodom - centar građevinskog materijala" />
                <link rel="icon" href="/favicon.ico" />
            </Head>

            <Header />

            <div className={styles.wrapper}>
                { korisnikContext == null ||
                    korisnikContext.value == null ||
                    korisnikContext.value.naziv == null ||
                    korisnikContext.value.naziv == 'jednokratni' ?
                        <Logovanje /> : <PregledNaloga /> }
            </div>
        </div>
    )
}