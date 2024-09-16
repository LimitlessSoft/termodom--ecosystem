import { Checkbox, Dialog, FormControlLabel, Grid } from '@mui/material'
import { IPartneriPickGroupsProps } from '@/widgets/Partneri/PartneriList/interfaces/IPartneriPickGroupsProps'
import { useEffect, useState } from 'react'

export const PartneriPickGroups = (props: IPartneriPickGroupsProps) => {
    const [checked, setChecked] = useState<number[]>([])

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

    return (
        <Dialog open={props.open} onClose={props.onClose}>
            <Grid p={2} container>
                {props.kategorije.map((kategorija: any) => {
                    return (
                        <Grid item key={kategorija.katNaziv} xs={6}>
                            <FormControlLabel
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
                                                            x !=
                                                            kategorija.katId
                                                    )
                                                )
                                            }
                                        }}
                                        checked={checked.some(
                                            (x) => x == kategorija.katId
                                        )}
                                    />
                                }
                                label={kategorija.katNaziv}
                            />
                        </Grid>
                    )
                })}
            </Grid>
        </Dialog>
    )
}
