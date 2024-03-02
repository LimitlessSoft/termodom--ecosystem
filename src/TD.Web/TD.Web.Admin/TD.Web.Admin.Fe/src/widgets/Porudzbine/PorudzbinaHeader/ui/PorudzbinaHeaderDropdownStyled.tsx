import { TextField, styled } from "@mui/material";

export const PorudzbinaHeaderDropdownStyled = styled(TextField)(
    ({ theme }) => `

        p {
            color: white;
        }

        .MuiInputBase-input {
            color: white;
            background-color: ${theme.palette.primary.main};
        }

        label {
            color: white;
        }

        .Mui-focused {
            color: orange !important;
            border-color: orange !important;
        }

        fieldset {
            border-color: white;
        }

        .Mui-focused fieldset {
            border-color: orange !important;
        }

        svg {
            color: white;
        }

        &:hover {
            fieldset {
                border-color: orange !important;
            }
        }
    `
)