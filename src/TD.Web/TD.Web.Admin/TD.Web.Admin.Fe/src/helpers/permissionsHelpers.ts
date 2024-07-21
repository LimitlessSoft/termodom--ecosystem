import { IPermissionDto } from '@/dtos'

export const hasPermission = (
    permissions: IPermissionDto[] | undefined,
    permissionName: string
): boolean =>
    permissions?.find((p) => p.name === permissionName)?.isGranted || false
