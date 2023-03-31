import Head from 'next/head'
import Header from '../components/Header'
import styles from './korpa.module.css'
import CookieKorpa from '../CookieKorpa'
import Link from 'next/link'
import { useContext, useEffect, useState } from 'react'
import { apiFetch } from '../api'
import { KorisnikContext } from '../KorisnikContext'
import NumberFormat from 'react-number-format'

export default function Korpa() {
    const korisnikContext = useContext(KorisnikContext)
    const [ironCenovnik, setIronCenovnik] = useState(null)
    const [proizvodi, setProizvodi] = useState(null)
    const [korpaItems, setKorpaItems] = useState(null)
    const [cenovnik, setCenovnik] = useState(null)
    const [isJednokratni, setIsJednokratni] = useState(null)
    const [ukupno, setUkupno] = useState(0)
    const [PDV, setPDV] = useState(0)
    const [zaUplatu, setZaUplatu] = useState(0)
    const [usteda, setUstead] = useState(0)
    const [isporuka, setIsporuka] = useState(false)

    useEffect(() => {
        apiFetch('/webshop/cenovnik/get?nivo=0', 'GET', null, null)
            .then(r => {
                if(r.status == 200) {
                    r.json().then(r => setIronCenovnik(r))
                } else if(r.status == 400) {
                    r.text().then(r => console.log(r))
                } else {
                    console.log("Sistemska greska!")
                }
            })
        apiFetch('/api/roba/list', 'GET', null, null).then(r => {
            if(r.status == 200) {
                r.json().then(x => {
                    setProizvodi(x)
                })
            }
        })
        setKorpaItems(CookieKorpa.get())
    }, [])

    useEffect(() => {
        if(korisnikContext.value != null && korisnikContext.value.naziv != null) {
            if(korisnikContext.value.naziv == 'jednokratni') {
                setIsJednokratni(true)
            } else {
                setIsJednokratni(false)
            }
            apiFetch('/webshop/cenovnik/get?korisnik=' + korisnikContext.value.naziv, 'GET', null, null)
                .then((r) => {
                    if(r.status == 200) {
                        r.json().then(r => setCenovnik(r))
                    }
                })
        }
    }, [korisnikContext?.value])

    useEffect(() => {
        if(ironCenovnik == null || cenovnik == null) {
            return
        }
        var ukupno = 0
        var PDV = 0
        var usteda = 0

        korpaItems?.map((ci) => {
            var ironCena = ironCenovnik?.find(x => x.robaid == ci.robaid)
            var cena = cenovnik?.find(x => x.robaid == ci.robaid)
            var pdv = cena.prodajna_cena_bez_pdv * cena.pdv / 100
            var ust = ironCena.prodajna_cena_bez_pdv - cena.prodajna_cena_bez_pdv
            ust *= ci.kolicina
            ust *= ((100 + cena.pdv) / 100)

            usteda += ust
            ukupno += cena.prodajna_cena_bez_pdv * ci.kolicina
            PDV += pdv * ci.kolicina
        })

        setUkupno(ukupno)
        setPDV(PDV)
        setZaUplatu((ukupno + PDV))
        setUstead(usteda)
    }, [cenovnik, ironCenovnik])

    return (
        <div>
            <Head>
                <title>Termodom - centar građevinskog materijala</title>
                <meta name="description" content="Termodom - centar građevinskog materijala" />
                <link rel="icon" href="/favicon.ico" />
            </Head>
    
            <Header />

            <div className={styles.wrapper}>
                <div className={styles.actionBar}>
                    <Link href='/'>Nastavi Kupovinu</Link>
                </div>
                <div className={styles.table}>
                    {
                        korpaItems == null ?
                            null :
                            <table>
                                <tbody>
                                    <tr>
                                        <th>Proizvod</th>
                                        <th>Kolicina</th>
                                        <th>Cena Sa PDV</th>
                                        <th>Vrednost Sa PDV</th>
                                        <th></th>
                                    </tr>
                                    {
                                        korpaItems.map((ci, i) => {
                                            var proizvod = proizvodi?.find(x => x.robaid == ci.robaid)
                                            var cena = cenovnik?.find(x => x.robaid == ci.robaid)
                                            var cenaSaPDV = cena == null ? 0 : cena.prodajna_cena_bez_pdv * ((100 + cena.pdv) / 100)
                                            return (
                                                <tr key={'12f1qwaf-' + i}>
                                                    <td className={styles.proizvodColumn}>{proizvod == null ? 'undefined' : proizvod.naziv}</td>
                                                    <td>
                                                        {ci.kolicina + ' ' + proizvod?.jm}
                                                        <button className={styles.tableButton}>izmeni</button>
                                                    </td>
                                                    <td>
                                                        <NumberFormat 
                                                            value={cenaSaPDV.toFixed(2)}
                                                            displayType={'text'}
                                                            thousandSeparator={true} />
                                                    </td>
                                                    <td>
                                                        <NumberFormat 
                                                            value={(cenaSaPDV * ci.kolicina).toFixed(2)}
                                                            displayType={'text'}
                                                            thousandSeparator={true} />
                                                    </td>
                                                    <td><button className={styles.tableButton}>ukloni</button></td>
                                                </tr>
                                            )
                                        }
                                    )}
                                </tbody>
                            </table>
                    }
                </div>
                <div className={styles.summWrapper}>
                    <div className={styles.summUkupno}>
                        <div>Ukupno:
                            <span>
                                <NumberFormat 
                                    value={ukupno.toFixed(2)}
                                    displayType={'text'}
                                    thousandSeparator={true} />
                            </span>
                        </div>
                    </div>
                    <div className={styles.summPDV}>
                        <div>PDV:
                            <span>
                                <NumberFormat 
                                    value={PDV.toFixed(2)}
                                    displayType={'text'}
                                    thousandSeparator={true} />
                            </span>
                        </div>
                    </div>
                    <div className={styles.summZaUplatu}>
                        <div>Za uplatu:
                            <span>
                                <NumberFormat 
                                    value={zaUplatu.toFixed(2)}
                                    displayType={'text'}
                                    thousandSeparator={true} />
                            </span>
                        </div>
                    </div>
                    <div className={styles.summUsteda}>
                        <div>Ušteda:
                            <span>
                                <NumberFormat 
                                    value={usteda.toFixed(2)}
                                    displayType={'text'}
                                    thousandSeparator={true} />
                            </span>
                        </div>
                    </div>
                </div>
                {
                    isJednokratni == null ?
                        null :
                        <div className={styles.lockWrapper}>
                            <div className={styles.lockItem}>
                                <div>Mesto preuzimanja:</div>
                                <select id='magacin-id-select-input' onChange={(event) => {
                                        var nacinUplateEl = document.getElementById('nacin-placanja-select-input')
                                        if(event.target.value == -5) {
                                            nacinUplateEl.querySelector("option[value='3']").style.display = 'initial'
                                            nacinUplateEl.value = 3
                                            setIsporuka(true)
                                        } else {
                                            nacinUplateEl.querySelector("option[value='3']").style.display = 'none'
                                            nacinUplateEl.value = 0
                                            setIsporuka(false)
                                        }
                                    }}
                                    defaultValue={12}>
                                    <option value="12">Beograd - Smederevski put 14 ( VML )</option>
                                    <option value="13">Beograd - Zrenjaninski put 84g ( Kotež )</option>
                                    <option value="28">Beograd - Batajnički drum BB ( Između BN Boss-a i Coca Cole )</option>
                                    <option value="26">Beograd - Kružni put BB - ( Resnik )</option>
                                    <option value="17">Niš - Brzi Brod (Niš)</option>
                                    <option value="15">Pančevo</option>
                                    <option value="16">Novi Sad</option>
                                    <option value="18">Čačak</option>
                                    <option value="19">Požarevac</option>
                                    <option value="21">Jagodina</option>
                                    <option value="20">Subotica</option>
                                    <option value="22">Šabac</option>
                                    <option value="23">Smederevo</option>
                                    <option value="25">Kragujevac</option>
                                    <option value="27">Sremska Mitrovica</option>
                                    <option value="-5">Dostava</option>
                                </select>
                            </div>
                            {
                                isporuka ? 
                                    <div className={styles.lockItem}>
                                        <div>Adresa Isporuke:</div>
                                        <input id='adresa-isporuke-input' type='text' />
                                    </div> :
                                    null
                            }
                            {
                                !isJednokratni ?
                                    null :
                                    <div className={styles.lockItem}>
                                        <div>Ime i prezime:</div>
                                        <input id='ime-i-prezime-input' type='text' />
                                    </div>
                            }
                            {
                                !isJednokratni ?
                                    null :
                                    <div className={styles.lockItem}>
                                        <div>Mobilni:</div>
                                        <input id='kontakt-mobilni-input' type='text' />
                                    </div>
                            }
                            <div className={styles.lockItem}>
                                <div>Napomena:</div>
                                <input id='napomena-input' type='text' />
                            </div>
                            <div className={styles.lockItem}>
                                <div>Nacin plaćanja:</div>
                                <select id='nacin-placanja-select-input' defaultValue={0}>
                                    <option value="0">Gotovinom na licu mesta</option>
                                    <option value="1">Virmanom</option>
                                    <option value="2">Uplatnicom</option>
                                    <option value="3" style={{ display: 'none' }}>Po vozacu</option>
                                </select>
                            </div>
                            <div className={styles.lockItem + ' ' + styles.lockAnnotation}>Cene iz porudžbine važe 1 dan od dana zaključivanja!</div>
                            <button className={styles.lockItem} onClick={() => {
                                var korisnik = korisnikContext?.value?.naziv
                                var magacinID = document.getElementById('magacin-id-select-input').value
                                var nacinPlacanja = document.getElementById('nacin-placanja-select-input').value
                                var napomena = document.getElementById('napomena-input').value
                                var adresaIsporuke = isporuka ? document.getElementById('adresa-isporuke-input').value : null
                                var kontaktMobilni = korisnik == 'jednokratni' ? document.getElementById('kontakt-mobilni-input').value : korisnikContext?.value?.mobilni
                                var imeIPrezime = korisnik == 'jednokratni' ? document.getElementById('ime-i-prezime-input').value : '[ profi ] ' + korisnikContext?.value?.nadimak

                                var obj = {
                                    korisnik: korisnik,
                                    magacinID: magacinID,
                                    ppid: null,
                                    nacinPlacanja: nacinPlacanja,
                                    ustedaKorisnik: usteda,
                                    ustedaKlijent: 0,
                                    napomena: napomena,
                                    adresaIsporuke: adresaIsporuke,
                                    kontaktMobilni: kontaktMobilni,
                                    imeIPrezime: imeIPrezime,
                                    stavke: JSON.stringify(CookieKorpa.get())
                                }

                                if(obj.korisnik == null) {
                                    alert("Sistemska greska!")
                                    return
                                }

                                apiFetch('/webshop/porudzbina/zakljuci', 'POST', 'application/json', JSON.stringify(obj)).then(r => {
                                    console.log(r)
                                })
                            }}>Zaključi porudžbinu</button>
                        </div>
                }
            </div>
        </div>
    )
}