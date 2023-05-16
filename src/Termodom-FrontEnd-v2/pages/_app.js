import React, { useEffect, useState } from 'react'
import { KorisnikContext } from '../KorisnikContext'
import './globals.css'
import { apiFetch } from '../api'
import { MessageBoxContext } from '../MessageBoxContext'
import messageBoxStyles from './messageBox.module.css'

function MessageBox(props) {
  return (
    <div className={messageBoxStyles.messageBox + ' ' + (props.message == null || props.message.trim().length == 0 ? '' : messageBoxStyles.active)}>
      { props.message }
    </div>
  )
}

function MyApp({ Component, pageProps }) {

  const [trenutniKorisnik, setTrenutniKorisnik] = useState(null)
  const [messageBoxMessage, setMessageBoxMessage] = useState(null)

  var messageBoxTimeout = null

  useEffect(() => {
  }, [])

  useEffect(() => {
    if(messageBoxMessage == null || messageBoxMessage.trim().length == 0) {
      return
    }

    clearTimeout(messageBoxTimeout)
    messageBoxTimeout = setTimeout(() => {
      setMessageBoxMessage(null)
    }, 3000);
  }, [messageBoxMessage])

  return (
    <KorisnikContext.Provider value={{ value: trenutniKorisnik, set: setTrenutniKorisnik }}>
      <MessageBoxContext.Provider value={{ show: (message) => {
        setMessageBoxMessage(message)
      }}}>
        <MessageBox message={messageBoxMessage} />
        <Component {...pageProps} />
      </MessageBoxContext.Provider>
    </KorisnikContext.Provider>
  )
}

export default MyApp
