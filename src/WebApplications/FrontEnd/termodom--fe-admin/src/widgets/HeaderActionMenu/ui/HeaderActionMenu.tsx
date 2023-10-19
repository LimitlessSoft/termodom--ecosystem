import { PropsWithChildren } from "react"

export interface IAction {
    callback: () => void,
    text: string
}

type Props = {
    actions?: IAction[]
}

export const HeaderActionMenu = (props: PropsWithChildren<Props>): JSX.Element => {
    return (
        <div>
            {
                props.actions?.map((action, index, array) => {
                    return (
                        <div key={`fafsaf${index}`}>button</div>
                        // <Button
                        // key={`${action.text}-${index}`}
                        // callback={() => {
                        //     action.callback()
                        // }}
                        // text={action.text}/>
                    )
                })
            }
        </div>
    )
}