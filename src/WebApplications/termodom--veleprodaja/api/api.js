const getAsync = (endpoint) => {
    return new Promise(async (resolve, reject) => {

        postAsync("/api/korisnik/getToken?username=sasar&password=12321", {})
        .then(response => {
            if(response.status != 200) {
                reject(response)
                return
            }
            response.text()
                .then(async apiKey => {
                    resolve(await fetch('https://api.termodom.rs/' + endpoint, {
                        method: "GET",
                        mode: "cors",
                        cache: "no-cache",
                        headers: {
                            Authorization: 'bearer ' + apiKey
                        }
                    }))
                })
        })
    })
}

const postAsync = (endpoint, body) => {
    return new Promise(async (resolve, reject) => {
        var response = await fetch('https://api.termodom.rs/' + endpoint,
            {
                method: "POST",
                mode: "cors",
                cache: "no-cache",
                body: body
            })
        resolve(response)
    })
}

module.exports = {
    apiGetAsync: getAsync
}