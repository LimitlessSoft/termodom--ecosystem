require("dotenv").config();
const pool = require("db-connection");
const executeJobAsync = require("jobs-common");
const { ORDERS_TABLE_NAME, STATUS_FIELD, ID_FIELD } = require("./constants");

executeJobAsync(async () => {
  const pendingOrdersRes = await pool.query(
    `SELECT * FROM "${ORDERS_TABLE_NAME}" WHERE "${STATUS_FIELD}" = ${process.env.WAITING_COLLECTION_ORDER_STATUS};`
  );

  const pendingOrders = pendingOrdersRes.rows;

  if (!pendingOrders || pendingOrders.length === 0) return;

  for (const order of pendingOrders) {
    const commercialOrderRes = await fetch(
      `${process.env.BASE_COMERCIAL_API_URL}/${order.KomercijalnoVrDok}/${order.KomercijalnoBrDok}`
    ).then((res) => res.json());

    if (!commercialOrderRes.vrdokOut || !commercialOrderRes.brdokOut) return;

    const relatedOrderStatusRes = await fetch(
      `${process.env.BASE_COMERCIAL_API_URL}/${commercialOrderRes.vrdokOut}/${order.brdokOut}`
    ).then((res) => res.json());

    if (!relatedOrderStatusRes.flag == 1 || !relatedOrderStatusRes.placen == 1)
      return;

    await pool.query(
      `UPDATE "${ORDERS_TABLE_NAME}" SET "${STATUS_FIELD}" = $1 WHERE "${ID_FIELD}" = $2`,
      [process.env.TAKEN_ORDER_STATUS, order.Id]
    );
  }
});
