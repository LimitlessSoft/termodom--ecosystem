import Grid2 from '@mui/material/Unstable_Grid2'
import { PartneriDropdown } from '@/widgets'
import { PartneriAnalizaYearTable } from './PartneriAnalizaYearTable'
import { useState } from 'react'

export const PartneriAnaliza = () => {
    const [ppid, setPpid] = useState(null)

    return (
        <Grid2 container gap={2}>
            <Grid2>
                <PartneriDropdown
                    onChange={(value) => {
                        setPpid(value.ppid)
                    }}
                />
            </Grid2>
            <Grid2 sm={12}>
                <PartneriAnalizaYearTable ppid={ppid} />
            </Grid2>
        </Grid2>
    )
}
