import Image from 'next/image'
import logoSvg from '../src/assets/images/Termodom_Logo_White.svg'
import styles from './header.module.css'

export default function Header() {
    return (
        <div className={`${styles.wrapper} flex max-width py-2 items-center
            space-x-3 text-white text-lg`}>
            <div className={`flex-none relative`}>
                <Image
                    alt={"termodom-logo"}
                    src={logoSvg}
                    height={48} />
            </div>
            <div className={`flex-auto space-x-4 px-4 pointer-events-auto`}>
                <a className={`hover:text-black py-6`} key='a-prodavnica' href='/prodavnica'>Prodavnica</a>
                <a className={`hover:text-black py-6`} key='a-kontakt' href='/kontakt'>Kontakt</a>
                <a className={`hover:text-black py-6`} key='a-maloprodaja' href='/maloprodaja'>Maloprodaja</a>
            </div>
            <div className={`flex-none space-x-4 px-4 pointer-events-auto`}>
                <a className={`hover:text-black py-6`} key='a-korpa' href='/korpa'>Korpa</a>
                <a className={`hover:text-black py-6`} key='a-profi-kutak' href='/profi-kutak'>Profi kutak</a>
                <a className={`hover:text-black py-6`} key='a-izloguj-se' href='/izloguj-se'>Izloguj se</a>
            </div>
        </div>
    )
}