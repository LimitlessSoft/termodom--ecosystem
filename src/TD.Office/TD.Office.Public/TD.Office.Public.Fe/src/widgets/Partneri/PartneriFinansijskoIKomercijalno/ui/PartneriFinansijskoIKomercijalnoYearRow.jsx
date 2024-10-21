import React from 'react'
import { TableRow, TableCell } from '@mui/material'
import { ALIGNMENT } from '../constants'
import { formatYear } from '../helpers/formatYear'

export default function PartneriFinansijskoIKomercijalnoYearRow({
    defaultData,
    partner,
    type,
}) {
    return (
        <TableRow>
            <TableCell align={ALIGNMENT}>{partner.ppid}</TableCell>
            <TableCell align={ALIGNMENT}>{partner.naziv}</TableCell>
            {defaultData.years.map((year, index) => {
                const currentYearData = partner[type].find((item) =>
                    item.year.toString().includes(formatYear(year.key))
                )

                let previousYearDataKraj = null
                if (index > 0) {
                    const previousYear = defaultData.years[index - 1]
                    const previousYearData = partner[type].find((item) =>
                        item.year
                            .toString()
                            .includes(formatYear(previousYear.key))
                    )
                    previousYearDataKraj = previousYearData?.kraj || 0
                }

                const overTolerance =
                    previousYearDataKraj !== null &&
                    currentYearData?.pocetak - previousYearDataKraj >=
                        defaultData.defaultTolerancija

                return (
                    <React.Fragment key={year.value}>
                        <TableCell
                            align={ALIGNMENT}
                            sx={{
                                color: overTolerance ? 'red' : '',
                            }}
                        >
                            {currentYearData ? currentYearData.pocetak : 0}
                        </TableCell>
                        <TableCell align={ALIGNMENT}>
                            {currentYearData ? currentYearData.kraj : 0}
                        </TableCell>
                    </React.Fragment>
                )
            })}
        </TableRow>
    )
}
