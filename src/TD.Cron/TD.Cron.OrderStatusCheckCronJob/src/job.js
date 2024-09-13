const { executeJobAsync } = require('td-cron-common-domain-node')
const webDb = require('td-web-common-repository-node').webDb
const orderEntity = require('td-web-common-contracts-node').entities.orderEntity

module.exports = executeJobAsync(async () => {
    const pendingOrdersRes = await webDb.query(
        `SELECT * FROM "${orderEntity.tableName}" WHERE "${orderEntity.fields.status}" = 3`
    )

    const pendingOrders = pendingOrdersRes.rows

    if (!pendingOrders || pendingOrders.length === 0) {
        console.log('No pending orders found')
        return
    }

    console.log(`Found ${pendingOrders.length} pending orders`)

    for (const order of pendingOrders) {
        console.log(`Checking order with ID: ${order.Id}`)
        console.log(`Order status: ${order.Status}`)
        console.log(
            `Order Komercijalno Dok: ${order.KomercijalnoVrDok} - ${order.KomercijalnoBrDok}`
        )
        const payload = await fetch(
            `${process.env.BASE_COMERCIAL_API_URL}/${order.KomercijalnoVrDok}/${order.KomercijalnoBrDok}`
        ).then((res) => res.json())

        if (!payload.vrdokOut || !payload.brdokOut) {
            console.log('Order not found in komercijalno')
            continue
        }

        console.log(
            `Found order in komercijalno. Now checking if it is realized`
        )
        const relatedOrderStatusRes = await fetch(
            `${process.env.BASE_COMERCIAL_API_URL}/${commercialOrderRes.vrdokOut}/${order.brdokOut}`
        ).then((res) => res.json())

        if (
            !relatedOrderStatusRes.flag != 1 ||
            !relatedOrderStatusRes.placen != 1
        ) {
            console.log('Order not realized yet')
            return
        }

        console.log('Order realized. Updating order status')
        await pool.query(
            `UPDATE "${orderEntity.tableName}" SET "${orderEntity.fields.status}" = $1 WHERE "${orderEntity.fields.id}" = $2`,
            [process.env.TAKEN_ORDER_STATUS, order.Id]
        )
        console.log('Order status updated successfully')
    }
})
