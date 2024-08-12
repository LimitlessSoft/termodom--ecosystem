import webdriver, { Capabilities } from 'selenium-webdriver'

export const createDriver = async () => await new webdriver.Builder().withCapabilities(Capabilities.firefox()
        .set("acceptInsecureCerts", true)).build()
