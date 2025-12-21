import React, { useState, useEffect, useRef, useCallback } from 'react'
import { Box, Paper, IconButton, useTheme, useMediaQuery } from '@mui/material'
import DragIndicatorIcon from '@mui/icons-material/DragIndicator'
import CloseIcon from '@mui/icons-material/Close'
import UnfoldMoreIcon from '@mui/icons-material/UnfoldMore'
import UnfoldLessIcon from '@mui/icons-material/UnfoldLess'
import { styled } from '@mui/material/styles'
import { useZLeftMenuVisible } from '../../zStore'

const StyledPaper = styled(Paper)(({ theme }) => ({
    position: 'fixed',
    zIndex: 1000,
    overflow: 'auto',
    display: 'flex',
    flexDirection: 'column',
    boxShadow: theme.shadows[10],
    border: `1px solid ${theme.palette.divider}`,
    backgroundColor: theme.palette.background.paper,
}))

const DragHandle = styled(Box)(({ theme }) => ({
    position: 'absolute',
    top: 24,
    left: 24,
    cursor: 'grab',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    zIndex: 1001,
    color: theme.palette.text.secondary,
    '&:active': {
        cursor: 'grabbing',
    },
}))

const CloseButtonWrapper = styled(Box)({
    position: 'absolute',
    top: 20,
    right: 20,
    zIndex: 1001,
})

const MaximizeVerticalButtonWrapper = styled(Box)({
    position: 'absolute',
    top: 20,
    right: 56,
    zIndex: 1001,
})

const MaximizeHorizontalButtonWrapper = styled(Box)({
    position: 'absolute',
    top: 20,
    right: 92,
    zIndex: 1001,
})

const ContentWrapper = styled(Box)({
    flex: 1,
    overflow: 'auto',
    padding: '48px 24px 24px 24px',
    display: 'block',
    width: 'calc(100% - 48px)',
    height: '100%',
})

// Resize handle components
const ResizeHandle = styled(Box)({
    position: 'absolute',
    zIndex: 1001,
})

const ResizeHandleNW = styled(ResizeHandle)({
    top: 0,
    left: 0,
    width: 20,
    height: 20,
    cursor: 'nw-resize',
})

const ResizeHandleN = styled(ResizeHandle)({
    top: 0,
    left: 20,
    right: 20,
    height: 16,
    cursor: 'n-resize',
})

const ResizeHandleNE = styled(ResizeHandle)({
    top: 0,
    right: 0,
    width: 20,
    height: 20,
    cursor: 'ne-resize',
})

const ResizeHandleE = styled(ResizeHandle)({
    top: 20,
    right: 0,
    bottom: 20,
    width: 16,
    cursor: 'e-resize',
})

const ResizeHandleSE = styled(ResizeHandle)({
    bottom: 0,
    right: 0,
    width: 20,
    height: 20,
    cursor: 'se-resize',
})

const ResizeHandleS = styled(ResizeHandle)({
    bottom: 0,
    left: 20,
    right: 20,
    height: 16,
    cursor: 's-resize',
})

const ResizeHandleSW = styled(ResizeHandle)({
    bottom: 0,
    left: 0,
    width: 20,
    height: 20,
    cursor: 'sw-resize',
})

const ResizeHandleW = styled(ResizeHandle)({
    top: 20,
    left: 0,
    bottom: 20,
    width: 16,
    cursor: 'w-resize',
})

