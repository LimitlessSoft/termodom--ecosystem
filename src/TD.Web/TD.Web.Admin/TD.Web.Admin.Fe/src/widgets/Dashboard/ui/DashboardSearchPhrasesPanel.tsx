import { useEffect, useState } from 'react'
import { DashboardPanel } from './DashboardPanel'
import { ApiBase, fetchApi } from '@/api'
import {
    Button,
    CircularProgress,
    Dialog,
    DialogContent,
    DialogTitle,
    List,
    ListItem,
    ListItemText,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material'

export const DashboardSearchPhrasesPanel = (): JSX.Element => {
    const [data, setData] = useState<any[] | undefined>(undefined)
    const [currentWordDetails, setCurrentWordDetails] = useState<
        string | undefined
    >(undefined)

    useEffect(() => {
        const dateFrom = new Date()
        dateFrom.setDate(dateFrom.getDate() - 7)

        fetchApi(
            ApiBase.Main,
            `/search-phrases-statistics?DateFromUtc=${dateFrom.toISOString()}&DateToUtc=${new Date().toISOString()}`
        ).then((response: any) => {
            response.json().then((response: any) => {
                setData(
                    response.items
                        .toSorted(
                            (x: any, y: any) =>
                                y.searchedTimesCount - x.searchedTimesCount
                        )
                        .slice(0, 10)
                )
            })
        })
    }, [])

    return (
        <DashboardPanel title={`Pretrage proizvoda ove nedelje`}>
            {data != null && (
                <Dialog
                    open={currentWordDetails != null && data != null}
                    onClose={() => {
                        setCurrentWordDetails(undefined)
                    }}
                >
                    <DialogTitle>
                        Konteksti pretrage reči: &quot;{currentWordDetails}
                        &quot;
                    </DialogTitle>
                    <DialogContent>
                        <List dense={true}>
                            {data!
                                .find((x) => x.keyword == currentWordDetails)
                                ?.phrases.map((item: any, index: number) => (
                                    <ListItemText key={index} primary={item} />
                                ))}
                        </List>
                        <Typography></Typography>
                    </DialogContent>
                </Dialog>
            )}
            {data == null ? (
                <CircularProgress />
            ) : (
                <Table>
                    <TableContainer>
                        <TableHead>
                            <TableCell>Reč</TableCell>
                            <TableCell>Pretraženo puta</TableCell>
                            <TableCell>Kontekst pretrage</TableCell>
                        </TableHead>
                        <TableBody>
                            {data.map((item: any, index: number) => (
                                <TableRow key={index}>
                                    <TableCell sx={{ p: 0 }}>
                                        {item.keyword}
                                    </TableCell>
                                    <TableCell
                                        sx={{ p: 0, textAlign: `center` }}
                                    >
                                        {item.searchedTimesCount}
                                    </TableCell>
                                    <TableCell sx={{ p: 0 }}>
                                        <Button
                                            onClick={() => {
                                                setCurrentWordDetails(
                                                    item.keyword
                                                )
                                            }}
                                        >
                                            Prikaži kontekste
                                        </Button>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </TableContainer>
                </Table>
            )}
        </DashboardPanel>
    )
}
