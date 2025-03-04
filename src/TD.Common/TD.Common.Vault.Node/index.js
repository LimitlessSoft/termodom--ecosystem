module.exports = class VaultManager {
    #token
    #cache = new Map()

    constructor({
        engine,
        username,
        password,
        uri,
        defaultPath,
        defaultSecret,
    }) {
        this.engine = engine
        this.username = username
        this.password = password
        this.baseUrl = uri
        this.defaultPath = defaultPath
        this.defaultSecret = defaultSecret
    }

    async #getTokenAsync() {
        if (!this.#token) {
            await this.login()
        }
        return this.#token
    }

    async login() {
        const response = await fetch(
            this.baseUrl + '/auth/userpass/login/' + this.username,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    username: this.username,
                    password: this.password,
                }),
            }
        )
        const data = await response.json()
        this.#token = data.auth.client_token
    }

    async getSecretAsync(path) {
        if (path && path[path.length - 1] === '/')
            throw new Error(
                'Secret should not end with /. Did you mean to use getSecrets?'
            )

        if (this.#cache.has(path)) {
            return this.#cache.get(path)
        }

        const response = await fetch(
            `${this.baseUrl}/${this.engine}/data/${path || this.defaultSecret}`,
            {
                method: 'GET',
                headers: {
                    'X-Vault-Token': await this.#getTokenAsync(),
                },
            }
        )
        const data = await response.json()
        const secretData = data.data.data

        this.#cache.set(path, secretData)
        return secretData
    }

    async getSecretsAsync(path) {
        const response = await fetch(
            `${this.baseUrl}/${this.secretServer}/metadata/${
                path || this.defaultPath
            }`,
            {
                method: 'LIST',
                headers: {
                    'X-Vault-Token': await this.#getTokenAsync(),
                },
            }
        )
        const data = await response.json()
        return data.data.keys
    }
}
