import { By, until } from 'selenium-webdriver'
import { PROJECT_URL, WAIT_TIMEOUT } from '../constants'
import assert from 'assert'
import { generateRandomCharacters } from '../helpers/generateRandomCharacters'

export default async (driver) => {
    await driver.get(PROJECT_URL)

    const profiKutakButtonLocator = By.xpath(`//*[@id="header-wrapper"]/a[4]`)
    await driver.wait(
        until.elementLocated(profiKutakButtonLocator),
        WAIT_TIMEOUT
    )
    const profiKutakButton = await driver.findElement(profiKutakButtonLocator)
    await profiKutakButton.click()

    const postaniProfiKupacButtonLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div/a'
    )
    await driver.wait(
        until.elementLocated(postaniProfiKupacButtonLocator),
        WAIT_TIMEOUT
    )
    const postaniProfiKupacButton = await driver.findElement(
        profiKutakButtonLocator
    )
    await postaniProfiKupacButton.click()

    const fullNameAndSurnameInputLocator = By.xpath('//*[@id="nickname"]')
    await driver.wait(
        until.elementLocated(fullNameAndSurnameInputLocator),
        WAIT_TIMEOUT
    )
    const fullNameAndSurnameInput = await driver.findElement(
        fullNameAndSurnameInputLocator
    )
    await fullNameAndSurnameInput.sendKeys(generateRandomCharacters('string'))

    const usernameInputLocator = By.xpath('//*[@id="username"]')
    await driver.wait(until.elementLocated(usernameInputLocator), WAIT_TIMEOUT)
    const usernameInput = await driver.findElement(usernameInputLocator)
    await usernameInput.sendKeys(generateRandomCharacters('string'))

    const randomPassword = generateRandomCharacters('string')

    const passwordInputLocator = By.xpath('//*[@id="password1"]')
    await driver.wait(until.elementLocated(passwordInputLocator), WAIT_TIMEOUT)
    const passwordInput = await driver.findElement(passwordInputLocator)
    await passwordInput.sendKeys(randomPassword)

    const confirmPasswordInputLocator = By.xpath('//*[@id="password2"]')
    await driver.wait(
        until.elementLocated(confirmPasswordInputLocator),
        WAIT_TIMEOUT
    )
    const confirmPasswordInput = await driver.findElement(
        confirmPasswordInputLocator
    )
    await confirmPasswordInput.sendKeys(randomPassword)

    const mobilePhoneInputLocator = By.xpath('//*[@id="mobile"]')
    await driver.wait(
        until.elementLocated(mobilePhoneInputLocator),
        WAIT_TIMEOUT
    )
    const mobilePhoneInput = await driver.findElement(mobilePhoneInputLocator)
    await mobilePhoneInput.sendKeys(generateRandomCharacters('number'))

    const addressInputLocator = By.xpath('//*[@id="address"]')
    await driver.wait(until.elementLocated(addressInputLocator))
    const addressInput = await driver.findElement(addressInputLocator)
    await addressInput.sendKeys(generateRandomCharacters('address'))

    const placeOfResidenceInputLocator = By.xpath('//*[@id="cityId"]')
    await driver.wait(
        until.elementLocated(placeOfResidenceInputLocator),
        WAIT_TIMEOUT
    )
    const placeOfResidenceInput = await driver.findElement(
        placeOfResidenceInputLocator
    )
    await placeOfResidenceInput.click()

    const placeOfResidenceFirstOptionLocator = By.xpath('//*[@id=":ri:"]/li[1]')
    await driver.wait(
        until.elementLocated(placeOfResidenceFirstOptionLocator),
        WAIT_TIMEOUT
    )
    const placeOfResidenceFirstOption = await driver.findElement(
        placeOfResidenceFirstOptionLocator
    )
    await placeOfResidenceFirstOption.click()

    const favoriteStoreInputLocator = By.xpath('//*[@id="favoriteStoreId"]')
    await driver.wait(
        until.elementLocated(favoriteStoreInputLocator),
        WAIT_TIMEOUT
    )
    const favoriteStoreInput = await driver.findElement(
        favoriteStoreInputLocator
    )
    await favoriteStoreInput.click()

    const favoriteStoreSecondOptionLocator = By.xpath('//*[@id=":rk:"]/li[2]')
    await driver.wait(
        until.elementLocated(favoriteStoreSecondOptionLocator),
        WAIT_TIMEOUT
    )
    const favoriteStoreSecondOption = await driver.findElement(
        favoriteStoreSecondOptionLocator
    )
    await favoriteStoreSecondOption.click()

    const emailInputLocator = By.xpath(`//*[@id="email"]`)
    await driver.wait(until.elementLocated(emailInputLocator), WAIT_TIMEOUT)
    const emailInput = await driver.findElement(emailInputLocator)
    await emailInput.sendKeys(generateRandomCharacters('email'))

    const requestRegistrationLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div/button'
    )
    await driver.wait(
        until.elementLocated(requestRegistrationLocator),
        WAIT_TIMEOUT
    )
    const requestResgistration = await driver.findElement(
        requestRegistrationLocator
    )
    await requestResgistration.click()

    //dovrsiti sta dalje
}
