This microservice is used to prepare mass SMS sending. It stores state of mass sms queue,
and passes messages to the actual sender API. If any problem happens, this microservice will
preserve the state and try again later.

SMSEntity will store all SMSs needed to be sent. Once you decide to send SMS messages,
global state flag will be updated to "SENDING", and all SMSs will be sent to the actual API.
As long as there is at least one SMS in the queue, global state flag will be "SENDING", and no other
mass SMS will be accepted. If there are no SMSs in the queue, global state flag will be "NOT_SENDING"
at which point new SMSs will be accepted.