import { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { IPermissionDto } from '@/dtos'

export const usePermissions = (group: string) => {
    const [permissions, setPermissions] = useState<
        IPermissionDto[] | undefined
    >(undefined)

    useEffect(() => {
        adminApi
            .get(`/permissions/${group}`)
            .then((response) => {
                setPermissions(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [group])

    return permissions
}
