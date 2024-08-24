export function generateRandomCharacters(type) {
    switch (type) {
        case 'string':
            return Math.random().toString(36).substring(2, 10)
        case 'phone':
            return Array.from({ length: 10 }, () =>
                Math.floor(Math.random() * 10)
            ).join('')
        case 'address':
            const street = Math.random().toString(36).substring(2, 10)
            const number = Math.floor(Math.random() * 100)
            return `${
                street.charAt(0).toUpperCase() + street.slice(1)
            } ${number}`
        case 'email':
            const randomString = Math.random().toString(36).substring(2, 10)
            return `${randomString}@test.com`
    }
}
