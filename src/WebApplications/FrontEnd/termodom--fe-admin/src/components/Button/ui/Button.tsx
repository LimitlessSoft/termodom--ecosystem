import { PropsWithChildren } from 'react'
import styles from './Button.module.css'

type Props = {
    text: string,
    callback?: () => void
}

export const Button = (props: PropsWithChildren<Props>): JSX.Element => {
    return (
        <div
        className={`
        ${styles.buttonWrapper}
        inline
        m-1
        px-3
        py-1
        duration-100`}
        onClick={() => {
            if(props.callback != null)
                props.callback()
        }}>
            { props.text }
        </div>
    )
}