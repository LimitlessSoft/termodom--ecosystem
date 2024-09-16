const orderEntity = require('td-web-common-contracts-node/src/entities/orderEntity')
const ORDER_STATUS = require('td-web-common-contracts-node/src/enums/orderStatus')

const webDb = require('td-web-common-repository-node').webDb

const orderManager = {
    getPendingOrdersAsync: async () => {
        const pendingOrdersRes = await webDb.query(
            `SELECT * FROM "${orderEntity.tableName}" WHERE "${orderEntity.columns.isActive}" = true AND "${orderEntity.columns.status}" = ${ORDER_STATUS.PENDING}`
        )

        return pendingOrdersRes.rows
    },
    markOrderAsRealizedAsync: async (orderId) => {
        await webDb.query(
            `UPDATE "${orderEntity.tableName}" SET "${orderEntity.columns.status}" = $1 WHERE "${orderEntity.columns.id}" = $2`,
            [ORDER_STATUS.REALIZED, orderId]
        )
    }
}

module.exports = orderManager