const { executeJobAsync } = require('td-cron-common-domain-node')
const { orderManager } = require('td-web-common-domain-node')

module.exports = executeJobAsync(async () => {
    const pendingOrders = await orderManager.getPendingOrdersAsync()
    
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
        console.log(`${process.env.BASE_KOMERCIJALNO_API_URL}/${order.KomercijalnoVrDok}/${order.KomercijalnoBrDok}`)
        const payload = await fetch(
            `${process.env.BASE_KOMERCIJALNO_API_URL}/${order.KomercijalnoVrDok}/${order.KomercijalnoBrDok}`
        ).then((res) => res.json())

        if (!payload.vrdokOut || !payload.brdokOut) {
            console.log('Order not found in komercijalno')
            continue
        }

        console.log(
            `Found order in komercijalno. Now checking if it is realized`
        )
        const relatedOrderStatusRes = await fetch(
            `${process.env.BASE_KOMERCIJALNO_API_URL}/${payload.vrdokOut}/${payload.brdokOut}`
        ).then((res) => res.json())
        
        if (
            relatedOrderStatusRes.flag != 1 ||
            relatedOrderStatusRes.placen != 1
        ) {
            console.log('Order not realized yet')
            continue
        }

        console.log('Order realized. Updating order status')
        await orderManager.markOrderAsRealizedAsync(order.Id)
        console.log('Order status updated successfully')
    }
})
