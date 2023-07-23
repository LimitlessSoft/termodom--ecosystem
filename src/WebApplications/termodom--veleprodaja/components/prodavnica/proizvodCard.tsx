import Image from "next/image";
import styles from './proizvodCard.module.css'
import { useEffect } from "react";

export default function ProizvodCard(props: any) {
    
    const proizvod = props.proizvod;

    useEffect(() => {
        console.log(proizvod)
    }, [])
    return (
        <a href={`/proizvod`} className={`${styles.card}
            ${proizvod.klasifikacija == 2 ?
                styles.borderOrange :
                proizvod.klasifikacija == 1 ?
                    styles.borderGreen :
                    styles.borderGray}
            p-1`}>
            
            <div className={styles.imgWrapper}>
                <img
                    className={``}
                    src={`https://termodom.rs${proizvod.slika}`}
                    alt={proizvod.naziv} />
            </div>
            <div className={`text-md font-medium text-center py-1`}>
                {proizvod.naziv}
            </div>
        </a>
    )
}