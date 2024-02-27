import { Grid, styled } from "@mui/material";
import { KolicinaInputFieldButton } from "./KolicinaInputFieldButton";
import { useEffect, useState } from "react";

export const KolicinaInputFieldWrapper = (props: any): JSX.Element => {

    const [isLastComma, setIsLastComma] = useState(false)
    const [value, setValue] = useState<string>("0")

    useEffect(() => {
        setValue(isLastComma ? props.value + '.' : props.value ?? 0)
    }, [props.value, isLastComma])

    return (
        <Grid container>
            <Grid item
                style={{
                    width: `80%`
                }}>
                <KolicinaInputFieldStyled
                value={value}
                onKeyDown={(e) => {

                    if(e.code === 'NumpadDecimal' ||
                        e.code === 'Period')
                        {
                            if(isLastComma || props.value.toString().includes('.'))
                                e.preventDefault()
                            return
                        }

                    if(e.key === 'Backspace' ||
                        e.key === 'Delete')
                        {
                            if(props.value.toString().length === 1 && !isLastComma)
                            {
                                props.onValueChange(0)
                                e.preventDefault()
                            }
                            return
                        }

                    if(e.key === 'ArrowLeft' ||
                        e.key === 'ArrowRight' ||
                        e.key === 'ArrowUp' ||
                        e.key === 'ArrowDown')
                        return

                    if(!isFinite(parseFloat(e.key)))
                        e.preventDefault()
                }}
                onChange={(e) => {
                    var val = e.target.value

                    if(val[val.length - 1] === '.') {
                        props.onValueChange(parseFloat(val))
                        setIsLastComma(true)
                        return
                    }

                    if(props.onValueChange === undefined)
                        return

                    props.onValueChange(parseFloat(val))
                    setIsLastComma(false)
                }}/>
            </Grid>
            <Grid item
                style={{
                    width: `20%`
                }}>
                <Grid container direction={`column`}
                    sx={{
                        textAlign: 'center',
                        height: `80px`
                    }}>
                        <KolicinaInputFieldButton text={'+'} onClick={() => {
                            props.onPlusClick()
                        }} />
                        <KolicinaInputFieldButton text={'-'} onClick={() => {
                            props.onMinusClick()
                        }}/>
                </Grid>
            </Grid>
        </Grid>
    )
}

const KolicinaInputFieldStyled = styled(`input`)(
    ({ theme }) => `
        width: 100%;
        height: calc(100% - 2px);
        padding: 0;
        margin: 0;
        border: 1px solid gray;
        text-align: center;
        font-size: 1.5rem;

        &:focus {
            outline: none;
        }
    `)