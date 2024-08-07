import {
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Checkbox,
} from '@mui/material'
import { IKorisniciListCheckBoxFilterProps } from '../interfaces/IKorisniciListCheckBoxFilterProps'

export const KorisniciListCheckBoxFilter = ({
    property,
    onClick,
    isChecked,
}: IKorisniciListCheckBoxFilterProps) => {
    return (
        <ListItem disablePadding>
            <ListItemButton
                role={undefined}
                onClick={onClick}
                dense
                sx={{ p: 0 }}
            >
                <ListItemIcon>
                    <Checkbox checked={isChecked} />
                </ListItemIcon>
                <ListItemText primary={property.name} />
            </ListItemButton>
        </ListItem>
    )
}
