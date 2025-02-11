import { PROJECT_URL } from '../constants.js'
import assert from 'assert'

export default async (driver) => {
    await driver.get(PROJECT_URL)
    
    const title = await driver.getTitle()
    await assert.equal(title, 'Gipsane ploče | Fasade | OSB Ploče | Cene | Termodom Online prodavnica')
}
