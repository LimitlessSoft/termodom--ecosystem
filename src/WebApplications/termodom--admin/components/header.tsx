import Image from 'next/image'
import logoSvg from '../src/assets/images/Termodom_Logo_White.svg'
import styles from './header.module.css'
import { useEffect, useState } from 'react'
import { apiGetAsync } from '../api/api'
import Link from 'next/link'

export default function Header(props: any) {

    const [isAuthenticated, setIsAuthenticated] = useState(false)
    
    useEffect(() => {
        apiGetAsync("/me")
        .then(response => {
            setIsAuthenticated(response.status == 200)
            props.setIsAuthenticated(response.status == 200)
        })
        .catch(error => {
            console.error(error)
        })
    }, [])

    return (
        <div className={`${styles.wrapper} flex max-width py-2 items-center
            space-x-3 text-white text-lg`}>
            <div className={`flex-none relative`}>
                <Image
                    alt={"termodom-logo"}
                    src={logoSvg}
                    height={48} />
            </div>

            { 
                isAuthenticated ?
                    <div className={`flex-auto space-x-4 px-4 pointer-events-auto`}>
                    <Link className={`hover:text-black py-6`} key='a-pocetna' href='/'>Početna</Link>
                        <Link className={`hover:text-black py-6`} key='a-porudzbine' href='/porudzbine'>Porudzbine</Link>
                        <Link className={`hover:text-black py-6`} key='a-proizvodi' href='/proizvodi'>Proizvodi</Link>
                        <Link className={`hover:text-black py-6`} key='a-podesavanja' href='/podesavanja'>Podesavanja</Link>
                    </div> : <div className={`font-medium`}>Please login to use admin panel</div>
            }
            {
                isAuthenticated ?
                    <div className={`flex-none space-x-4 px-4 pointer-events-auto`}>
                        <a className={`hover:text-black py-6`} key='a-izloguj-se' href='#' onClick={() => {
                            if(!window)
                                return

                            sessionStorage.clear()
                            location.reload()
                        }}>Izloguj se</a>
                    </div> : ''
            }
        </div>
    )
}