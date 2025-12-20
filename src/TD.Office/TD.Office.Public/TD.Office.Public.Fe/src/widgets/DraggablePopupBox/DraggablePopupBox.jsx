import React, { useState, useEffect, useRef, useCallback } from 'react'
import { Box, Paper, IconButton, useTheme, useMediaQuery } from '@mui/material'
import DragIndicatorIcon from '@mui/icons-material/DragIndicator'
import CloseIcon from '@mui/icons-material/Close'
import { styled } from '@mui/material/styles'
import { useZLeftMenuVisible } from '../../zStore'

const StyledPaper = styled(Paper)(({ theme }) => ({
    position: 'fixed',
    width: '50vw',
    height: '50vh',
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
    top: 8,
    left: 8,
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
    top: 4,
    right: 4,
    zIndex: 1001,
})

const ContentWrapper = styled(Box)({
    flex: 1,
    overflow: 'auto',
    padding: '32px 16px 16px 16px', // Extra top padding to avoid overlapping with drag icon
    display: 'block',
    width: '100%',
    height: '100%',
})

export const DraggablePopupBox = ({ children, onClose }) => {
    const theme = useTheme()
    const isDesktop = useMediaQuery(theme.breakpoints.up('md'))
    const leftMenuVisible = useZLeftMenuVisible()

    const [position, setPosition] = useState({ x: 0, y: 0 })
    const [isDragging, setIsDragging] = useState(false)
    const dragStart = useRef({ x: 0, y: 0 })
    const isFirstRender = useRef(true)
    const MARGIN = 16

    const calculateBoundaries = useCallback(() => {
        const width = window.innerWidth / 2
        const height = window.innerHeight / 2
        const leftMenuWidth = isDesktop && leftMenuVisible ? 64 : 0

        return {
            minX: MARGIN + leftMenuWidth,
            minY: MARGIN,
            maxX: window.innerWidth - width - MARGIN,
            maxY: window.innerHeight - height - MARGIN,
        }
    }, [isDesktop, leftMenuVisible])

    useEffect(() => {
        const handleResize = () => {
            setPosition((prev) => {
                const { minX, minY, maxX, maxY } = calculateBoundaries()
                return {
                    x: Math.max(minX, Math.min(prev.x, maxX)),
                    y: Math.max(minY, Math.min(prev.y, maxY)),
                }
            })
        }

        if (isFirstRender.current) {
            const { maxX, maxY } = calculateBoundaries()
            setPosition({ x: maxX, y: maxY })
            isFirstRender.current = false
        } else {
            handleResize()
        }

        window.addEventListener('resize', handleResize)
        return () => window.removeEventListener('resize', handleResize)
    }, [calculateBoundaries])

    const handleMouseDown = (e) => {
        setIsDragging(true)
        dragStart.current = {
            x: e.clientX - position.x,
            y: e.clientY - position.y,
        }
        e.preventDefault() // Prevent text selection
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

    return (
        <StyledPaper
            elevation={10}
            style={{
                left: position.x,
                top: position.y,
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
            <ContentWrapper>{children}</ContentWrapper>
        </StyledPaper>
    )
}
