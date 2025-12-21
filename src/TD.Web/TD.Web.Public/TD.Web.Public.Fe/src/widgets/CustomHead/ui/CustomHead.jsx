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
            property: `og:image`,
            content: `https://www.termodom.rs/Termodom_Logo.png`,
        },
        {
            property: `og:image:width`,
            content: `500`,
        },
        {
            property: `og:image:height`,
            content: `500`,
        },
        {
            property: `og:image:type`,
            content: `image/png`,
        },
        {
            name: `keywords`,
            content: `gips ploče, fasade, suva gradnja, izolacija, cene`,
        },
        {
            property: `og:site_name`,
            content: props.title ?? DefaultMetadataTitle,
        },
    ]

    if (props.structuredData?.offers) {
        let offer = null

        if (
            Array.isArray(props.structuredData.offers) &&
            props.structuredData.offers.length > 0
        ) {
            offer = props.structuredData.offers[0]
        } else if (
            typeof props.structuredData.offers === 'object' &&
            !Array.isArray(props.structuredData.offers) &&
            Object.keys(props.structuredData.offers).length > 0
        ) {
            offer = props.structuredData.offers
        }

        if (offer) {
            additionalMetaTags.push(
                {
                    property: 'og:type',
                    content: 'product',
                },
                {
                    property: 'product:price:amount',
                    content: offer.price,
                },
                {
                    property: 'product:price:currency',
                    content: offer.priceCurrency,
                }
            )
        }
    }

    return (
        <>
            <NextSeo
                title={props.title ?? DefaultMetadataTitle}
                description={props.description ?? DefaultMetadataDescription}
                additionalMetaTags={additionalMetaTags}
                additionalLinkTags={[
                    {
                        rel: `icon`,
                        href: `/favicon.ico`,
                        sizes: 'any',
                    },
                    {
                        rel: 'icon',
                        href: '/termodom_logo.svg',
                        type: 'image/svg+xml',
                    },
                    {
                        rel: 'apple-touch-icon',
                        href: '/Termodom_Logo.png',
                    }
                ]}
            />
            {props.structuredData && (
                <ProductJsonLd {...props.structuredData} />
            )}
        </>
    )
}
