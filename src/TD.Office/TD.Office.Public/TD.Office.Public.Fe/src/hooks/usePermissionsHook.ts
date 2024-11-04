import { IPermissionDto } from '@/dtos/permissions/IPermissionDto'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { useEffect, useState } from 'react'

export const usePermissions = (group: string) => {
    const [permissions, setPermissions] = useState<IPermissionDto[]>([])

    useEffect(() => {
        officeApi
            .get(`/permissions/${group}`)
            .then((response) => {
                setPermissions(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [group])

    return permissions
}
