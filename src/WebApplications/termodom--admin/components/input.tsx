import { useState } from "react";
import Button from "./button";
import LoadAnim from "./loadAnim";

export default function Input({ labelText, type, placeholder,
    required, forwardRef, readOnly, defaultValue,
    onKeyUp,
    onKeyDown,
    buttonText,
    buttonOnClick,
    buttonShowOnlyOnChange }: any) {

    const [pendingSave, setPendingSave] = useState(false)
    const [buttonDisabled, setButtonDisabled] = useState(false)

    return (
        <div className={``}>
            { required ? "*" : ""}
            { labelText ? <label className={``}>{labelText}</label> : "" }
            <input disabled={readOnly}
                ref={forwardRef}
                className={`border-solid border-2 border-slate-200 p-1 m-1`}
                type={type}
                defaultValue={defaultValue}
                placeholder={placeholder}
                onKeyUp={(event) => {
                    onKeyUp(event)
                    setPendingSave(true)
                }}
                onKeyDown={(event) => {
                }}/>
            {
                buttonText && buttonOnClick && (!buttonShowOnlyOnChange || pendingSave) ?
                <Button
                    text={buttonText}
                    disabled={buttonDisabled}
                    onClick={(event: MouseEvent) => {
                    buttonOnClick(event, setButtonDisabled)
                }} /> : ""
            }
        </div>
    )
}