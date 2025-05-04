import { Paper, Stack, Switch, Typography } from '@mui/material'

export const MagaciniDropdownSviFilter = ({ value, setValue, disabled }) => {
    return (
        <Paper
            sx={{
                backgroundColor: disabled ? `grey.100` : `white`,
            }}
        >
            <Stack m={1} direction={`row`} alignItems={`center`}>
                <Typography>Svi magacini</Typography>
                <Switch
                    disabled={disabled}
                    checked={value}
                    onChange={(e) => {
                        setValue(e.target.checked)
                    }}
                />
            </Stack>
        </Paper>
    )
}
