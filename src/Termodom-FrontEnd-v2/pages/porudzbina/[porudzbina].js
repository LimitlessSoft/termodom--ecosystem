import Head from "next/head"
import { useRouter } from "next/router"
import { useContext, useEffect, useRef, useState } from "react"
import { apiFetch } from "../../api"
import Header from "../../components/Header"
import styles from './porudzbina.module.css'
import moment from 'moment'
import Link from "next/link"
import ProdajnaMesta from "../../models/ProdajnaMesta"
import StatusiPorudzbine from '../../models/StatusiPorudzbine'
import NaciniPlacanja from '../../models/NaciniPlacanja'
import { KorisnikContext } from "../../KorisnikContext"
import { MessageBoxContext } from "../../MessageBoxContext"

function PorudzbinaHeader(props) {

    const korisnikContext = useContext(KorisnikContext)
    const porudzbina = props.porudzbina
    const [korisnikPorudzbine, setKorisnikPorudzbine] = useState(null)
    const messageBox = useContext(MessageBoxContext)

    useEffect(() => {
        if(porudzbina == null) {
            setKorisnikPorudzbine(null)
            return
        }

        if(porudzbina.korisnik == 'jednokratni')
        {
            setKorisnikPorudzbine({ naziv: 'Jednokratni Kupac' })
        } else {
            apiFetch('/api/korisnik/get?naziv=' + porudzbina.korisnik, 'get', null, null)
                .then(r => {
                    if(r.status == 200) {
                        r.json().then(r =>
                            {
                                setKorisnikPorudzbine(r)
                            })
                    }
                })
        }
    }, [porudzbina])

    function AdminHeader() {
        return (
            <div>
                <div className={styles.headerWrapper}>
                    <div>
                        <div>Web Broj: {porudzbina.id}</div>
                        <div>TD Broj: {porudzbina.br_dok_komercijalno?.toString()}</div>
                    </div>
                    <div>
                        <div>
                            <select defaultValue={porudzbina.magacin_id} onChange={(event) => {
                                apiFetch('/webshop/porudzbina/magacin-id/set', 'post', 'application/json', JSON.stringify({ porudzbinaId: porudzbina.id, magacinId: event.target.value }))
                                    .then(r => {
                                        if(r.status == 200) {
                                            porudzbina.magacin_id = event.target.value
                                            messageBox.show('Mesto preuzimanja izmenjeno!')
                                        } else {
                                            messageBox.show('Doslo je do greske prilikom izmene mesta preuzimanja!')
                                        }
                                    })
                            }}>
                                {
                                    ProdajnaMesta.map((pm) => {
                                        return (
                                            <option key={'porudzbina-prodajno-mesto-option-' + pm.id} value={pm.id}>{pm.naziv} - {pm.mesto}</option>
                                        )
                                    })
                                }
                            </select>
                        </div>
                        <div>
                            <select defaultValue={porudzbina.status} onChange={(event) => {
                                apiFetch('/webshop/porudzbina/status/set', 'post', 'application/json', JSON.stringify({ porudzbinaId: porudzbina.id, status: event.target.value }))
                                    .then(r => {
                                        if(r.status == 200) {
                                            porudzbina.status = event.target.value
                                            messageBox.show('Status izmenjen!')
                                        } else {
                                            messageBox.show('Doslo je do greske prilikom izmene statusa!')
                                        }
                                    })
                            }}>
                                {
                                    StatusiPorudzbine.map((sp) => {
                                        return (
                                            <option key={'porudzbina-status-' + sp.id} value={sp.id}>{sp.opis}</option>
                                        )
                                    })
                                }
                            </select>
                        </div>
                        <div>
                            <select defaultValue={porudzbina.nacin_placanja} onChange={(event) => {
                                apiFetch('/webshop/porudzbina/nacin-placanja/set', 'post', 'application/json', JSON.stringify({ porudzbinaId: porudzbina.id, nacinPlacanja: event.target.value }))
                                    .then(r => {
                                        if(r.status == 200) {
                                            porudzbina.nacin_placanja = event.target.value
                                            messageBox.show('Nacin placanja izmenjen!')
                                        } else {
                                            messageBox.show('Doslo je do greske prilikom izmene nacina placanja!')
                                        }
                                    })
                            }}>
                                {
                                    NaciniPlacanja.map((np) => {
                                        return (
                                            <option key={'porudzbina-nacin-placanja-' + np.id} value={np.id}>{np.opis}</option>
                                        )
                                    })
                                }
                            </select>
                        </div>
                    </div>
                    <div>
                        <div>Datum: {moment(porudzbina.datum).format('DD.MM.yyyy HH:mm')}</div>
                        {
                            korisnikPorudzbine != null &&
                            <Link href={'/korisnik/' + korisnikPorudzbine.naziv}>
                                <div>
                                    Korisnik: {korisnikPorudzbine.nadimak}
                                </div>
                            </Link>
                        }
                    </div>
                </div>
                {
                    porudzbina.referent_obrade != null && porudzbina.referent_obrade == korisnikContext.naziv &&
                    <div className={styles.headerAdminKomande}>
                        <button>Razvezi od proračuna</button>
                        <button>Obavesti radnju</button>
                        <button>Obavesti korisnika</button>
                        <button>Pošalji sms</button>
                    </div>
                }
                <ReferentLabel porudzbina={porudzbina} />
            </div>
        )
    }

    function NormalHeader() {
        var prodajnoMesto = ProdajnaMesta.find(x => x.id == porudzbina.magacin_id)
        var statusPorudzbine = StatusiPorudzbine.find(x => x.id == porudzbina.status)
        var nacinPlacanja = NaciniPlacanja.find(x => x.id == porudzbina.nacin_placanja)

        return (
            <div>
                <div className={styles.headerWrapper}>
                    <div>
                        <div>Web Broj: {porudzbina.id}</div>
                        <div>TD Broj: {porudzbina.br_dok_komercijalno?.toString()}</div>
                    </div>
                    <div>
                        <div>
                            <span>Mesto preuzimanja: </span>
                            {
                                prodajnoMesto == null ?
                                    'greska' :
                                    prodajnoMesto.naziv + ' - ' + prodajnoMesto.mesto
                            }
                        </div>
                        <div>
                            <span>Status porudzbine: </span>
                            {
                                statusPorudzbine == null ?
                                    'greska' :
                                    statusPorudzbine.opis
                            }
                        </div>
                        <div>
                            <span>Nacin placanja: </span>
                            {
                                nacinPlacanja == null ?
                                    'greska' :
                                    nacinPlacanja.opis
                            }
                        </div>
                    </div>
                    <div>
                        <div>Datum: {moment(porudzbina.datum).format('DD.MM.yyyy HH:mm')}</div>
                        {
                            korisnikPorudzbine != null &&
                            <Link href={'/korisnik/' + korisnikPorudzbine.naziv}>
                                <div>
                                    Korisnik: {korisnikPorudzbine.nadimak}
                                </div>
                            </Link>
                        }
                    </div>
                </div>
            </div>
        )
    }

    function ReferentLabel(props) {

        const porudzbina = props.porudzbina
        
        return (
            <div className={styles.referentLabel}>
            <div>
                {
                    porudzbina.referent_obrade == null ? 
                        <span>Porudzbina jos uvek nije preuzeta na obradu <button onClick={() => {
                            apiFetch('/webshop/porudzbina/preuzmi-na-obradu','post', 'application/json', JSON.stringify({
                                    korisnik: korisnikContext.value.naziv,
                                    porudzbinaId: porudzbina.id }))
                                .then(r => {
                                    if(r.status == 200) {
                                        props.ucitajPorudzbinu(porudzbina.id)
                                    }
                                })
                        }}>Preuzmi je</button></span> :
                        <span>Referent obrade ove porudzbine je: { porudzbina.referent_obrade }</span>
                }
            </div>
        </div>
        )
    }

    return (
        korisnikContext == null || korisnikContext.value == null ?
            <div>Prikazi ako je jednokratni</div> :
            (
                porudzbina.referent_obrade != null && porudzbina.referent_obrade == korisnikContext.value.naziv ?
                    <AdminHeader /> :
                    <NormalHeader />
            )
    )
}
function ProizvodiPorudzbine(props) {

    const korisnikContext = useContext(KorisnikContext)
    const porudzbina = props.porudzbina
    const stavkePorudzbine = props.stavkePorudzbine
    const proizvodi = props.proizvodi
    const roba = props.roba

    return (
        <div className={styles.proizvodiPorudzbineWrapper}>
            <div className={styles.proizvodiPorudzbineItem + ' ' + styles.porizvodiPoruzbineHeaderCol}>
                <div className={styles.nazivCol}><span>Proizvod</span></div>
                <div className={styles.kolicinaCol}><span>Količina</span></div>
                <div className={styles.cenaCol}><span>Cena sa PDV</span></div>
                <div className={styles.vrednostCol}><span>Vrednost sa PDV</span></div>
                <div className={styles.rabatCol}><span>Rabat</span></div>
                {
                    porudzbina.referent_obrade == null && porudzbina.referent_obrade == korisnikContext.naziv &&
                    <div className={styles.akcijaCol}><span>Akcija</span></div>
                }
            </div>
            {
                stavkePorudzbine?.map((stavka) => {
                    var r = roba?.find(x => x.robaid == stavka.roba_id)
                    var p = proizvodi?.find(x => x.robaid == stavka.roba_id)
                    var cenaSaPDV = (parseFloat(stavka.vp_cena) * (1 - (parseFloat(stavka.rabat) / 100))) * ((100 + parseFloat(p.pdv)) / 100)
                    return (
                        <div key={'stavka-porudzbine-id-' + stavka.id} className={styles.proizvodiPorudzbineItem}>
                            <div className={styles.nazivCol}><span>{ r == null ? 'Undefined' : r.naziv }</span></div>
                            <div className={styles.kolicinaCol}><span>{ parseFloat(stavka.kolicina).toFixed(2) } { r == null ? 'undefined' : r.jm }</span></div>
                            <div className={styles.cenaCol}><span>{ cenaSaPDV.toFixed(2) }</span></div>
                            <div className={styles.vrednostCol}><span>{ (cenaSaPDV * stavka.kolicina).toFixed(2) }</span></div>
                            <div className={styles.rabatCol}><span>{ parseFloat(stavka.rabat).toFixed(2) }%</span></div>
                            {
                                porudzbina.referent_obrade != null && porudzbina.referent_obrade == korisnikContext.naziv &&
                                <div className={styles.akcijaCol}><button>ukloni</button></div>
                            }
                        </div>
                    )
                })
            }
        </div>
    )
}

