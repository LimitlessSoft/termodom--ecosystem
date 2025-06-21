import { Close, OpenInFull } from '@mui/icons-material'
import {
    Box,
    Dialog,
    DialogActions,
    DialogContent,
    Grid,
    IconButton,
    Paper,
    Stack,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { useZLeftMenuVisibleActions } from '../../../zStore'

export const Zoomable = ({
    children,
    gridItem,
    xs,
    md,
    lg,
    id,
    sx,
    component,
    groupState,
}) => {
    if (gridItem)
        return (
            <GridItemZoomable
                sx={sx}
                xs={xs}
                md={md}
                lg={lg}
                id={id}
                groupState={groupState}
            >
                {children}
            </GridItemZoomable>
        )
    return (
        <PopupZoomable sx={sx} component={component}>
            {children}
        </PopupZoomable>
    )
}

const GridItemZoomable = ({ children, xs, md, lg, id, groupState, sx }) => {
    const zLeftMenuVisibleActions = useZLeftMenuVisibleActions()
    const [zoomed, setZoomed] = groupState
    const [state, setState] = useState(false)

    const getIsZoomed = () => {
        if (id && zoomed) return zoomed === id
        return state
    }

    const handleZoomToggle = () => {
        if (id && setZoomed) {
            setZoomed(id)
        } else {
            setState((prev) => !prev)
        }
    }

    useEffect(() => {
        if (!zLeftMenuVisibleActions) return

        if (zoomed) {
            zLeftMenuVisibleActions.hide()
        } else {
            zLeftMenuVisibleActions.show()
        }
    }, [zoomed, zLeftMenuVisibleActions])

    return (
        <Grid
            item
            xs={getIsZoomed() ? 12 : xs}
            md={getIsZoomed() ? 12 : md}
            lg={getIsZoomed() ? 12 : lg}
            sx={{
                ...sx,
                transitionDuration: '0.3s',
            }}
        >
            <Paper
                sx={{
                    position: 'relative',
                    maxHeight: '80vh',
                    overflow: 'auto',
                }}
            >
                <Stack direction={`row`} justifyContent={`end`}>
                    <IconButton onClick={handleZoomToggle}>
                        <OpenInFull />
                    </IconButton>
                </Stack>
                <Box
                    sx={{
                        position: 'relative',
                    }}
                >
                    {children}
                </Box>
            </Paper>
        </Grid>
    )
}

const PopupZoomable = ({ children, component, sx }) => {
    const [zoomed, setZoomed] = useState(false)
    const zLeftMenuVisibleActions = useZLeftMenuVisibleActions()

    useEffect(() => {
        if (!zLeftMenuVisibleActions) return

        if (zoomed) {
            zLeftMenuVisibleActions.hide()
        } else {
            zLeftMenuVisibleActions.show()
        }
    }, [zoomed, zLeftMenuVisibleActions])

    return (
        <Box component={component} sx={sx}>
            <Stack direction={`row`} justifyContent={`end`}>
                <IconButton onClick={() => setZoomed(true)}>
                    <OpenInFull />
                </IconButton>
            </Stack>
            <Dialog
                open={zoomed}
                fullWidth={true}
                maxWidth={null}
                onClose={() => setZoomed(false)}
            >
                <DialogActions>
                    <IconButton onClick={() => setZoomed(false)}>
                        <Close />
                    </IconButton>
                </DialogActions>
                <DialogContent>{children}</DialogContent>
            </Dialog>
            <>{children}</>
        </Box>
    )
}
