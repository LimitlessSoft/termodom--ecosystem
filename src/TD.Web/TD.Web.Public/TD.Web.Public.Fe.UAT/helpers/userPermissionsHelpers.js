const userPermissionsHelpers = {
    async createMockUserPermission(webDbClient, { permission, userId }) {
        return await webDbClient.userPermissionsRepository.create({
            permission,
            userId,
        })
    },
    async hardDeleteMockUserPermission(webDbClient, id) {
        await webDbClient.userPermissionsRepository.hardDelete(id)
    },
}

export default userPermissionsHelpers
