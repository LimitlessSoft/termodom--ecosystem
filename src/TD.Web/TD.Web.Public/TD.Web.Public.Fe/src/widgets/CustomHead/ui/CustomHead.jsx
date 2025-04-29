import {
    DefaultMetadataDescription,
    DefaultMetadataTitle,
} from '@/app/constants'
import { NextSeo, ProductJsonLd } from 'next-seo'

export const CustomHead = (props) => {
    const additionalMetaTags = [
        {
            name: `viewport`,
            content: `width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0`,
        },
        {
            name: `google-site-verification`,
            content: `Q-DHI0_kFx4L_d4CLO09MBzIYrRbvCpQRebNsi8zQxU`,
        },
        {
            name: `og:image`,
            content: `/termodom_logo.svg`,
        },
        {
            name: `og:image:width`,
            content: `200`,
        },
        {
            name: `og:image:height`,
            content: `200`,
        },
        {
            name: `keywords`,
            content: `gips ploče, fasade, suva gradnja, izolacija, cene`,
        },
        {
            name: `og:site_name`,
            content: props.title ?? DefaultMetadataTitle,
        },
    ]

    if (props.structuredData?.offers?.[0]) {
        additionalMetaTags.push(
            {
                property: 'og:type',
                content: 'product',
            },
            {
                property: 'og:product:price:amount',
                content: props.structuredData.offers[0].price,
            },
            {
                property: 'og:product:price:currency',
                content: props.structuredData.offers[0].priceCurrency,
            }
        )
    }

    return (
        <>
            <NextSeo
                title={props.title ?? DefaultMetadataTitle}
                description={props.description ?? DefaultMetadataDescription}
                additionalMetaTags={additionalMetaTags}
                additionalLinkTags={[
                    {
                        rel: `shortcut icon`,
                        href: `/termodom_logo.svg`,
                    },
                ]}
            />
            {props.structuredData && (
                <ProductJsonLd {...props.structuredData} />
            )}
        </>
    )
}
