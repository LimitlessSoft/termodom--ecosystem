import { IPermissionDto } from '@/dtos/permissions/IPermissionDto'
import { officeApi } from '@/apis/officeApi'
import { useEffect, useState } from 'react'

export const usePermissions = (group: string) => {
    const [permissions, setPermissions] = useState<
        IPermissionDto[] | undefined
    >(undefined)

    useEffect(() => {
        officeApi.get(`/permissions/${group}`).then((response) => {
            setPermissions(response.data)
        })
    }, [group])

    return permissions
}
