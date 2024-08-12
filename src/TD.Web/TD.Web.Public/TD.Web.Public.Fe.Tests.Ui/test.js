import { Browser, Builder } from 'selenium-webdriver'

console.log('hi')
new Builder().forBrowser(Browser.CHROME).build()
    .then(driver => {
        driver.get('https://termodom.rs');
        driver.getTitle().then(title => {
            console.log(title);
        });
        driver.quit
    });