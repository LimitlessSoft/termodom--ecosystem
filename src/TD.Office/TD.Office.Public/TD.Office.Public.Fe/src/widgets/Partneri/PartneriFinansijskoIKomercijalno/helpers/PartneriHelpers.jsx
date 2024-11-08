import { PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS } from '@/constants'
import { Typography, Stack, Box } from '@mui/material'
import { formatNumber } from '@/helpers/numberHelpers'

export const renderRow = (params, year, type, tolerance) => {
    const { KOMERCIJALNO, FINANSIJSKO_KUPAC, FINANSIJSKO_DOBAVLJAC } =
        PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS

    const getRowData = (yearData, rowType) =>
        params.row[rowType].find(
            (row) => row.year.toString() === yearData.toString()
        )

    const currentKomercijalnoRow = getRowData(year, KOMERCIJALNO)
    const previousKomercijalnoRow = getRowData(year - 1, KOMERCIJALNO)

    const currentFinansijskoKupacRow = getRowData(year, FINANSIJSKO_KUPAC)
    const previousFinansijskoKupacRow = getRowData(year - 1, FINANSIJSKO_KUPAC)

    const currentFinansijskoDobavljacRow = getRowData(
        year,
        FINANSIJSKO_DOBAVLJAC
    )
    const previousFinansijskoDobavljacRow = getRowData(
        year - 1,
        FINANSIJSKO_DOBAVLJAC
    )

    const isToleranceExceeded = (val1, val2) =>
        Math.abs((val1 || 0) - (val2 || 0)) >= tolerance

    const isKomercijalnoStartGreaterThanPreviousEnd =
        previousKomercijalnoRow &&
        isToleranceExceeded(
            currentKomercijalnoRow?.pocetak,
            previousKomercijalnoRow?.kraj
        )

    const isFinansijskoKupacStartGreaterThanPreviousEnd =
        previousFinansijskoKupacRow &&
        isToleranceExceeded(
            currentFinansijskoKupacRow?.pocetak,
            previousFinansijskoKupacRow?.kraj
        )

    const isFinansijskoDobavljacStartGreaterThanPreviousEnd =
        previousFinansijskoDobavljacRow &&
        isToleranceExceeded(
            currentFinansijskoDobavljacRow?.pocetak,
            previousFinansijskoDobavljacRow?.kraj
        )

    const isDifferenceBetweenPocetakExceeded = isToleranceExceeded(
        currentKomercijalnoRow?.pocetak,
        (currentFinansijskoKupacRow?.pocetak || 0) +
            (currentFinansijskoDobavljacRow?.pocetak || 0)
    )

    const isDifferenceBetweenKrajExceeded = isToleranceExceeded(
        currentKomercijalnoRow?.kraj,
        (currentFinansijskoKupacRow?.kraj || 0) +
            (currentFinansijskoDobavljacRow?.kraj || 0)
    )

    const isStart =
        type ===
        PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
            .POCETAK_SUFFIX
    const isEnd =
        type ===
        PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
            .KRAJ_SUFFIX

    const stackColor =
        (isStart &&
            (isDifferenceBetweenPocetakExceeded ||
                isKomercijalnoStartGreaterThanPreviousEnd ||
                isFinansijskoKupacStartGreaterThanPreviousEnd ||
                isFinansijskoDobavljacStartGreaterThanPreviousEnd)) ||
        (isEnd && isDifferenceBetweenKrajExceeded)
            ? 'red'
            : ''

    return (
        <Stack key={year} gap={1} my={1} color={stackColor}>
            <Typography>
                Komercijalno:{' '}
                {formatNumber(
                    isStart
                        ? currentKomercijalnoRow?.pocetak || 0
                        : currentKomercijalnoRow?.kraj || 0
                )}
            </Typography>
            <Typography>
                Finansijsko kupac:{' '}
                {formatNumber(
                    isStart
                        ? currentFinansijskoKupacRow?.pocetak || 0
                        : currentFinansijskoKupacRow?.kraj || 0
                )}
            </Typography>
            <Typography>
                Finansijsko dobavljac:{' '}
                {formatNumber(
                    isStart
                        ? currentFinansijskoKupacRow?.pocetak || 0
                        : currentFinansijskoKupacRow?.kraj || 0
                )}
            </Typography>
        </Stack>
    )
}

export const renderCell = (params) => (
    <Box>
        <Typography>{params.value}</Typography>
    </Box>
)

export const generateColumns = (year, tolerance) => [
    {
        field: `${year}_${PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.KRAJ_SUFFIX}`,
        headerName: `${year} - ${PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.KRAJ_SUFFIX}`,
        width: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.COLUMN_TABLE_WIDTH,
        renderCell: (params) =>
            renderRow(
                params,
                year,
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                    .KRAJ_SUFFIX,
                tolerance
            ),
        disableColumnMenu: true,
        sortable: false,
    },
    {
        field: `${year}_${PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.POCETAK_SUFFIX}`,
        headerName: `${year} - ${PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.POCETAK_SUFFIX}`,
        width: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.COLUMN_TABLE_WIDTH,
        renderCell: (params) =>
            renderRow(
                params,
                year,
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                    .POCETAK_SUFFIX,
                tolerance
            ),
        disableColumnMenu: true,
        sortable: false,
    },
]
