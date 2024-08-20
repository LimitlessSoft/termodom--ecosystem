import { IPorudzbinaItemRowProps } from '../models/IPorudzbinaItemRowProps'
import { TableCell, TableRow, Tooltip, styled } from '@mui/material'
import { formatNumber } from '@/app/helpers/numberHelpers'
import InfoIcon from '@mui/icons-material/Info'
import WarningIcon from '@mui/icons-material/Warning'
import { STOCK_TYPES, STOCK_TYPES_MESSAGES } from '../../constants'

const PorudzbinaItemRowStyled = styled(TableRow)(
    ({ theme }) => `
    `
)

export const PorudzbinaItemRow = ({
    item,
    stockTypes,
    isDelivery,
}: IPorudzbinaItemRowProps) => {
    const stockType = stockTypes.find((type) => type.id === item.stockType)

    const renderStockTypeComponent = () => {
        if (!stockType || !stockType.name) return

        switch (stockType.name) {
            case STOCK_TYPES.VELIKA_STOVARISTA:
                return (
                    !isDelivery && (
                        <Tooltip
                            title={
                                STOCK_TYPES_MESSAGES.VELIKA_STOVARISTA_MESSAGE
                            }
                            enterTouchDelay={0}
                            leaveTouchDelay={6000}
                        >
                            <InfoIcon color="info" sx={{ display: 'flex' }} />
                        </Tooltip>
                    )
                )
            case STOCK_TYPES.TRANZIT:
                return (
                    <Tooltip title={STOCK_TYPES_MESSAGES.TRANZIT_MESSAGE}>
                        <WarningIcon color="warning" sx={{ display: 'flex' }} />
                    </Tooltip>
                )
        }
    }

    return (
        <PorudzbinaItemRowStyled>
            <TableCell>{item.name}</TableCell>
            <TableCell>{formatNumber(item.quantity)}</TableCell>
            <TableCell>{formatNumber(item.priceWithVAT)}</TableCell>
            <TableCell>{formatNumber(item.valueWithVAT)}</TableCell>
            <TableCell>{formatNumber(item.discount)}%</TableCell>
            <TableCell>{renderStockTypeComponent()}</TableCell>
        </PorudzbinaItemRowStyled>
    )
}
