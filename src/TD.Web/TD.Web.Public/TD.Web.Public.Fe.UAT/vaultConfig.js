import pg from 'pg'
const { Pool } = pg

const vault = {
    baseUrl: 'http://vault.termodom.rs:8199/v1',
    defaultSecret: 'web/public/fe',
    defaultPath: 'web/public',
    username: 'filip',
    password: '123456789',
    _token: null,
    token: async () => {
        if (!vault._token) {
            await vault.login()
        }

        return vault._token
    },
    login: async () => {
        const response = await fetch(
            vault.baseUrl + '/auth/userpass/login/' + vault.username,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    username: vault.username,
                    password: vault.password,
                }),
            }
        )
        const data = await response.json()
        vault._token = data.auth.client_token
    },
    getSecret: async (path) => {
        if (path && path[path.length - 1] === '/')
            throw new Error(
                'Secret should not end with /. Did you mean to use getSecrets?'
            )

        const response = await fetch(
            vault.baseUrl + '/develop/data/' + (path || vault.defaultSecret),
            {
                method: 'GET',
                headers: {
                    'X-Vault-Token': await vault.token(),
                },
            }
        )
        const data = await response.json()
        return data.data.data
    },
    getSecrets: async (path) => {
        const response = await fetch(
            vault.baseUrl + '/develop/metadata/' + (path || vault.defaultPath),
            {
                method: 'LIST',
                headers: {
                    'X-Vault-Token': await vault.token(),
                },
            }
        )
        const data = await response.json()
        return data.data.keys
    },
}

const { POSTGRES_HOST, POSTGRES_USER, POSTGRES_PORT, POSTGRES_PASSWORD } =
    await vault.getSecret('web/public/api')

console.log(POSTGRES_PASSWORD)

export const webDb = new Pool({
    host: POSTGRES_HOST,
    user: POSTGRES_USER,
    database: 'develop_web',
    password: POSTGRES_PASSWORD,
    port: POSTGRES_PORT,
})

//Username, Nickname, Password, Type, isActive, CreatedAt, CreatedBy

// console.log(await vault.getSecret())
// console.log(await vault.getSecret('office/public/api'))
// console.log(await vault.getSecrets('/office/public/'))
// console.log(await vault.getSecrets('/office'))
