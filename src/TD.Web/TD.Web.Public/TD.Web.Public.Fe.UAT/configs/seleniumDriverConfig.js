import { Builder, Capabilities } from 'selenium-webdriver'

const getCaps = (browser) => {
    let caps

    if (browser === 'firefox') {
        caps = Capabilities.firefox()
    } else if (browser === 'chrome') {
        caps = Capabilities.chrome()
    } else {
        throw new Error(`Unsupported browser: ${browser}`)
    }
    caps.set('acceptInsecureCerts', true)
    return caps
}

export const createDriver = () => {
    const { BROWSER, SELENIUM_SERVER } = process.env

    const seleniumServer = SELENIUM_SERVER || 'selenium'
    return new Builder()
        .usingServer(`http://${seleniumServer}:4444`)
        .withCapabilities(getCaps(BROWSER))
        .forBrowser(BROWSER)
        .build()
}
