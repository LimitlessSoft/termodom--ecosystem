// Settings
const BULK_SMS_COUNT = 3
// ======

// function
const getSecondsDifference = (startDate, endDate) => {
	const startMS = startDate.getTime()
	const endMS = endDate.getTime()
	return (endMS - startMS) / 1000
}
// =====
const process = require('process')
setTimeout(() => {
	process.exit(0)
}, 58 * 1000)

const serialPortGSM = require('serialport-gsm')
const modem = serialPortGSM.Modem()
const modemOptions = {
	baudRate: 115200,
	dataBits: 8,
	stopBits: 1,
	parity: 'none',
	rtscts: false,
	xon: false,
	xoff: false,
	xany: false,
	autoDeleteOnReceive: true,
	enableConcatenation: true,
	incomingCallIndication: true,
	incomingSMSIndication: true,
	pin: '',
	customInitCommand: '',
	cnmiCommand: 'AT+CNMI=2,1,0,2,1',
	logger: console
}
modem.setModemMode(null, 'SMS')
modem.open('COM3', modemOptions)

const Firebird = require('node-firebird')
const options = {
	host: '4monitor',
	port: 3050,
	database: "C:/LimitlessSoft/AdvancedGateway/AG.FDB",
	user: "SYSDBA",
	password: "m",
	pageSize: 4096,
	encoding: "UTF-8"
}

const queueSms = (remainingSms, db) => {
	const sms = remainingSms.pop()
	let firstCallback = true
	modem.sendSMS(sms.MOBILE, sms.TEXT, false, (e) => {

		if(firstCallback)
		{
			firstCallback = false
			return
		}

		db.query("UPDATE SMS SET STATUS = " + (e.status == 'success' ? 1 : 7) + " WHERE ID = " + sms.ID, (err, result) => {
			if(err)
				throw err
		})

		if(remainingSms.length == 0) {
			modem.close()
			db.detach()
			process.exit(0)
			return
		}

		queueSms(remainingSms, db)
	})
}

modem.on('open', data => {
	modem.initializeModem()

	Firebird.attach(options, function(err, db) {
		if(err)
			throw err

		// Select first last 10 SMSs pending to be sent
		db.query("SELECT FIRST " + BULK_SMS_COUNT + " * FROM SMS WHERE STATUS = 2 ORDER BY ID DESC", function(err, result) {

			if(result.length == 0) {
				modem.close()
				db.detach()
				process.exit(0)
			}

			queueSms(result, db)

			setTimeout(() => { process.exit(0) }, 60 * 1000)
		})
	})
})
