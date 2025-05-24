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
import { useEffect, useRef, useState } from 'react'

export const Zoomable = ({
    children,
    gridItem,
    xs,
    md,
    lg,
    id,
    component,
    groupState,
    headerRef,
}) => {
    if (gridItem)
        return (
            <GridItemZoomable
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
        <PopupZoomable component={component} headerRef={headerRef}>
            {children}
        </PopupZoomable>
    )
}

const GridItemZoomable = ({ children, xs, md, lg, id, groupState }) => {
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

    return (
        <Grid
            item
            xs={getIsZoomed() ? 12 : xs}
            md={getIsZoomed() ? 12 : md}
            lg={getIsZoomed() ? 12 : lg}
            sx={{
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

const HeaderBarContent = ({ onClick }) => {
    return (
        <IconButton onClick={onClick}>
            <OpenInFull />
        </IconButton>
    )
}

const PopupZoomable = ({ children, component, headerRef }) => {
    const [zoomed, setZoomed] = useState(false)
    useEffect(() => {
        if (!headerRef || !headerRef.current) return
        import('react-dom').then((ReactDOM) => {
            const headerBarContent = (
                <HeaderBarContent onClick={() => setZoomed(true)} />
            )
            ReactDOM.createRoot(headerRef.current).render(headerBarContent) // This will not work with multiple renders and is bugged totally, do not use further, this should be refactored
        })
    }, [headerRef, headerRef?.current])

    return (
        <Box component={component}>
            {!headerRef && (
                <Stack direction={`row`} justifyContent={`end`}>
                    <IconButton onClick={() => setZoomed(true)}>
                        <OpenInFull />
                    </IconButton>
                </Stack>
            )}
            <Dialog
                open={zoomed}
                fullWidth={true}
                maxWidth={`xl`}
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
