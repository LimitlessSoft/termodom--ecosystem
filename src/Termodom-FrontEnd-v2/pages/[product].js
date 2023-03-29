import Head from 'next/head'
import Header from '../components/Header'
import styles from './product.module.css'
import { useRouter } from 'next/router'
import { useContext, useEffect, useRef, useState } from 'react'
import { apiFetch, IMAGE_BASE_URL } from '../api'
import Link from 'next/link'
import ProductCards from '../components/ProductCards'
import CookieKorpa from '../CookieKorpa'
import { KorisnikContext } from '../KorisnikContext'

export default function Product(props) {
    const route = useRouter()

    const [product, setProduct] = useState(null)
    const [relatedProducts, setRelatedProducts] = useState(null)
    const inputKolicina = useRef(null)
    const inputKolicinaTP = useRef(1)
    const [proizvodJeVecUKorpi, setProizvodJeVecUKorpi] = useState(false)
    const [cenaProizvoda, setCenaProizvoda] = useState(null)
    const korisnikContext = useContext(KorisnikContext)

    useEffect(() => {
        loadProduct()
    }, [route])

    useEffect(() => {
        if(product == null || product.kupovina_samo_u_transportnom_pakovanju != 1) {
            return
        }
    }, [inputKolicina])

    useEffect(() => {
        if(CookieKorpa.get().find(x => x.robaid == product?.robaid) != null) {
            setProizvodJeVecUKorpi(true)
        } else {
            setProizvodJeVecUKorpi(false)
        }
        loadRelatedProducts()
        inputKolicina.current = product?.kupovina_samo_u_transportnom_pakovanju ? (product?.transportno_pakovanje_kolicina * 1).toFixed(2) : 1
        inputKolicinaTP.current = product?.kupovina_samo_u_transportnom_pakovanju ? (product?.transportno_pakovanje_kolicina * 1).toFixed(2) : 1
        document.getElementById('detailed-info-wrapper').innerHTML = product?.detaljan_opis
        if(product != null &&
            korisnikContext != null &&
            korisnikContext.value != null && 
            korisnikContext.value.naziv != null &&
            korisnikContext.value.naziv != 'jednokratni') {
            apiFetch(`/webshop/cenovnik/get?korisnik=${korisnikContext.value.naziv}&robaid=${product.robaid}`).then(r => {
                r.json().then(r => setCenaProizvoda(r))
            })
        }
    }, [product])

    function loadRelatedProducts() {
        return new Promise(() => {
          const j1 = apiFetch('/webshop/proizvod/related?limit=5', 'GET', null, null)
          const j2 = apiFetch('/api/roba/list', 'GET', null, null)
    
          Promise.all([j1, j2])
    
          j1.then(response => {
            if(response.status == 200) {
              response.json().then((fetchedProducts) => 
              {
                j2.then(response => {
                  if(response.status == 200) {
                    response.json().then((fetchedRoba) => {
                      const finalArr = []
                      fetchedProducts.map((prod) => {
                        if(prod.aktivan != 1) {
                          return
                        }
                        const r = fetchedRoba.find(o => o.robaid == prod.robaid)
                        finalArr.push({ ...r, ...prod })
                      })
                      finalArr.sort((a, b) => b.display_index - a.display_index )
                      setRelatedProducts(finalArr)
                    })
                  } else {
                    console.log('Error loading /api/roba/list')
                  }
                })
              })
            } else {
              console.log('Error loading /webshop/proizvod/list!')
            }
          })
        })
    }

    function loadProduct() {
        apiFetch('/webshop/proizvod/get?rel=' + route.query.product, 'GET', null, null)
            .then(response => {
                if(response.status == 200) {
                    response.json().then(r => {
                        apiFetch('/api/roba/get?robaid=' + r.robaid, 'GET', null, null)
                            .then(response1 => {
                                if(response1.status == 200) {
                                    response1.json().then(r1 => {
                                        setProduct({ ...r1, ...r})
                                    })
                                } else if (response1.status == 204) {
                                    console.log('Product Not Found!')
                                } else {
                                    console.log('API Error!')
                                }
                            })
                    })
                } else if (response.status == 204) {
                    console.log('Product Not Found!')
                } else {
                    console.log('API Error!')
                }
            })        
    }

    function validateInput(event) {
        if(event.keyCode == 8) {
            return
        }

        if(isNaN(event.key) && (event.currentTarget.value.toString().indexOf('.') >= 0 && event.key == '.')) {
            event.preventDefault()
        }

        var parts = event.currentTarget.value.toString().split('.')
        if(parts != null && parts.length > 1) {
            if(parts[1].length > 1) {
                event.preventDefault()
            }
        }
    }

    function KolicinaInput() {
        const k = product?.kupovina_samo_u_transportnom_pakovanju ? (product?.transportno_pakovanje_kolicina * 1).toFixed(2) : 1
        return (
            <div className={styles.kolicinaInputField}>
                <label>{product?.jm}</label>
                <div className={styles.kolicinaDownWrapper}>
                    <input
                        id='kolicina-standard-input'
                        onKeyUp={(event) => {
                            var newValue = event.currentTarget.value * 1
                            inputKolicina.current = newValue
                            if(product?.kupovina_samo_u_transportnom_pakovanju) {
                                document.getElementById('kolicina-tp-input').value = (newValue / (inputKolicinaTP.current * 1)).toFixed(2)
                            }
                        }}
                        onKeyDown={(event) => { validateInput(event) }}
                        type='text'
                        defaultValue={(inputKolicina.current * 1).toFixed(2)} />
                        <div className={styles.kolicinaButtons}>
                            <button onClick={() => {
                                var newValue = ((inputKolicina.current * 1) + (inputKolicinaTP.current * 1)).toFixed(2)
                                inputKolicina.current = newValue
                                document.getElementById('kolicina-standard-input').value = newValue
                                if(product?.kupovina_samo_u_transportnom_pakovanju) {
                                    document.getElementById('kolicina-tp-input').value = (newValue / (inputKolicinaTP.current * 1)).toFixed(2)
                                }
                            }}>+</button>
                            <button onClick={() => {
                                var newValue = ((inputKolicina.current * 1) - (inputKolicinaTP.current * 1)).toFixed(2)
                                if(newValue <= 0) {
                                    newValue = inputKolicinaTP.current
                                }
                                inputKolicina.current = newValue
                                document.getElementById('kolicina-standard-input').value = newValue
                                if(product?.kupovina_samo_u_transportnom_pakovanju) {
                                    document.getElementById('kolicina-tp-input').value = (newValue / (inputKolicinaTP.current * 1)).toFixed(2)
                                }
                            }}>-</button>
                        </div>
                </div>
            </div>
        )
    }

    function TransportnaKolicinaInput() {
        const k = product?.kupovina_samo_u_transportnom_pakovanju ? (product?.transportno_pakovanje_kolicina * 1).toFixed(2) : 1
        return (
            <div className={styles.kolicinaInputField}>
                <label>{product?.transportno_pakovanje_jm}</label>
                <div className={styles.kolicinaDownWrapper}>
                    <input
                        id='kolicina-tp-input'
                        onKeyUp={(event) => {
                            var newValue = event.currentTarget.value * 1
                            inputKolicina.current = newValue / inputKolicinaTP
                            if(product?.kupovina_samo_u_transportnom_pakovanju) {
                                document.getElementById('kolicina-standard-input').value = (newValue / (inputKolicinaTP.current * 1)).toFixed(2)
                            }
                        }}
                        onKeyDown={(event) => { validateInput(event) }}
                        type='text'
                        defaultValue={(1).toFixed(2)} />
                        <div className={styles.kolicinaButtons}>
                            <button onClick={() => {
                                var newValue = ((inputKolicina.current * 1) + (inputKolicinaTP.current * 1)).toFixed(2)
                                inputKolicina.current = newValue
                                document.getElementById('kolicina-standard-input').value = newValue
                                if(product?.kupovina_samo_u_transportnom_pakovanju) {
                                    document.getElementById('kolicina-tp-input').value = (newValue / (inputKolicinaTP.current * 1)).toFixed(2)
                                }
                            }}>+</button>
                            <button onClick={() => {
                                var newValue = ((inputKolicina.current * 1) - (inputKolicinaTP.current * 1)).toFixed(2)
                                if(newValue <= 0) {
                                    newValue = inputKolicinaTP.current
                                }
                                inputKolicina.current = newValue
                                document.getElementById('kolicina-standard-input').value = newValue
                                if(product?.kupovina_samo_u_transportnom_pakovanju) {
                                    document.getElementById('kolicina-tp-input').value = (newValue / (inputKolicinaTP.current * 1)).toFixed(2)
                                }
                            }}>-</button>
                        </div>
                </div>
            </div>
        )
    }

    function DodajUKorpu() {
        let kolicina = (inputKolicina.current * 1).toFixed(2)
        if(product?.kupovina_samo_u_transportnom_pakovanju == 1 &&
            ((kolicina * 1) % (product?.transportno_pakovanje_kolicina)) != 0) {
                alert('pogresna kolicina')
            }
        CookieKorpa.add(product?.robaid, kolicina)
        setProizvodJeVecUKorpi(true)
    }
    return (
        <div>
            <Head>
                <title>Termodom - centar građevinskog materijala</title>
                <meta name="description" content="Termodom - centar građevinskog materijala" />
                <link rel="icon" href="/favicon.ico" />
            </Head>
    
            <Header />

            <div className={styles.productWrapper}>
                <div className={styles.actionBar}>
                    <Link href='/'>
                        <div>Nazad</div>
                    </Link>
                </div>
                <div className={styles.modInfo}>
                    <div>
                        Trenutno se nalazite u modu jednokratne kupovine.
                        Cene zavise od ukupne vrednosti korpe.
                        Više o tome pročitajte ovde.
                    </div>
                </div>
                <div className={styles.productMainInfo}>
                    <div className={styles.productImageWrapper}>
                        <img src={IMAGE_BASE_URL + product?.slika} />
                    </div>
                    <div className={styles.centralInfo}>
                        <h1 className={styles.productTitle}>
                            { product?.naziv }
                        </h1>
                        <div className={styles.shortDescription}>
                            { product?.kratak_opis }
                        </div>
                        {
                            cenaProizvoda == null ?
                                null :
                                <div className={styles.korpaCene}>
                                    <div className={styles.korpaCenaItem}>
                                        <div>
                                            {
                                                cenaProizvoda == null ?
                                                    null :
                                                    cenaProizvoda[0].prodajna_cena_bez_pdv.toFixed(2)
                                                }
                                                <span>
                                                    RSD/{ product?.jm }
                                                </span>
                                        </div>
                                        <div>Cena bez PDV</div>
                                    </div>
                                    <div className={styles.korpaCenaItem}>
                                        <div>
                                            { cenaProizvoda == null ?
                                                null :
                                                (cenaProizvoda[0].prodajna_cena_bez_pdv * ((100 + cenaProizvoda[0].pdv) / 100))
                                                .toFixed(2)
                                            }
                                            <span>
                                                RSD/{ product?.jm }
                                            </span>
                                        </div>
                                        <div>Cena sa PDV</div>
                                    </div>
                                </div>
                        }
                        <div className={styles.korpaAkcije}>
                            <div className={styles.kolicinaInputWrapper}>
                                <KolicinaInput />
                                { product?.kupovina_samo_u_transportnom_pakovanju == 1 ? <TransportnaKolicinaInput /> : null }
                            </div>
                        </div>
                        {
                            proizvodJeVecUKorpi ? 
                                <div className={styles.proizvodJeVecUKorpi}>
                                    Proizvod je dodat u korpu!
                                </div> :
                                <button
                                    onClick={() => {
                                        DodajUKorpu()
                                    }}
                                    className={styles.dodajUKorpuButton}>
                                        Dodaj U Korpu
                                </button>

                        }
                        <div className={styles.systemInfo}>
                            <div className={styles.sysInfoItem}><label>Kataloski Broj:</label>{ product?.katbr }</div>
                            <div className={styles.sysInfoItem}><label>Podgrupe:</label>podgrupe</div>
                            <div className={styles.sysInfoItem}><label>JM:</label>{ product?.jm }</div>
                        </div>
                    </div>
                    <div className={styles.klasaWrapper}>
                        <img src={IMAGE_BASE_URL + '/images/Standard.svg'} />
                    </div>
                </div>
                <div id='detailed-info-wrapper' className={styles.detailedInfo}></div>
                <ProductCards products={relatedProducts} />
            </div>
        </div>
    )
}