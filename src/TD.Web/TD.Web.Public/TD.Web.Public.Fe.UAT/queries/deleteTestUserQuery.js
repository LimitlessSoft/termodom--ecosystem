const deleteTestUserQuery = (username) => ({
    text: 'DELETE FROM "Users" WHERE "Username" = $1;',
    values: [username],
})

export default deleteTestUserQuery
