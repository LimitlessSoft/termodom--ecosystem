import Cookies from 'universal-cookie'

const cookies = new Cookies()
const APIURL = "https://localhost:32770"

export const IMAGE_BASE_URL = APIURL
// export const IMAGE_BASE_URL = 'https://termodom.rs'

export const apiFetch = (endpoint, method, contentType, body) => {
  return new Promise((resolve) => {
    var auth = cookies.get("ARToken")

    var headers = {
      'Authorization': 'bearer ' + auth ,
      'X-TDAPI-Auth': 'bearer ' + '43685BC11A1E4EFF34C4BC2EAA8ADBAE2754DD6EF484485905EA86DC14C62AC4'
    }

    if(contentType != null) {
      headers['Content-Type'] = contentType
    }
    fetch(APIURL + endpoint, {
      method: method,
      headers: headers,
      body: body
    })
    .then((response) => {
      resolve(response);
    })
  })
}