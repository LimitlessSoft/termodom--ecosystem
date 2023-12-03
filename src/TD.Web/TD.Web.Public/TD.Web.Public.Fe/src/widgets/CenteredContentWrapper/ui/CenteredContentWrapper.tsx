import { Box } from "@mui/material"
import { ReactNode } from "react";

interface ICenteredContentWrapperProps {
    children: ReactNode;
}

export const CenteredContentWrapper = (props: ICenteredContentWrapperProps): JSX.Element => {
    const { children } = props
    return (
        <Box
            sx={{
                maxWidth: '1100px',
                minHeight: '100vh',
                margin: 'auto' }}>
            { children }
        </Box>
    )
}