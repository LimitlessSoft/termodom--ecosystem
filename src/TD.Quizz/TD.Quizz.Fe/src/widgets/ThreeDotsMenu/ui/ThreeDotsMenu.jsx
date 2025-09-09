import { MoreVert } from '@mui/icons-material'
import { IconButton, Menu, MenuItem } from '@mui/material'
import React, { useState } from 'react'

export default function ThreeDotsMenu({ options }) {
    const [anchorEl, setAnchorEl] = useState(null)
    const open = Boolean(anchorEl)

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget)
    }

    const handleClose = () => {
        setAnchorEl(null)
    }

    return (
        <>
            <IconButton
                id="three-dots-button"
                aria-controls={open ? 'three-dots-menu' : undefined}
                aria-haspopup="true"
                aria-expanded={open ? 'true' : undefined}
                onClick={handleClick}
            >
                <MoreVert />
            </IconButton>
            <Menu
                id="three-dots-menu"
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                slotProps={{
                    list: {
                        'aria-labelledby': 'three-dots-button',
                    },
                }}
            >
                {options.map((option, index) => (
                    <MenuItem
                        key={index}
                        onClick={() => {
                            option.onClick?.()
                            handleClose()
                        }}
                    >
                        {option.label}
                    </MenuItem>
                ))}
            </Menu>
        </>
    )
}
