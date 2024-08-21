import webdriver, { Builder, Capabilities } from 'selenium-webdriver'
import Chrome from 'selenium-webdriver/chrome.js'
import Firefox from 'selenium-webdriver/firefox.js'

const BROWSER = process.env.BROWSER || 'firefox'
const ENV = process.env.ENV || 'github-action'
const SELENIUM_SERVER = process.env.SELENIUM_SERVER || 'selenium'

const createLocalDriver = () => {
    if (BROWSER === 'firefox') {
        let options = new Firefox.Options()
        return new webdriver.Builder()
            .withCapabilities(Capabilities.firefox().set("acceptInsecureCerts", true))
            .setFirefoxOptions(options)
            .build()
    } else if (BROWSER === 'chrome') {
        let options = new Chrome.Options()
        return new webdriver.Builder()
            .withCapabilities(Capabilities.chrome().set("acceptInsecureCerts", true))
            .setChromeOptions(options)
            .build()
    } else {
        throw new Error('Unsupported browser')
    }
}

const createRemoteDriver = () => {
    const seleniumServer = SELENIUM_SERVER || 'selenium'
    return new Builder()
        .usingServer(`http://${seleniumServer}:4444`)
        .withCapabilities(getCaps())
        .forBrowser(BROWSER)
        .build();
}

const getCaps = () => {
    let caps = Capabilities.firefox()
    caps.set("acceptInsecureCerts", true)
    
    return caps
}

export const createDriver = () => {
    if (ENV === 'local') {
        return createLocalDriver()
    }
    else if (ENV === 'github-action') {
        return createRemoteDriver()
    }
    else {
        throw new Error('Unsupported environment')
    }
}