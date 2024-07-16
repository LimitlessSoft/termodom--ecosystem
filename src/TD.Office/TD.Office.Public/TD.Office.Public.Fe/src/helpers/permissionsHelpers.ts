import { IPermissionDto } from '@/dtos/permissions/IPermissionDto'

export const hasPermission = (
    permissions: IPermissionDto[] | undefined,
    permissionName: string
): boolean =>
    permissions?.find((p) => p.name === permissionName)?.isGranted || false
