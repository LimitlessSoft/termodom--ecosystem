import {
    Badge,
    Button,
    Grid,
    Stack,
    TextField,
    ToggleButton,
    Typography,
} from '@mui/material'
import { useState } from 'react'
import { BlogStatus, BlogStatusLabels } from '../../interfaces/IBlog'

export const BlogoviFilter = ({ onPretrazi, isFetching, currentBlogs }) => {
    const [text, setText] = useState('')
    const [statuses, setStatuses] = useState([BlogStatus.Draft, BlogStatus.Published])

    const handleSearch = () => {
        onPretrazi(text, statuses)
    }

    return (
        <Grid container alignItems="center" gap={2}>
            <Grid item xs={12}>
                <TextField
                    disabled={isFetching}
                    sx={{ minWidth: 400 }}
                    onChange={(e) => setText(e.target.value)}
                    onKeyDown={(e) => {
                        if (e.key === 'Enter' || e.key === 'Return') {
                            handleSearch()
                        }
                    }}
                    placeholder="Pretraga..."
                />
                <Button
                    variant="contained"
                    disabled={isFetching}
                    sx={{ m: 2 }}
                    onClick={handleSearch}
                >
                    Pretrazi
                </Button>
            </Grid>
            <Grid item>
                <Stack direction="row" gap={2}>
                    {Object.entries(BlogStatus).map(([key, value]) => (
                        <Badge
                            badgeContent={
                                currentBlogs?.filter((b) => b.status === value).length
                            }
                            color="warning"
                            key={key}
                        >
                            <ToggleButton
                                disabled={isFetching}
                                value={value}
                                selected={statuses?.includes(value)}
                                onClick={() => {
                                    if (statuses.includes(value)) {
                                        if (statuses.length === 1) {
                                            setStatuses(Object.values(BlogStatus))
                                            return
                                        }
                                        setStatuses(statuses.filter((s) => s !== value))
                                    } else {
                                        setStatuses([...statuses, value])
                                    }
                                }}
                            >
                                <Typography>{BlogStatusLabels[value]}</Typography>
                            </ToggleButton>
                        </Badge>
                    ))}
                </Stack>
            </Grid>
        </Grid>
    )
}
