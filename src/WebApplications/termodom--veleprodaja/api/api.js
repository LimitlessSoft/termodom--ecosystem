const nextConfig = require("../next.config")

const getAsync = (endpoint) => {
    return new Promise((resolve, reject) => {
        fetch(nextConfig.apiHost + endpoint, {
            method: "GET",
            mode: "cors",
            cache: "no-cache",
            headers: {
                Authorization: 'bearer ' + apiKey
            }
        })
        .then(x => resolve(x))
        .catch(x => reject(x))
    })
}

const postAsync = (endpoint, body) => {
    return new Promise((resolve, reject) => {
        fetch(nextConfig.apiHost + endpoint, {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            body: body
        })
        .then(x => resolve(x))
        .catch(x => reject(x))
    })
}

module.exports = {
    apiGetAsync: getAsync,
    apiPostAsync: postAsync
}