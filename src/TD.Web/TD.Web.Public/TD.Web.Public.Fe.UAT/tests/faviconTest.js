import { PROJECT_URL } from '../constants.js'
import { DOMParser } from 'xmldom'
import fs from 'fs'

export default {
    beforeExecution: async () => {},
    afterExecution: async () => {},
    execution: async (driver) => {
        const pocetak = new Date()
        const sitemapResponse = await fetch(`https://termodom.rs/sitemap.xml`)
        const rawXMLString = await sitemapResponse.text()
        const parsedDocument = new DOMParser().parseFromString(
            rawXMLString,
            'application/xml'
        )
        const locElements = Array.from(
            parsedDocument.getElementsByTagName('loc')
        )
        const pageUrls = locElements.map((e) => e.textContent.trim())

        const localSliceBase64 = fs
            .readFileSync('assets/termodom_logo.svg')
            .toString('base64')

        function compareFaviconWithLocal(expectedSliceBase64, done) {
            async function loadImage(src) {
                return new Promise((resolve, reject) => {
                    const img = new Image()
                    img.crossOrigin = 'anonymous'
                    img.onload = () => resolve(img)
                    img.onerror = () => reject('Failed to load ' + src)
                    img.src = src
                })
            }

            function getTopLeft10x10PixelImageData(img) {
                const canvas = document.createElement('canvas')
                canvas.width = 10
                canvas.height = 10
                const ctx = canvas.getContext('2d')

                ctx.drawImage(img, 0, 0, 10, 10)
                return ctx.getImageData(0, 0, 10, 10).data
            }

            function arePixelDataDifferent(data1, data2) {
                for (let i = 0; i < data1.length; i++) {
                    if (data1[i] !== data2[i]) return true
                }
                return false
            }

            ;(async () => {
                try {
                    let faviconUrl = '/favicon.ico'
                    const iconLink = document.querySelector('link[rel~="icon"]')
                    if (iconLink && iconLink.href) faviconUrl = iconLink.href

                    const faviconImg = await loadImage(faviconUrl)
                    const localImg = await loadImage(
                        'data:image/svg+xml;base64,' + expectedSliceBase64
                    )

                    const faviconData =
                        getTopLeft10x10PixelImageData(faviconImg)
                    const localData = getTopLeft10x10PixelImageData(localImg)

                    const isDifferent = arePixelDataDifferent(
                        faviconData,
                        localData
                    )
                    done(isDifferent)
                } catch (e) {
                    console.error('Browser script error:', e)
                    done(-1)
                }
            })()
        }

        const failedPages = []

        for (const pageUrl of pageUrls) {
            console.log('\n🌐 Checking page:', pageUrl)
            await driver.get(pageUrl)

            const isDifferent = await driver.executeAsyncScript(
                compareFaviconWithLocal,
                localSliceBase64
            )

            if (isDifferent === -1) {
                console.log('❌ Could not load favicon or local slice')
                failedPages.push(`${pageUrl} (load error)`)
            } else if (isDifferent === false) {
                console.log('✅ Favicon matches local slice exactly')
            } else if (isDifferent === true) {
                console.log('⚠️ Favicon differs')
                failedPages.push(`${pageUrl} (favicon mismatch)`)
            }
        }

        if (failedPages.length > 0) {
            throw new Error(
                `Favicon validation failed on the following pages:\n- ${failedPages.join(
                    '\n- '
                )}`
            )
        } else {
            console.log('All favicons match the expected image!')
        }
        console.log(pocetak, new Date())
    },
}
