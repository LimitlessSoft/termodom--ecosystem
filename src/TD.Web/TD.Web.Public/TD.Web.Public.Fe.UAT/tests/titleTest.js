import { PROJECT_URL } from '../constants.js'
import assert from 'assert'

export default {
    beforeExecution: () => {},
    afterExecution: () => {},
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        const title = await driver.getTitle()
        assert.equal(
            title,
            'Gipsane ploče | Fasade | OSB Ploče | Cene | Termodom Online prodavnica'
        )
    },
}
