import Head from 'next/head'
import { useContext, useEffect, useState } from 'react'
import Header from '../components/Header'
import styles from './index.module.css'
import { apiFetch, IMAGE_BASE_URL } from '../api'
import Link from 'next/link'
import ProductCards from '../components/ProductCards'
import { KorisnikContext } from '../KorisnikContext'

export default function Home() {
  const [products, setProducts] = useState(null)
  const [cenovnik, setCenovnik] = useState(null)

  const korisnikContext = useContext(KorisnikContext)

  useEffect(() => {
  }, [])

  useEffect(() => {
    loadProducts()
  }, [korisnikContext.value])

  function loadProducts() {
    return new Promise(() => {
      const products = apiFetch('/products', 'GET', null, null)

      if(korisnikContext == null ||
          korisnikContext.value == null ||
          korisnikContext.value.naziv == null ||
          korisnikContext?.value.naziv == 'jednokratni') {
        setCenovnik([])
      } else {
        apiFetch('/webshop/cenovnik/get?korisnik=' + korisnikContext?.value?.naziv, 'GET', null, null).then(r => {
          console.log(r)
          if(r.status == 200) {
            r.json().then(r => {
              setCenovnik(r)
            })
          }
        })
      }

      products.then(response => {
        if(response.status != 200) {
          console.log("Error fetching /products")
          return
        }

        if(response.status == 200) {
          response.json().then(r => {
            const finalArr = []
            r.body.map(prod => {
              if(prod.active == false) {
                return
              }
              finalArr.push(prod)
            })
            finalArr.sort((a, b) => b.displayIndex - a.displayIndex )
            setProducts(finalArr)
          })
        }
      })
    })
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
          <ProductCards products={products} cenovnik={cenovnik} />
      </div>
    </div>
  )
}
