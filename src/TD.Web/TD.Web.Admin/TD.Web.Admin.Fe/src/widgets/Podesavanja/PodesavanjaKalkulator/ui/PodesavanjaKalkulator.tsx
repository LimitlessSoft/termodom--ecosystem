import {
    Box,
    LinearProgress,
    MenuItem,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { PodesavanjaKalkulatorRow } from '@/widgets/Podesavanja/PodesavanjaKalkulator/ui/PodesavanjaKalkulatorRow'
import { PodesavanjaKalkulatorNoviProizvod } from '@/widgets/Podesavanja/PodesavanjaKalkulator/ui/PodesavanjaKalkulatorNoviProizvod'

export const PodesavanjaKalkulator = () => {
    const [calculatorTypes, setCalculatorTypes] = useState<any>(undefined)
    const [selectedCalculatorType, setSelectedCalculatorType] =
        useState<any>(undefined)

    const [items, setItems] = useState<any>(undefined)

    const [filteredItems, setFilteredItems] = useState<any>(undefined)
    const [reloadTrigger, setReloadTrigger] = useState<any>(0)

    useEffect(() => {
        adminApi
            .get(`/calculator-items`)
            .then((response) => {
                setItems(response.data)
            })
            .catch(handleApiError)

        adminApi
            .get(`/calculator-types`)
            .then((response) => {
                setSelectedCalculatorType(response.data[0].id)
                setCalculatorTypes(response.data)
            })
            .catch(handleApiError)
    }, [reloadTrigger])

    useEffect(() => {
        if (items === undefined || selectedCalculatorType === undefined) return

        setFilteredItems(
            items
                .filter(
                    (item: any) =>
                        item.calculatorType === selectedCalculatorType
                )
                .toSorted((a: any, b: any) => a.order > b.order)
        )
    }, [items, selectedCalculatorType])

    const reload = () => {
        setReloadTrigger((prev: any) => prev + 1)
    }
    return (
        <Box sx={{ width: '100%' }}>
            {!calculatorTypes ||
            selectedCalculatorType === undefined ||
            !items ||
            !filteredItems ? (
                <LinearProgress />
            ) : (
                <Stack gap={2} p={2}>
                    <Typography variant={`h6`}>
                        Podesavanja kalkulatora
                    </Typography>
                    <TextField
                        select
                        value={selectedCalculatorType}
                        onChange={(e: any) => {
                            setSelectedCalculatorType(e.target.value)
                        }}
                    >
                        {calculatorTypes.map((ct: any, index: any) => {
                            return (
                                <MenuItem key={index} value={ct.id}>
                                    {ct.name}
                                </MenuItem>
                            )
                        })}
                    </TextField>
                    {filteredItems.length === 0 && (
                        <Typography>Nema proizvoda za prikazati</Typography>
                    )}
                    {filteredItems.length > 0 && (
                        <TableContainer component={Paper}>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Proizvod</TableCell>
                                        <TableCell>Kolicina</TableCell>
                                        <TableCell>JM</TableCell>
                                        <TableCell>Glavni</TableCell>
                                        <TableCell width={150}></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {filteredItems.map(
                                        (item: any, index: any) => (
                                            <PodesavanjaKalkulatorRow
                                                item={item}
                                                key={index}
                                                isFirst={
                                                    filteredItems[0].id ===
                                                    item.id
                                                }
                                                isLast={
                                                    filteredItems[
                                                        filteredItems.length - 1
                                                    ].id === item.id
                                                }
                                                onRowUpdated={reload}
                                            />
                                        )
                                    )}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    )}
                    <PodesavanjaKalkulatorNoviProizvod reload={reload} />
                </Stack>
            )}
        </Box>
    )
}
