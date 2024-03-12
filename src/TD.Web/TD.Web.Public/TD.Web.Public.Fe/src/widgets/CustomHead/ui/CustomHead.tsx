import Head from "next/head"
import { ICustomHeadProps } from "../models/ICustomHeadProps"

export const CustomHead = (props: ICustomHeadProps): JSX.Element => {
    return (
        <Head>
            <title>TERMODOM - {props.title ?? `Gips ploče | Fasade | Suva gradnja | Izolacija | Cene`}</title>
            <meta name="viewport" content="width=device-width, initial-scale=1.0"></meta>
        </Head>
    )
}