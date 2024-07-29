import { Grid, Typography } from '@mui/material'
import { IEnchantedTextFieldProps } from '../interfaces/IEnchantedTextFieldProps'
import { EnchantedTextFieldStyled } from '../styled/EnchantedTextFieldStyled'
import { useState } from 'react'
import { formatNumber } from '@/helpers/numberHelpers'

export const EnchantedTextField = (props: IEnchantedTextFieldProps) => {
    
    if (props.value !== undefined && props.allowDecimal)
        throw new Error(`EnchantedTextField: You can't use 'value' and 'allowDecimal' at the same time. In order to use 'allowDecimal', you must use 'defaultValue' instead of 'value'.`)
    
    if (props.formatValue && props.inputType !== `number`)
        throw new Error(`EnchantedTextField: You can't use 'formatValue' without 'inputType' set to 'number'.`)
    
    const [currentValue, setCurrentValue] = useState(props.defaultValue?.toString() ?? '')
    const [isLastCharacterDecimal, setIsLastCharacterDecimal] = useState(false)
    
    const handleAllowOnlyNumbers = (
        event: React.KeyboardEvent<HTMLInputElement>
    ) => {
        if (
            event.key === 'Backspace' ||
            event.key === 'Delete' ||
            event.key === 'ArrowLeft' ||
            event.key === 'ArrowRight' ||
            event.key === 'ArrowUp' ||
            event.key === 'ArrowDown' ||
            event.key === 'Home' ||
            event.key === 'End' ||
            event.key === '1' ||
            event.key === '2' ||
            event.key === '3' ||
            event.key === '4' ||
            event.key === '5' ||
            event.key === '6' ||
            event.key === '7' ||
            event.key === '8' ||
            event.key === '9' ||
            event.key === '0' ||
            (props.allowDecimal && event.key === '.') ||
            (props.allowDecimal && event.key === ',')
        )
            return

        event.preventDefault()
    }
    
    const handleDecimal = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (!props.allowDecimal)
            return
        
        if (event.key !== '.' && event.key !== ',')
            return
        
        if (currentValue.indexOf('.') !== -1 || currentValue.indexOf(',') !== -1)
            event.preventDefault()
        
        if(isLastCharacterDecimal)
            event.preventDefault()
    }
    
    const getValueFormatted = () => {
        if (props.inputType === `number` && props.formatValue)
            return `${props.formatValuePrefix ?? ''}${formatNumber(+(props.value ?? 0))}${props.formatValueSuffix ?? ''}`
        return props.value
    }
    
    return (
        <Grid item>
            <EnchantedTextFieldStyled
                textalignment={props.textAlignment}
                fullWidth={props.fullWidth}
                onFocus={(e) => {
                    e.currentTarget.select()
                }}
                readOnly={props.readOnly}
                disabled={props.readOnly}
                variant={props.variant}
                label={props.label}
                value={props.defaultValue === undefined ? getValueFormatted() : currentValue}
                onKeyDown={(event: React.KeyboardEvent<HTMLInputElement>) => {
                    if (props.inputType === `number`) handleAllowOnlyNumbers(event)
                    if (props.inputType === `number`) handleDecimal(event)
                }}
                onChange={(event) => {
                    const ilcd = event.target.value[event.target.value.length - 1] === '.' || event.target.value[event.target.value.length - 1] === ','
                    setIsLastCharacterDecimal(ilcd)

                    let val = event.target.value.length === 0
                        ? '0'
                        : !ilcd
                            && props.inputType === `number`
                            && event.target.value[0] === `0`
                            && event.target.value[1] !== '.'
                            && event.target.value[1] !== ','
                            && event.target.value.length > 1
                                ? event.target.value.slice(1)
                                : event.target.value
                    
                    setCurrentValue(val)
                    
                    if (ilcd) return

                    if (!props.onChange) return
                    props.onChange(val)
                }}
            />
            {props.subLabel && <Typography textAlign={`right`}>{props.subLabelPrefix}{
                props.inputType === `number`
                    ? formatNumber(+props.subLabel)
                    : props.subLabel
            }{props.subLabelSuffix}</Typography>}
        </Grid>
    )
}
