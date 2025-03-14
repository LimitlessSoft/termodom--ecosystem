module.exports = class UsersEndpoints {
    constructor(axios) {
        this.axios = axios
    }

    /**
     * Registers a new user with the provided details
     * @param {string} username - The desired username for the account
     * @param {string} password - The user's password (should meet security requirements)
     * @param {string} dateOfBirth - Date of birth in YYYY-MM-DD format
     * @param {string} mobile - Mobile number with country code (e.g., +1234567890)
     * @param {string} address - Full physical address
     * @param {number} cityId - ID representing the city (from predefined options)
     * @param {number} favoriteStoreId - ID of the preferred store location
     * @param {string} mail - Valid email address for the account
     */
    async registerUser({
        username,
        password,
        nickname,
        dateOfBirth,
        mobile,
        address,
        cityId,
        favoriteStoreId,
        mail,
    }) {
        return await this.axios.put('/register', {
            username,
            password,
            nickname,
            dateOfBirth,
            mobile,
            address,
            cityId,
            favoriteStoreId,
            mail,
        })
    }
}
