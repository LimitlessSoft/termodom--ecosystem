import {
    CircularProgress,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@mui/material'
import { useZMassSMSQueue } from '../../../zStore'

export const MassSMSQueue = () => {
    const queue = useZMassSMSQueue()

    if (!queue) return <CircularProgress />

    return (
        <>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Broj Telefona</TableCell>
                            <TableCell>Tekst</TableCell>
                            {/*<TableCell>/!*Akcija*!/</TableCell>*/}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {queue.map((item, i) => (
                            <TableRow key={`sms-${i}`}>
                                <TableCell>{item.phoneNumber}</TableCell>
                                <TableCell>{item.text}</TableCell>
                                {/*<TableCell*/}
                                {/*    sx={{*/}
                                {/*        textAlign: 'right',*/}
                                {/*    }}*/}
                                {/*>*/}
                                {/*    <IconButton>*/}
                                {/*        <Delete />*/}
                                {/*    </IconButton>*/}
                                {/*</TableCell>*/}
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    )
}
