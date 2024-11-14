import { styled, Grid } from '@mui/material'

export const LayoutLeftMenuStyled = styled(Grid)(
    ({ theme, isMobileMenuExpanded }) => `
      background-color: ${theme.palette.primary.main};
      color: ${theme.palette.primary.contrastText};
      z-index: 9999;
      height: 100vh;
      position: fixed;
      display: flex;
      flex-direction: column;
      transition: width 0.3s ease;
  
      .nav-label {
        opacity: 0;
        visibility: hidden;
        white-space: nowrap;
        transform: translateX(-10px);
        transition: opacity 0.3s ease, transform 0.3s ease, visibility 0s 0.3s;
      }
          
      @media screen and (min-width: ${theme.breakpoints.values.md}px) {
        width: 56px;

        &:hover {
          width: 250px;
  
          .nav-label {
            visibility: visible;
            opacity: 1;
            transform: translateX(0);
            transition: opacity 0.3s ease, transform 0.3s ease, visibility 0s 0s;
          }
        }
      }

      @media screen and (max-width: ${theme.breakpoints.values.md}px) {
        width: 0;
        align-items: center;
        justify-content: center;
        width: ${isMobileMenuExpanded && '100vw'};

        .nav-label {
            visibility: ${isMobileMenuExpanded && 'visible'};
            opacity: ${isMobileMenuExpanded && '1'};
            transform: ${isMobileMenuExpanded && 'translateX(0)'};
            transition: ${isMobileMenuExpanded && 'opacity 0.3s ease, transform 0.3s ease, visibility 0s 0s'};
          }
      }
    `
)
