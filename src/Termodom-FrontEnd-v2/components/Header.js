import Link from 'next/link'
import styles from './Header.module.css'

export default function Header() {
    return (
        <div className={styles.wrapper}>
            <Link href='/'>
                <img className={styles.link} src='/Termodom_Logo_White.svg' />
            </Link>
            <Link href='/'>
                <div className={styles.link}>
                    Prodavnica
                </div>
            </Link>
            <Link href='/kontakt'>
                <div className={styles.link}>
                    Kontakt
                </div>
            </Link>
            <div className={styles.filler}></div>
            <Link href='/korpa'>
                <div className={styles.link}>
                    Korpa
                </div>
            </Link>
            <Link href='/profi-kutak'>
                <div className={styles.link}>
                    Profi Kutak
                </div>
            </Link>
        </div>
    )
}