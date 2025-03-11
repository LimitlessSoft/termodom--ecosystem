import { Builder, Capabilities } from 'selenium-webdriver'

const getCaps = (browser) => {
    let caps

    if (browser === 'firefox') {
        caps = Capabilities.firefox()
        // caps.set('moz:firefoxOptions', { args: ['-headless'] })
    } else if (browser === 'chrome') {
        caps = Capabilities.chrome()
        // caps.set('goog:chromeOptions', {
        //     args: ['--headless', '--no-sandbox', '--disable-dev-shm-usage'],
        // })
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
