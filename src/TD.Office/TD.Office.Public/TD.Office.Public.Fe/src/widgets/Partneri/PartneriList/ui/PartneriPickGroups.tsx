import {
    Checkbox,
    Dialog,
    FormControlLabel,
    Grid,
    useMediaQuery,
    useTheme,
} from '@mui/material'
import { IPartneriPickGroupsProps } from '@/widgets/Partneri/PartneriList/interfaces/IPartneriPickGroupsProps'
import { useEffect, useState } from 'react'

export const PartneriPickGroups = (props: IPartneriPickGroupsProps) => {
    const [checked, setChecked] = useState<number[]>([])

    const theme = useTheme()

    useEffect(() => {
        let kat = 0
        if (checked.length === 0) {
            props.onChange(kat, 0)
            return
        }

        const stepenRobaKategorije = (b: number) => {
            let strbk: number
            if (b > 0) {
                strbk = 1
                for (let j = 1; j <= b; j++) {
                    strbk = strbk * 2
                }
            } else {
                strbk = 1
            }
            return Math.abs(strbk)
        }

        for (let i = 0; i < checked.length; i++) {
            kat = kat + stepenRobaKategorije(i)
        }

        props.onChange(kat, checked.length)
    }, [checked])

    const isSmallScreen = useMediaQuery(theme.breakpoints.down('sm'))
    const isMediumScreen = useMediaQuery(theme.breakpoints.between('sm', 'md'))

    const numColumns = isSmallScreen ? 1 : isMediumScreen ? 2 : 3

    return (
        <Dialog
            open={props.open}
            onClose={props.onClose}
            sx={{ zIndex: 12000 }}
        >
            <Grid p={2} container spacing={2}>
                {Array(numColumns)
                    .fill(null)
                    .map((_, columnIndex) => {
                        const categoriesPerColumn = Math.ceil(
                            props.kategorije.length / numColumns
                        )

                        const columnCategories = props.kategorije.slice(
                            columnIndex * categoriesPerColumn,
                            (columnIndex + 1) * categoriesPerColumn
                        )

                        return (
                            <Grid item xs={12} sm={6} md={4} key={columnIndex}>
                                {columnCategories.map((kategorija: any) => (
                                    <FormControlLabel
                                        key={kategorija.katNaziv}
                                        control={
                                            <Checkbox
                                                onChange={(e) => {
                                                    if (e.target.checked) {
                                                        setChecked([
                                                            ...checked,
                                                            kategorija.katId,
                                                        ])
                                                    } else {
                                                        setChecked(
                                                            checked.filter(
                                                                (x) =>
                                                                    x !==
                                                                    kategorija.katId
                                                            )
                                                        )
                                                    }
                                                }}
                                                checked={checked.some(
                                                    (x) =>
                                                        x === kategorija.katId
                                                )}
                                            />
                                        }
                                        label={kategorija.katNaziv}
                                        style={{ width: '100%' }}
                                    />
                                ))}
                            </Grid>
                        )
                    })}
            </Grid>
        </Dialog>
    )
}
