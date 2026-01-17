import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { DATE_FORMAT, ENDPOINTS_CONSTANTS } from '../../../constants'
import { CircularProgress } from '@mui/material'
import moment from 'moment'
import {
    decryptSpecifikacijaNovcaId,
    getUkupnoGotovine,
} from '../../../widgets/SpecifikacijaNovca/helpers/SpecifikacijaHelpers'
import { formatNumber } from '../../../helpers/numberHelpers'

const safeFormat = (value) => {
    const num = Number(value)
    return formatNumber(isNaN(num) ? 0 : num)
}

const printStyles = {
    page: {
        fontFamily: 'Arial, sans-serif',
        fontSize: '10px',
        padding: '10mm',
        maxWidth: '210mm',
        margin: '0 auto',
    },
    header: {
        display: 'flex',
        justifyContent: 'space-between',
        marginBottom: '8px',
        paddingBottom: '6px',
        borderBottom: '2px solid #333',
    },
    headerItem: {
        fontSize: '11px',
    },
    headerLabel: {
        fontWeight: 'bold',
        marginRight: '4px',
    },
    columns: {
        display: 'grid',
        gridTemplateColumns: '1fr 1fr 1fr',
        gap: '10px',
    },
    section: {
        marginBottom: '8px',
    },
    sectionTitle: {
        fontSize: '11px',
        fontWeight: 'bold',
        backgroundColor: '#e0e0e0',
        padding: '4px 6px',
        marginBottom: '4px',
        borderRadius: '2px',
    },
    table: {
        width: '100%',
        borderCollapse: 'collapse',
        fontSize: '9px',
    },
    tableHeader: {
        backgroundColor: '#f5f5f5',
        fontWeight: 'bold',
        textAlign: 'left',
        padding: '3px 4px',
        borderBottom: '1px solid #ccc',
    },
    tableCell: {
        padding: '2px 4px',
        borderBottom: '1px solid #eee',
    },
    tableCellRight: {
        padding: '2px 4px',
        borderBottom: '1px solid #eee',
        textAlign: 'right',
    },
    totalRow: {
        fontWeight: 'bold',
        backgroundColor: '#f0f0f0',
    },
    komentar: {
        fontStyle: 'italic',
        color: '#555',
        fontSize: '9px',
        padding: '4px',
        backgroundColor: '#fafafa',
        border: '1px solid #eee',
        borderRadius: '2px',
        minHeight: '20px',
    },
    obracun: {
        marginTop: '10px',
        padding: '8px',
        borderRadius: '4px',
    },
    obracunSuccess: {
        backgroundColor: '#c8e6c9',
        border: '1px solid #4caf50',
    },
    obracunError: {
        backgroundColor: '#ffcdd2',
        border: '1px solid #f44336',
    },
    obracunContent: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
    },
    obracunItem: {
        fontSize: '11px',
    },
    warningText: {
        color: '#d32f2f',
        fontWeight: 'bold',
        textAlign: 'center',
        marginBottom: '6px',
    },
    ostaloKomentar: {
        fontStyle: 'italic',
        color: '#666',
        fontSize: '8px',
    },
}

