import styles from './ProductCards.module.css'
import { IMAGE_BASE_URL } from '../api'
import Link from 'next/link'

export default function ProductCards(props) {

    return (
        <div className={styles.proizvodiWrapper}>
            { props.products?.map((p) => {
                let cena = props.cenovnik?.find(x => x.robaid == p.robaid)

                return (
                    <Link
                        key={'a1faac21a-' + p.robaid}
                        href={'/' + p.rel}>
                        <div
                            className={styles.proizvodItem}
                            data-klasifikacija={p.quality}
                            data-katbr={p.catalogueId}
                            data-robaid={p.id}
                            data-cenovna-grupa-id={p.priceListGroupId}>
                            <div className={styles.proizvodiItemInnerWrapper}>
                                <div className={styles.proizvodItemImageWrapper}>
                                    <img src={IMAGE_BASE_URL + p.imageUrl.toString().replace('/source/', '/128/') } />
                                </div>
                                <div className={styles.proizvodItemNaslov}>
                                    { p.name }
                                </div>
                                <div className={styles.jmLabel}>
                                    JM: { p.baseUnit }
                                </div>
                                {
                                    cena == null ?
                                        null :
                                        <div className={styles.cenaWrapper}>
                                            <div className={styles.vpCena}><label>VP:</label> {cena.prodajna_cena_bez_pdv.toFixed(2)} RSD</div>
                                            <div className={styles.mpCena}><label>MP:</label> {(cena.prodajna_cena_bez_pdv * ((100 + cena.pdv) / 100)).toFixed(2)} RSD</div>
                                        </div>
                                }
                            </div>
                        </div>
                    </Link>
                )
            })}
        </div>
    )
}