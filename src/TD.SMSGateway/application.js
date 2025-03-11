// Settings
const BULK_SMS_COUNT = 3
// ======

console.log("Starting applicatino at: " + new Date())
const process = require('process')
const serialPortGSM = require('serialport-gsm')
const Firebird = require('node-firebird')

const isMobileValid = (mobile) => {
  if(mobile == null)
    return false

  if(mobile.length == 0)
    return false
  
  if(mobile[0] != '+' && mobile[0] != '0')
    return false

  if(/^\+?\d+$/.test(mobile) == false) // Checks if only numbers
    return false

  return true
}

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
  incomingCallIndication: false,
  incomingSMSIndication: true,
  pin: '',
  customInitCommand: '',
  cnmiCommand: 'AT+CNMI=2,1,0,2,1',
	logger: console
}
modem.setModemMode(null, 'SMS')

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
  console.log("Queueing message")
	let sms = remainingSms.pop()
	let firstCallback = true

	if(sms == null)
	{
    console.log("SMS is null, invoking onAllSmsSent()")
		onAllSmsSent()
		return
  }

  console.log("Trying to send SMS: " + JSON.stringify(sms))
	if(sms.MOBILE == null || sms.TEXT == null || isMobileValid(sms.MOBILE) == false)
	{
    console.log("SMS contains invalid mobile or text. Marking it with error status and continuing")
		db.query("UPDATE SMS SET STATUS = 7 WHERE ID = " + sms.ID, (err, result) => {
			if(err)
				throw err
			if(remainingSms.length == 0 && onAllSmsSent != null)
			{
        console.log("No more SMS to send, invoking onAllSmsSent")
				onAllSmsSent()
				return
      }
			queueSms(remainingSms, db, onAllSmsSent)
		})
  }

  console.log("Sending SMS to modem")
  modem.sendSMS(sms.MOBILE, sms.TEXT, false, (e) => {
		if(firstCallback)
		{
			firstCallback = false
			return
    }

    console.log("SMS sending response received, updating status accordingly")

		db.query("UPDATE SMS SET STATUS = " + (e.status == 'success' ? 1 : 7)
			+ " WHERE ID = " + sms.ID, (err, result) => {
			if(err)
				throw err

			if(remainingSms.length == 0 && onAllSmsSent != null)
			{
        console.log("No more SMS to send, invoking onAllSmsSent")
				onAllSmsSent()
				return
        }
			queueSms(remainingSms, db, onAllSmsSent)
		})
	})
}

const prepareSmsToSend = (db) => {
  console.log("Preparing SMSs to send")
  // Select first last X SMSs pending to be sent
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
};

// Graceful shutdown function
const gracefulShutdown = () => {
  console.log('Graceful shutdown initiated...');

  // Close the modem port
  if (modem && modem.isOpen) {
    console.log('Closing modem...');
    modem.close(() => {
      console.log('Modem closed.');

      // Detach from the database
      if (db) {
        console.log('Detaching from database...');
        db.detach();
        console.log('Database detached.');
      }

      console.log('Exiting process.');
      process.exit(0); // Exit cleanly
    });
  } else {
    // If the modem isn't open, still try to detach from the database
    if (db) {
      console.log('Detaching from database...');
      db.detach();
      console.log('Database detached.');
    }

    console.log('Exiting process.');
    process.exit(0); // Exit cleanly
  }
};

// Handle signals for graceful shutdown
process.on('SIGINT', gracefulShutdown); // Ctrl+C
process.on('SIGTERM', gracefulShutdown); // pm2 stop/restart

modem.on('open', data => {
  modem.initializeModem()

	try
	{
		Firebird.attach(options, function(err, db) {
			if(err)
				throw err

      prepareSmsToSend(db);
    });
  } catch (ex) {
    console.error(ex);
    if (modem && modem.isOpen) {
      modem.close();
    }
    if (db) {
      db.detach();
    }
  }
});

modem.on('close', () => {
  console.log('Modem port closed.');
});

modem.on('error', (err) => {
  console.error('Modem error:', err);
});

console.log("Queue modem port opening in 10 sec")
setTimeout(() => {
  console.log("Opening modem port");
  modem.open('COM3', modemOptions)
  console.log("Modem port opened")
}, 10000);