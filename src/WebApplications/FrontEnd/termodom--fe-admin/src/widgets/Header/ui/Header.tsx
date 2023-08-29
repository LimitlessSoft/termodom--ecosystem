import Link from 'next/link'
import styles from './Header.module.css'

export const Header = (): JSX.Element => {
    return (
        <header className={`${styles.header} text-xl`}>
            <div className={`flex items-center`}>
                <div className={`flex-none`}>
                    <div className={`${styles.button} drop-shadow-md`}>
                        <Link href="/">
                            <img src="/termodom-logo-white.svg" className={`${styles.logo}`} />
                        </Link>
                    </div>
                </div>
                <div className={`flex-1 flex`}>
                    <div className={`${styles.button} drop-shadow-md`}>
                        <Link href="/kontrolna-tabla">
                            Kontrolna tabla
                        </Link>
                    </div>
                    <div className={`${styles.button} drop-shadow-md`}>
                        <Link href="/proizvodi">
                            Proizvodi
                        </Link>
                    </div>
                    <div className={`${styles.button} drop-shadow-md`}>
                        <Link href="/korisnici">
                            Korisnici
                        </Link>
                    </div>
                    <div className={`${styles.button} drop-shadow-md`}>
                        <Link href="/podešavanja">
                            Podešavanja
                        </Link>
                    </div>
                </div>
                <div className={`${styles.button} flex-none drop-shadow-md`}>
                </div>
            </div>
        </header>
    )
}