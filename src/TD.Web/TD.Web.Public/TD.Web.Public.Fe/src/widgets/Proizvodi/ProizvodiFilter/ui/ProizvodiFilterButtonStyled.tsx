import { Grid, styled } from '@mui/material'

export const ProizvodiFilterButtonStyled = styled(Grid)<{}>(
    ({ theme }) =>
        `
        Button {
            background-color: ${theme.palette.primary.main};
            font-family: GothamProMedium;
            width: 100%;
        }

        @media only
            screen and (max-width: 1920px),
            {
                Button {
                    font-size: ${theme.fontSizes?._1920};
                }
            }

        @media only
            screen and (max-width: 1600px),
            {
                Button {
                    font-size: ${theme.fontSizes?._1600};
                }
            }

        @media only
            screen and (max-width: 1336px),
            {
                Button {
                    font-size: ${theme.fontSizes?._1336};
                }
            }
        
        @media only
            screen and (max-width: 960px),
            {
                Button {
                    font-size: ${theme.fontSizes?._960};
                }
            }
        
        @media only
            screen and (max-width: 720px),
            {
                Button {
                    font-size: ${theme.fontSizes?._720};
                }
            }
        
        @media only
            screen and (max-width: 520px),
            {
                Button {
                    font-size: ${theme.fontSizes?._520};
                }
            }

        @media only
            screen and (max-width: 360px),
            {
                Button {
                    font-size: ${theme.fontSizes?._360};
                }
            }
            
        @media only
            screen and (max-width: 260px),
            {
                Button {
                    font-size: ${theme.fontSizes?._260};
                }
            }
    `
)
