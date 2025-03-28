const userPermissionsHelpers = {
    async createMockUserPermission(webDbClient, { permission, userId }) {
        return await webDbClient.userPermissionsRepository.create({
            permission,
            userId,
        })
    },
}

export default userPermissionsHelpers
