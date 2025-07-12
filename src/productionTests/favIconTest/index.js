import { DOMParser } from 'xmldom'
import pLimit from 'p-limit'
import fs from 'fs'
const sliceSize = 100
const localIconPath = 'termodom_logo.svg'
const localSliceBase64 = Buffer.from(fs.readFileSync(localIconPath))
    .toString('base64')
    .slice(0, sliceSize)
const failedOnes = []
const sitemap = 'https://termodom.rs/sitemap.xml'
async function Test(url) {
    const response = await fetch(url)
    // using regex check if page contains link with rel containing "icon"
    const text = await response.text()
    const iconLinkRegex = /<link[^>]+rel=["']?[^"'>]*icon[^"'>]*["']?[^>]*>/i
    const iconLinkMatch = text.match(iconLinkRegex)
    if (!iconLinkMatch) {
        const msg = `No icon link found in ${url}`
        failedOnes.push({
            url,
            message: msg,
        })
        console.log(msg)
        return
    }
    const iconLink = iconLinkMatch[0]
    const hrefRegex = /href=["']([^"']+)["']/i
    const hrefMatch = iconLink.match(hrefRegex)
    if (!hrefMatch) {
        const msg = `No href found in icon link for ${url}`
        failedOnes.push({
            url,
            message: msg,
        })
        console.log(msg)
        return
    }
    const iconUrl = 'https://termodom.rs' + hrefMatch[1]
    const iconResponse = await fetch(iconUrl)
    if (!iconResponse.ok) {
        const msg = `Failed to fetch icon from ${iconUrl}: ${iconResponse.status} ${iconResponse.statusText}`
        failedOnes.push({
            url,
            message: msg,
        })
        return
    }
    const iconBlob = await iconResponse.blob()
    const iconArrayBuffer = await iconBlob.arrayBuffer()
    const iconUint8Array = new Uint8Array(iconArrayBuffer)
    const sliceBase64 = Buffer.from(iconUint8Array)
        .toString('base64')
        .slice(0, sliceSize)
    if (sliceBase64 !== localSliceBase64) {
        const msg = `Icon from ${url} does not match local icon slice.`
        failedOnes.push({
            url,
            message: msg,
        })
        console.log(msg)
    } else {
        console.log(`Icon from ${url} matches local icon slice.`)
    }
}
async function Start() {
    const response = await fetch(sitemap)
    const rawXMLString = await response.text()
    const parser = new DOMParser()
    const parsedDocument = parser.parseFromString(
        rawXMLString,
        'application/xml'
    )
    const locElements = Array.from(parsedDocument.getElementsByTagName('loc'))
    const urls = []
    locElements.forEach((e) => urls.push(e.textContent.trim()))

    console.log('Found pages:', urls.length)

    const limit = pLimit(100)
    const tasks = urls.map((url) => limit(() => Test(url)))
    await Promise.all(tasks)
}
const sb = []
sb.push(new Date())
Start().then(() => {
    console.log('Finished checking icons.')
    if (failedOnes.length > 0) {
        console.log('Failed on the following pages:')
        failedOnes.forEach((e) => {
            console.log(`- ${e.url}: ${e.message}`)
        })
    } else {
        console.log('All icons matched successfully.')
    }
    console.log(`// Execution time:`)
    // --
    sb.push(new Date())
    console.log(
        `Execution time: ${((sb[1] - sb[0]) / 1000).toFixed(2)} s, Pages checked: ${sb.length - 2}`
    )
    if (failedOnes.length > 0) {
        process.exit(1)
    }
})
