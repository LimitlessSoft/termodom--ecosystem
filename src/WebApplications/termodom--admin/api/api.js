const getAsync = (endpoint) => {
    return new Promise((resolve, reject) => {
        fetch(process.env.NEXT_PUBLIC_API_HOST + endpoint, {
            method: "GET",
            mode: "cors",
            cache: "no-cache",
            headers: {
                Authorization: 'bearer ' + sessionStorage.getItem("bearer_token")
            }
        })
        .then(x => resolve(x))
        .catch(x => reject(x))
    })
}

const postAsync = (endpoint, body) => {
    return new Promise((resolve, reject) => {
        fetch(process.env.NEXT_PUBLIC_API_HOST + endpoint, {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            body: JSON.stringify(body),
            headers: {
                Authorization: 'bearer ' + sessionStorage.getItem("bearer_token"),
                "Content-Type": "application/json"
            }
        })
        .then(x => resolve(x))
        .catch(x => reject(x))
    })
}

const putAsync = (endpoint, body) => {
    return new Promise((resolve, reject) => {
        fetch(process.env.NEXT_PUBLIC_API_HOST + endpoint, {
            method: "PUT",
            mode: "cors",
            cache: "no-cache",
            body: JSON.stringify(body),
            headers: {
                Authorization: 'bearer ' + sessionStorage.getItem("bearer_token"),
                "Content-Type": "application/json"
            }
        })
        .then(x => resolve(x))
        .catch(x => reject(x))
    })
}

module.exports = {
    apiGetAsync: getAsync,
    apiPostAsync: postAsync,
    apiPutAsync: putAsync
}