const SpecifikacijaNovcaPrintPage = () => {
    const router = useRouter()
    const { encryptedId } = router.query

    const [specifikacijaNovcaOstalo, setSpecifikacijaNovcaOstalo] = useState(0)
    const [obracunRazlika, setObracunRazlika] = useState(0)
    const [currentSpecification, setCurrentSpecification] = useState(null)

    useEffect(() => {
        if (!currentSpecification) return

        const ostaloSum =
            currentSpecification?.specifikacijaNovca.ostalo.reduce(
                (prevValue, currentValue) => prevValue + currentValue.vrednost,
                0
            ) ?? 0
        setSpecifikacijaNovcaOstalo(ostaloSum)

        const eur1Value =
            (currentSpecification?.specifikacijaNovca.eur1?.komada ?? 0) *
            (currentSpecification?.specifikacijaNovca.eur1?.kurs ?? 0)
        const eur2Value =
            (currentSpecification?.specifikacijaNovca.eur2?.komada ?? 0) *
            (currentSpecification?.specifikacijaNovca.eur2?.kurs ?? 0)

        setObracunRazlika(
            getUkupnoGotovine(currentSpecification) +
                eur1Value +
                eur2Value +
                ostaloSum -
                (currentSpecification?.racunar.racunarTraziValue ?? 0)
        )

        setTimeout(() => {
            const css =
                    '@page { size: A4; margin: 10mm; } html, body { margin: 0; padding: 0; }',
                head =
                    document.head || document.getElementsByTagName('head')[0],
                style = document.createElement('style')

            style.type = 'text/css'
            style.media = 'print'

            if (style.styleSheet) {
                style.styleSheet.cssText = css
            } else {
                style.appendChild(document.createTextNode(css))
            }

            head.appendChild(style)
        }, 500)
    }, [currentSpecification])

    useEffect(() => {
        if (!encryptedId) return

        const id = decryptSpecifikacijaNovcaId(encryptedId)
        officeApi
            .get(ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.GET(id))
            .then((response) => {
                setCurrentSpecification(response.data)
            })
            .catch(handleApiError)
    }, [encryptedId])

    if (!currentSpecification) return <CircularProgress />

    const { racunar, poreska, komentar, specifikacijaNovca } =
        currentSpecification

    const renderHeader = () => (
        <div style={printStyles.header}>
            <span style={printStyles.headerItem}>
                <span style={printStyles.headerLabel}>Broj:</span>
                {currentSpecification.id}
            </span>
            <span style={printStyles.headerItem}>
                <span style={printStyles.headerLabel}>Magacin:</span>
                {currentSpecification.magacinId}
            </span>
            <span style={printStyles.headerItem}>
                <span style={printStyles.headerLabel}>Datum:</span>
                {moment(currentSpecification.datumUTC).format(DATE_FORMAT)}
            </span>
        </div>
    )

    const renderRacunar = () => (
        <div style={printStyles.section}>
            <div style={printStyles.sectionTitle}>Računi</div>
            <table style={printStyles.table}>
                <tbody>
                    <tr>
                        <td style={printStyles.tableCell}>
                            1) Gotovinski računi
                        </td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(racunar.gotovinskiRacuni)}
                        </td>
                    </tr>
                    <tr>
                        <td style={printStyles.tableCell}>
                            2) Virmanski računi
                        </td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(racunar.virmanskiRacuni)}
                        </td>
                    </tr>
                    <tr>
                        <td style={printStyles.tableCell}>3) Kartice</td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(racunar.kartice)}
                        </td>
                    </tr>
                    <tr style={printStyles.totalRow}>
                        <td style={printStyles.tableCell}>
                            Ukupno (1+2+3)
                        </td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(racunar.racunarTraziValue)}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    )

    const renderPovratnice = () => (
        <div style={printStyles.section}>
            <div style={printStyles.sectionTitle}>Povratnice</div>
            <table style={printStyles.table}>
                <tbody>
                    <tr>
                        <td style={printStyles.tableCell}>Gotovinske</td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(racunar.gotovinskePovratnice)}
                        </td>
                    </tr>
                    <tr>
                        <td style={printStyles.tableCell}>Virmanske</td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(racunar.virmanskePovratnice)}
                        </td>
                    </tr>
                    <tr>
                        <td style={printStyles.tableCell}>Ostale</td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(racunar.ostalePovratnice)}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    )

    const renderPoreska = () => (
        <div style={printStyles.section}>
            <div style={printStyles.sectionTitle}>Poreska</div>
            <table style={printStyles.table}>
                <tbody>
                    <tr>
                        <td style={printStyles.tableCell}>
                            Fiskalizovani računi
                        </td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(poreska.fiskalizovaniRacuni)}
                        </td>
                    </tr>
                    <tr>
                        <td style={printStyles.tableCell}>
                            Fiskalizovane povratnice
                        </td>
                        <td style={printStyles.tableCellRight}>
                            {safeFormat(poreska.fiskalizovanePovratnice)}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    )

    const renderKomentar = () =>
        komentar ? (
            <div style={printStyles.section}>
                <div style={printStyles.sectionTitle}>Komentar</div>
                <div style={printStyles.komentar}>{komentar || '-'}</div>
            </div>
        ) : null

    const renderGotovina = () => {
        const novcanice = specifikacijaNovca.novcanice
            .slice()
            .sort((a, b) => b.key - a.key)
        const ukupnoGotovine = getUkupnoGotovine(currentSpecification)

        return (
            <div style={printStyles.section}>
                <div style={printStyles.sectionTitle}>Gotovina</div>
                <table style={printStyles.table}>
                    <thead>
                        <tr>
                            <th style={printStyles.tableHeader}>Apoeni</th>
                            <th
                                style={{
                                    ...printStyles.tableHeader,
                                    textAlign: 'right',
                                }}
                            >
                                Kom
                            </th>
                            <th
                                style={{
                                    ...printStyles.tableHeader,
                                    textAlign: 'right',
                                }}
                            >
                                Iznos
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {novcanice.map((n, i) => (
                            <tr key={i}>
                                <td style={printStyles.tableCell}>{n.key}</td>
                                <td style={printStyles.tableCellRight}>
                                    {n.value}
                                </td>
                                <td style={printStyles.tableCellRight}>
                                    {safeFormat(n.key * n.value)}
                                </td>
                            </tr>
                        ))}
                        <tr style={printStyles.totalRow}>
                            <td
                                style={printStyles.tableCell}
                                colSpan={2}
                            >
                                Ukupno
                            </td>
                            <td style={printStyles.tableCellRight}>
                                {safeFormat(ukupnoGotovine)} RSD
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        )
    }

    const renderEvri = () => {
        const eur1 = specifikacijaNovca?.eur1
        const eur2 = specifikacijaNovca?.eur2
        const eur1Total = (eur1?.komada ?? 0) * (eur1?.kurs ?? 0)
        const eur2Total = (eur2?.komada ?? 0) * (eur2?.kurs ?? 0)

        return (
            <div style={printStyles.section}>
                <div style={printStyles.sectionTitle}>EUR</div>
                <table style={printStyles.table}>
                    <thead>
                        <tr>
                            <th style={printStyles.tableHeader}></th>
                            <th
                                style={{
                                    ...printStyles.tableHeader,
                                    textAlign: 'right',
                                }}
                            >
                                Kom
                            </th>
                            <th
                                style={{
                                    ...printStyles.tableHeader,
                                    textAlign: 'right',
                                }}
                            >
                                Kurs
                            </th>
                            <th
                                style={{
                                    ...printStyles.tableHeader,
                                    textAlign: 'right',
                                }}
                            >
                                Iznos
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style={printStyles.tableCell}>EUR 1</td>
                            <td style={printStyles.tableCellRight}>
                                {eur1?.komada ?? 0}
                            </td>
                            <td style={printStyles.tableCellRight}>
                                {safeFormat(eur1?.kurs)}
                            </td>
                            <td style={printStyles.tableCellRight}>
                                {safeFormat(eur1Total)}
                            </td>
                        </tr>
                        <tr>
                            <td style={printStyles.tableCell}>EUR 2</td>
                            <td style={printStyles.tableCellRight}>
                                {eur2?.komada ?? 0}
                            </td>
                            <td style={printStyles.tableCellRight}>
                                {safeFormat(eur2?.kurs)}
                            </td>
                            <td style={printStyles.tableCellRight}>
                                {safeFormat(eur2Total)}
                            </td>
                        </tr>
                        <tr style={printStyles.totalRow}>
                            <td
                                style={printStyles.tableCell}
                                colSpan={3}
                            >
                                Ukupno EUR
                            </td>
                            <td style={printStyles.tableCellRight}>
                                {safeFormat(eur1Total + eur2Total)} RSD
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        )
    }

    const renderOstalo = () => {
        const ostalo = specifikacijaNovca?.ostalo || []

        return (
            <div style={printStyles.section}>
                <div style={printStyles.sectionTitle}>Ostalo</div>
                <table style={printStyles.table}>
                    <thead>
                        <tr>
                            <th style={printStyles.tableHeader}>Naziv</th>
                            <th
                                style={{
                                    ...printStyles.tableHeader,
                                    textAlign: 'right',
                                }}
                            >
                                Vrednost
                            </th>
                            <th style={printStyles.tableHeader}>Komentar</th>
                        </tr>
                    </thead>
                    <tbody>
                        {ostalo.map((item, i) => (
                            <tr key={i}>
                                <td style={printStyles.tableCell}>
                                    {item.key.charAt(0).toUpperCase() +
                                        item.key.slice(1)}
                                </td>
                                <td style={printStyles.tableCellRight}>
                                    {safeFormat(item.vrednost)}
                                </td>
                                <td
                                    style={{
                                        ...printStyles.tableCell,
                                        ...printStyles.ostaloKomentar,
                                    }}
                                >
                                    {item.komentar || '-'}
                                </td>
                            </tr>
                        ))}
                        <tr style={printStyles.totalRow}>
                            <td style={printStyles.tableCell}>Ukupno</td>
                            <td style={printStyles.tableCellRight}>
                                {safeFormat(specifikacijaNovcaOstalo)} RSD
                            </td>
                            <td style={printStyles.tableCell}></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        )
    }

    const renderObracun = () => {
        const isError =
            Math.abs(obracunRazlika) > 5 || racunar.imaNefiskalizovanih
        const obracunStyle = {
            ...printStyles.obracun,
            ...(isError
                ? printStyles.obracunError
                : printStyles.obracunSuccess),
        }

        return (
            <div style={obracunStyle}>
                {racunar.imaNefiskalizovanih && (
                    <div style={printStyles.warningText}>
                        ⚠ Imate nefiskalizovanih računa ili povratnica u
                        računaru
                    </div>
                )}
                <div style={printStyles.obracunContent}>
                    <span style={printStyles.obracunItem}>
                        <strong>Računi traže:</strong>{' '}
                        {safeFormat(racunar.racunarTraziValue)} RSD
                    </span>
                    <span style={printStyles.obracunItem}>
                        <strong>Razlika:</strong> {safeFormat(obracunRazlika)}{' '}
                        RSD
                    </span>
                </div>
            </div>
        )
    }

    return (
        <div style={printStyles.page}>
            {renderHeader()}
            <div style={printStyles.columns}>
                <div>
                    {renderRacunar()}
                    {renderPovratnice()}
                    {renderPoreska()}
                    {renderKomentar()}
                </div>
                <div>{renderGotovina()}</div>
                <div>
                    {renderEvri()}
                    {renderOstalo()}
                </div>
            </div>
            {renderObracun()}
        </div>
    )
}

export default SpecifikacijaNovcaPrintPage
