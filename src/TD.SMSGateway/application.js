// Settings
const BULK_SMS_COUNT = 3
// ======

const process = require('process')
const serialPortGSM = require('serialport-gsm')
const Firebird = require('node-firebird')

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

const options = {
	host: '4monitor',
	port: 3050,
	database: "C:/LimitlessSoft/AdvancedGateway/AG.FDB",
	user: "SYSDBA",
	password: "m",
	pageSize: 4096,
	encoding: "UTF-8"
}

const queueSms = (remainingSms, db, onAllSmsSent) => {
	let sms = remainingSms.pop()
	let firstCallback = true

	if(sms == null)
	{
		onAllSmsSent()
		return
	}

	modem.sendSMS(sms.MOBILE, sms.TEXT, false, (e) => {

		if(firstCallback)
		{
			firstCallback = false
			return
		}

		db.query("UPDATE SMS SET STATUS = " + (e.status == 'success' ? 1 : 7)
			+ " WHERE ID = " + sms.ID, (err, result) => {
			if(err)
				throw err

			if(remainingSms.length == 0 && onAllSmsSent != null)
			{
				onAllSmsSent()
				return
			}
			queueSms(remainingSms, db, onAllSmsSent)
		})
	})
}

const prepareSmsToSend = (db) => {
	// Select first last 10 SMSs pending to be sent
	db.query("SELECT FIRST " + BULK_SMS_COUNT
		+ " * FROM SMS WHERE STATUS = 2 ORDER BY ID DESC", function(err, result) {

		if(err)
			throw err

		if(result.length == 0)
		{
			setTimeout(() => {
				prepareSmsToSend(db)
			}, 3000)
			return
		}

		queueSms(result, db, () => { prepareSmsToSend(db) })

	})
}

modem.on('open', data => {
	modem.initializeModem()

	try
	{
		Firebird.attach(options, function(err, db) {
			if(err)
				throw err

			prepareSmsToSend(db)

		})
	}
	catch(ex)
	{
		console.error(ex)
		modem.close()
		db.detach()
	}
})
