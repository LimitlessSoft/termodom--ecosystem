import { DefaultMetadataDescription, DefaultMetadataTitle } from "@/app/constants"
import { NextSeo } from "next-seo"

export const CustomHead = (props: any): JSX.Element => {

    return <NextSeo
        title={props.title ?? DefaultMetadataTitle}
        description={props.description ?? DefaultMetadataDescription}
        additionalMetaTags={
            [
                {
                    name: `viewport`,
                    content: `width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0`
                },
                {
                    name: `google-site-verification`,
                    content: `Q-DHI0_kFx4L_d4CLO09MBzIYrRbvCpQRebNsi8zQxU`
                },
                {
                    name: `og:image`,
                    content: `/termodom_logo.svg`
                },
                {
                    name: `og:image:width`,
                    content: `200`
                },
                {
                    name: `og:image:height`,
                    content: `200`
                },
                {
                    name: `keywords`,
                    content: `gips ploče, fasade, suva gradnja, izolacija, cene`
                },
                {
                    name: `og:site_name`,
                    content: props.title ?? DefaultMetadataTitle
                }
            ]
        }
        additionalLinkTags={
            [
                {
                    rel: `shortcut icon`,
                    href: `/termodom_logo.svg`
                }
            ]
        } />
}