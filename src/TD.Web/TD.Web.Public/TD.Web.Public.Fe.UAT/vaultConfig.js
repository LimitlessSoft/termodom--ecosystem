export const vault = {
    baseUrl: 'http://vault.termodom.rs:8199/v1',
    defaultSecret: 'web/public/fe',
    secretServer: 'automation',
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
            `${vault.baseUrl}/${vault.secretServer}/data/${path || vault.defaultSecret}`,
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
            `${vault.baseUrl}/${vault.secretServer}/metadata/${path || vault.defaultPath}`,
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