function SummaruPorudzbine(props) {
    const stavkePorudzbine = props.stavkePorudzbine
    const roba = props.roba
    const proizvodi = props.proizvodi

    var vrednostBezPopustaSaPDV = 0
    var osnovica = 0
    var PDV = 0

    stavkePorudzbine?.map((stavka) => {
        var r = roba?.find(x => x.robaid == stavka.roba_id)
        var p = proizvodi?.find(x => x.robaid == stavka.roba_id)
        var cenaBezPDV = (parseFloat(stavka.vp_cena) * (1 - (parseFloat(stavka.rabat) / 100)))
        var cenaSaPDV = cenaBezPDV * ((100 + parseFloat(p.pdv)) / 100)
        vrednostBezPopustaSaPDV += parseFloat(stavka.vp_cena) * ((100 + parseFloat(p.pdv)) / 100) * stavka.kolicina
        osnovica += parseFloat(stavka.kolicina * cenaBezPDV)
        PDV += parseFloat(stavka.kolicina * cenaBezPDV * parseFloat(p.pdv) / 100)
    })

    return (
        <div className={styles.summWrapper}>
            <div className={styles.summInner}>
                <div>Osnovica: { osnovica.toFixed(2) } RSD</div>
                <div>PDV: { PDV.toFixed(2) } RSD</div>
                <div>Za uplatu: { (osnovica + PDV).toFixed(2)} RSD</div>
                <div>Popust: { vrednostBezPopustaSaPDV.toFixed(2)} RSD</div>
            </div>
        </div>
    )
}

