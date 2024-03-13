import { DefaultMetadataTitle } from "@/app/constants"
import Head from "next/head"

export const CustomHead = (props: any): JSX.Element => {

    const defaultDescription = `Termodom Online prodavnica. Gips karton ploče, stiropor, fasade, bavalit, azmafon, stirodur - TERMODOM.RS - Centar građevinskog materijala.`

    return (
        <Head>
            <link rel="shortcut icon" href="/Termodom_Logo.svg" />

            <title>{props.title ?? DefaultMetadataTitle}</title>

            <meta property="og:site_name" content={props.title ?? DefaultMetadataTitle} key={`title`} />
            {/* <meta name="description" content={props.description ?? defaultDescription} /> */}
            <meta name="keywords" content={defaultDescription} />

            <meta name="google-site-verification" content="Q-DHI0_kFx4L_d4CLO09MBzIYrRbvCpQRebNsi8zQxU" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0"></meta>

            <meta property="og:image" content="/termodom-logo-white.svg" />
            <meta property="og:image:width" content="200" />
            <meta property="og:image:height" content="200" />
        </Head>
    )
}