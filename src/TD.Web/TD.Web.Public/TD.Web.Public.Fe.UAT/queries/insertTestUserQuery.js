// const insertTestUserQuery = {
//     text: 'INSERT INTO "Users"("Username", "Password", "Nickname", "Type", "IsActive", "CreatedAt", "CreatedBy") VALUES($1, $2, $3, $4, $5, $6, $7) RETURNING *;',
//     values: [
//         'neko',
//         'BkgUbcI0oWPKY7ECqj0xquTUT1eQlCvnNdSevvpA4tC55jMgWAB0i',
//         'neko',
//         0,
//         true,
//         new Date(),
//         3,
//     ],
// }

// const insertTestUserQuery = (username) => ({
//     text: 'INSERT INTO "Users"("CityId", "Mail", "Mobile", "Type", "Address", "Username", "Password", "Nickname", "FavoriteStoreId", "DateOfBirth", "IsActive", "CreatedAt", "CreatedBy") VALUES($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13) RETURNING *;',
//     values: [
//         3,
//         `${username.toLowerCase()}@test.com`,
//         '+381123123',
//         0,
//         `${username} address`,
//         username,
//         '$2a$11$IpDZh9k7m/ejsSmxR2WDrubKJlja6RpmyAHl0Eh4vkT9ZhC6.zsB6',
//         username,
//         -5,
//         new Date(),
//         true,
//         new Date(),
//         0,
//     ],
// })

const confirmUserRegistrationQuery = (username) => ({
    text: 'UPDATE "Users" SET "ProcessingDate" = $1 WHERE "Username" = $2;',
    values: [new Date(), username],
})

export default confirmUserRegistrationQuery