export const PopupBox = ({ children, onClose }) => {
    const theme = useTheme()
    const isDesktop = useMediaQuery(theme.breakpoints.up('md'))
    const leftMenuVisible = useZLeftMenuVisible()

    const [position, setPosition] = useState({ x: 0, y: 0 })
    const [size, setSize] = useState({ width: 0, height: 0 })
    const [isDragging, setIsDragging] = useState(false)
    const [isResizing, setIsResizing] = useState({
        active: false,
        handle: null,
    })
    const [isMaximizedVertical, setIsMaximizedVertical] = useState(false)
    const [isMaximizedHorizontal, setIsMaximizedHorizontal] = useState(false)
    const dragStart = useRef({ x: 0, y: 0 })
    const resizeStart = useRef({
        x: 0,
        y: 0,
        width: 0,
        height: 0,
        posX: 0,
        posY: 0,
    })
    const previousSize = useRef({ width: 0, height: 0 })
    const previousPosition = useRef({ x: 0, y: 0 })
    const isFirstRender = useRef(true)
    const MARGIN = 16
    const MIN_WIDTH = 300
    const MIN_HEIGHT = 200

    const calculateBoundaries = useCallback(() => {
        const leftMenuWidth = isDesktop && leftMenuVisible ? 64 : 0

        return {
            minX: MARGIN + leftMenuWidth,
            minY: MARGIN,
            maxX: window.innerWidth - size.width - MARGIN,
            maxY: window.innerHeight - size.height - MARGIN,
        }
    }, [isDesktop, leftMenuVisible, size.width, size.height])

    useEffect(() => {
        const handleWindowResize = () => {
            setPosition((prev) => {
                const { minX, minY, maxX, maxY } = calculateBoundaries()
                return {
                    x: Math.max(minX, Math.min(prev.x, maxX)),
                    y: Math.max(minY, Math.min(prev.y, maxY)),
                }
            })

            setSize((prev) => {
                const leftMenuWidth = isDesktop && leftMenuVisible ? 64 : 0
                const maxWidth = window.innerWidth - leftMenuWidth - MARGIN * 2
                const maxHeight = window.innerHeight - MARGIN * 2
                return {
                    width: Math.min(prev.width, maxWidth),
                    height: Math.min(prev.height, maxHeight),
                }
            })
        }

        if (isFirstRender.current) {
            const initialWidth = window.innerWidth * 0.5
            const initialHeight = window.innerHeight * 0.5
            setSize({ width: initialWidth, height: initialHeight })

            const leftMenuWidth = isDesktop && leftMenuVisible ? 64 : 0
            const initialX = window.innerWidth - initialWidth - MARGIN
            const initialY = window.innerHeight - initialHeight - MARGIN
            setPosition({ x: initialX, y: initialY })
            isFirstRender.current = false
        } else {
            handleWindowResize()
        }

        window.addEventListener('resize', handleWindowResize)
        return () => window.removeEventListener('resize', handleWindowResize)
    }, [calculateBoundaries, isDesktop, leftMenuVisible])

    const handleMouseDown = (e) => {
        setIsDragging(true)
        dragStart.current = {
            x: e.clientX - position.x,
            y: e.clientY - position.y,
        }
        e.preventDefault()
    }

    useEffect(() => {
        const handleMouseMove = (e) => {
            if (!isDragging) return

            let newX = e.clientX - dragStart.current.x
            let newY = e.clientY - dragStart.current.y

            const { minX, minY, maxX, maxY } = calculateBoundaries()

            newX = Math.max(minX, Math.min(newX, maxX))
            newY = Math.max(minY, Math.min(newY, maxY))

            setPosition({ x: newX, y: newY })
        }

        const handleMouseUp = () => {
            setIsDragging(false)
        }

        if (isDragging) {
            window.addEventListener('mousemove', handleMouseMove)
            window.addEventListener('mouseup', handleMouseUp)
        }

        return () => {
            window.removeEventListener('mousemove', handleMouseMove)
            window.removeEventListener('mouseup', handleMouseUp)
        }
    }, [isDragging, calculateBoundaries])

    const handleResizeMouseDown = (handle, e) => {
        setIsResizing({ active: true, handle })
        resizeStart.current = {
            x: e.clientX,
            y: e.clientY,
            width: size.width,
            height: size.height,
            posX: position.x,
            posY: position.y,
        }
        e.preventDefault()
        e.stopPropagation()
    }

    const handleMaximizeVertical = () => {
        const maxHeight = window.innerHeight - MARGIN * 2

        if (isMaximizedVertical) {
            // Restore previous vertical size
            setSize((prev) => ({
                ...prev,
                height: previousSize.current.height,
            }))
            setPosition((prev) => ({ ...prev, y: previousPosition.current.y }))
            setIsMaximizedVertical(false)
        } else {
            // Save current state before maximizing
            previousSize.current = {
                ...previousSize.current,
                height: size.height,
            }
            previousPosition.current = {
                ...previousPosition.current,
                y: position.y,
            }

            // Maximize vertically
            setSize((prev) => ({ ...prev, height: maxHeight }))
            setPosition((prev) => ({ ...prev, y: MARGIN }))
            setIsMaximizedVertical(true)
        }
    }

    const handleMaximizeHorizontal = () => {
        const leftMenuWidth = isDesktop && leftMenuVisible ? 64 : 0
        const maxWidth = window.innerWidth - leftMenuWidth - MARGIN * 2

        if (isMaximizedHorizontal) {
            // Restore previous horizontal size
            setSize((prev) => ({ ...prev, width: previousSize.current.width }))
            setPosition((prev) => ({ ...prev, x: previousPosition.current.x }))
            setIsMaximizedHorizontal(false)
        } else {
            // Save current state before maximizing
            previousSize.current = {
                ...previousSize.current,
                width: size.width,
            }
            previousPosition.current = {
                ...previousPosition.current,
                x: position.x,
            }

            // Maximize horizontally
            setSize((prev) => ({ ...prev, width: maxWidth }))
            setPosition((prev) => ({ ...prev, x: MARGIN + leftMenuWidth }))
            setIsMaximizedHorizontal(true)
        }
    }

    useEffect(() => {
        const handleResizeMouseMove = (e) => {
            if (!isResizing.active) return

            const deltaX = e.clientX - resizeStart.current.x
            const deltaY = e.clientY - resizeStart.current.y

            const leftMenuWidth = isDesktop && leftMenuVisible ? 64 : 0
            const maxWidth = window.innerWidth - leftMenuWidth - MARGIN * 2
            const maxHeight = window.innerHeight - MARGIN * 2

            let newWidth = resizeStart.current.width
            let newHeight = resizeStart.current.height
            let newX = resizeStart.current.posX
            let newY = resizeStart.current.posY

            switch (isResizing.handle) {
                case 'se':
                    newWidth = Math.max(
                        MIN_WIDTH,
                        Math.min(resizeStart.current.width + deltaX, maxWidth)
                    )
                    newHeight = Math.max(
                        MIN_HEIGHT,
                        Math.min(resizeStart.current.height + deltaY, maxHeight)
                    )
                    break
                case 'sw':
                    newWidth = Math.max(
                        MIN_WIDTH,
                        Math.min(resizeStart.current.width - deltaX, maxWidth)
                    )
                    newHeight = Math.max(
                        MIN_HEIGHT,
                        Math.min(resizeStart.current.height + deltaY, maxHeight)
                    )
                    newX =
                        resizeStart.current.posX +
                        (resizeStart.current.width - newWidth)
                    newX = Math.max(MARGIN + leftMenuWidth, newX)
                    break
                case 'ne':
                    newWidth = Math.max(
                        MIN_WIDTH,
                        Math.min(resizeStart.current.width + deltaX, maxWidth)
                    )
                    newHeight = Math.max(
                        MIN_HEIGHT,
                        Math.min(resizeStart.current.height - deltaY, maxHeight)
                    )
                    newY =
                        resizeStart.current.posY +
                        (resizeStart.current.height - newHeight)
                    newY = Math.max(MARGIN, newY)
                    break
                case 'nw':
                    newWidth = Math.max(
                        MIN_WIDTH,
                        Math.min(resizeStart.current.width - deltaX, maxWidth)
                    )
                    newHeight = Math.max(
                        MIN_HEIGHT,
                        Math.min(resizeStart.current.height - deltaY, maxHeight)
                    )
                    newX =
                        resizeStart.current.posX +
                        (resizeStart.current.width - newWidth)
                    newY =
                        resizeStart.current.posY +
                        (resizeStart.current.height - newHeight)
                    newX = Math.max(MARGIN + leftMenuWidth, newX)
                    newY = Math.max(MARGIN, newY)
                    break
                case 'n':
                    newHeight = Math.max(
                        MIN_HEIGHT,
                        Math.min(resizeStart.current.height - deltaY, maxHeight)
                    )
                    newY =
                        resizeStart.current.posY +
                        (resizeStart.current.height - newHeight)
                    newY = Math.max(MARGIN, newY)
                    break
                case 's':
                    newHeight = Math.max(
                        MIN_HEIGHT,
                        Math.min(resizeStart.current.height + deltaY, maxHeight)
                    )
                    break
                case 'e':
                    newWidth = Math.max(
                        MIN_WIDTH,
                        Math.min(resizeStart.current.width + deltaX, maxWidth)
                    )
                    break
                case 'w':
                    newWidth = Math.max(
                        MIN_WIDTH,
                        Math.min(resizeStart.current.width - deltaX, maxWidth)
                    )
                    newX =
                        resizeStart.current.posX +
                        (resizeStart.current.width - newWidth)
                    newX = Math.max(MARGIN + leftMenuWidth, newX)
                    break
            }

            // Ensure popup stays within viewport
            newX = Math.max(
                MARGIN + leftMenuWidth,
                Math.min(newX, window.innerWidth - newWidth - MARGIN)
            )
            newY = Math.max(
                MARGIN,
                Math.min(newY, window.innerHeight - newHeight - MARGIN)
            )

            setSize({ width: newWidth, height: newHeight })
            setPosition({ x: newX, y: newY })
        }

        const handleResizeMouseUp = () => {
            setIsResizing({ active: false, handle: null })
        }

        if (isResizing.active) {
            window.addEventListener('mousemove', handleResizeMouseMove)
            window.addEventListener('mouseup', handleResizeMouseUp)
        }

        return () => {
            window.removeEventListener('mousemove', handleResizeMouseMove)
            window.removeEventListener('mouseup', handleResizeMouseUp)
        }
    }, [isResizing, isDesktop, leftMenuVisible])

    return (
        <StyledPaper
            elevation={10}
            style={{
                left: position.x,
                top: position.y,
                width: size.width,
                height: size.height,
            }}
        >
            <DragHandle onMouseDown={handleMouseDown}>
                <DragIndicatorIcon fontSize="small" />
            </DragHandle>
            {onClose && (
                <CloseButtonWrapper>
                    <IconButton size="small" onClick={onClose}>
                        <CloseIcon fontSize="small" />
                    </IconButton>
                </CloseButtonWrapper>
            )}
            <MaximizeVerticalButtonWrapper>
                <IconButton size="small" onClick={handleMaximizeVertical}>
                    {isMaximizedVertical ? (
                        <UnfoldLessIcon fontSize="small" />
                    ) : (
                        <UnfoldMoreIcon fontSize="small" />
                    )}
                </IconButton>
            </MaximizeVerticalButtonWrapper>
            <MaximizeHorizontalButtonWrapper>
                <IconButton size="small" onClick={handleMaximizeHorizontal}>
                    {isMaximizedHorizontal ? (
                        <UnfoldLessIcon
                            fontSize="small"
                            sx={{ transform: 'rotate(90deg)' }}
                        />
                    ) : (
                        <UnfoldMoreIcon
                            fontSize="small"
                            sx={{ transform: 'rotate(90deg)' }}
                        />
                    )}
                </IconButton>
            </MaximizeHorizontalButtonWrapper>

            <ResizeHandleNW
                onMouseDown={(e) => handleResizeMouseDown('nw', e)}
            />
            <ResizeHandleN onMouseDown={(e) => handleResizeMouseDown('n', e)} />
            <ResizeHandleNE
                onMouseDown={(e) => handleResizeMouseDown('ne', e)}
            />
            <ResizeHandleE onMouseDown={(e) => handleResizeMouseDown('e', e)} />
            <ResizeHandleSE
                onMouseDown={(e) => handleResizeMouseDown('se', e)}
            />
            <ResizeHandleS onMouseDown={(e) => handleResizeMouseDown('s', e)} />
            <ResizeHandleSW
                onMouseDown={(e) => handleResizeMouseDown('sw', e)}
            />
            <ResizeHandleW onMouseDown={(e) => handleResizeMouseDown('w', e)} />

            <ContentWrapper>{children}</ContentWrapper>
        </StyledPaper>
    )
}