export default function Porudzbina() {
    const route = useRouter()
    
    const [porudzbina, setPorudzbina] = useState(null)
    const [stavkePorudzbine, setStavkePorudzbine] = useState(null)
    const [roba, setRoba] = useState(null)
    const [proizvodi, setProizvodi] = useState(null)

    useEffect(() => {
        apiFetch('/api/roba/list', 'get', null, null)
            .then(r => {
                if(r.status == 200) {
                    r.json().then(r => {
                            setRoba(r)
                        })
                }
            })
        apiFetch('/webshop/proizvod/list', 'get', null, null)
            .then(r => {
                if(r.status == 200) {
                    r.json().then(r => {
                            setProizvodi(r)
                        })
                }
            })
    }, [])

    const ucitajPorudzbinu = (id) => {
        apiFetch('/webshop/porudzbina/get?id=' + id, 'get', null, null)
        .then(r => {
            if(r.status == 200)
            {
                r.json().then(r => {
                    setPorudzbina(r)
                })
            }
        })
    }

    useEffect(() => {
        if(route == null || route.query == null || route.query.porudzbina == null) {
            return
        }
        ucitajPorudzbinu(route.query.porudzbina)
    }, [route])

    useEffect(() => {
        if(porudzbina == null) {
            setStavkePorudzbine(null)
            return
        }

        apiFetch('/webshop/porudzbina/stavka/list?porudzbinaId=' + porudzbina.id, 'get', null, null)
            .then(r => {
                if(r.status == 200) {
                    r.json().then(r => {
                        setStavkePorudzbine(r)
                    })
                }
            })
    }, [porudzbina])

    return (
        <div>
            <Head>
                <title>Termodom - centar građevinskog materijala</title>
                <meta name="description" content="Termodom - centar građevinskog materijala" />
                <link rel="icon" href="/favicon.ico" />
            </Head>

            <Header />
            <div style={{width: '500px', wordBreak: 'break-all'}}>
            </div>
            {
                porudzbina != null &&
                <div className={styles.mainWrapper}>
                    <PorudzbinaHeader porudzbina={porudzbina} ucitajPorudzbinuFunc={ucitajPorudzbinu} />
                    <ProizvodiPorudzbine porudzbina={porudzbina} stavkePorudzbine={stavkePorudzbine} roba={roba} proizvodi={proizvodi} />
                    <SummaruPorudzbine stavkePorudzbine={stavkePorudzbine} roba={roba} proizvodi={proizvodi} />
                </div>
            }
        </div>
    )
}