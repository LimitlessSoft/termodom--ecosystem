import {
    Box,
    Button,
    Checkbox,
    Divider,
    FormControlLabel,
    Grid,
    LinearProgress,
    Paper,
    TextField,
    Typography,
} from '@mui/material'
import React, { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { toast } from 'react-toastify'

interface Permission {
    id: number
    name: string
    description: string
    isGranted: boolean
}

interface KorisnikAdminSettingsProps {
    username: string | string[] | undefined
    disabled?: boolean
}

export const KorisnikAdminSettings = ({ username, disabled }: KorisnikAdminSettingsProps) => {
    const [groups, setGroups] = useState<any[] | null>(null)
    const [checkedGroups, setCheckedGroups] = useState<number[] | null>(null)
    const [permissions, setPermissions] = useState<Permission[] | null>(null)
    const [savingGroups, setSavingGroups] = useState(false)
    const [savingPermissions, setSavingPermissions] = useState(false)
    const [permissionSearch, setPermissionSearch] = useState('')

    useEffect(() => {
        if (!username) return

        adminApi.get(`/products-groups`).then((response) => {
            setGroups(response.data)
        })

        adminApi
            .get(`/users-managing-products-groups/${username}`)
            .then((response) => setCheckedGroups(response.data))
            .catch((err) => handleApiError(err))

        adminApi
            .get(`/users/${username}/permissions`)
            .then((response) => setPermissions(response.data))
            .catch((err) => handleApiError(err))
    }, [username])

    const handlePermissionChange = (permissionName: string, checked: boolean, autoSave = false) => {
        const updated = permissions?.map((p) =>
            p.name === permissionName ? { ...p, isGranted: checked } : p
        ) ?? null

        setPermissions(updated)

        if (autoSave && updated) {
            setSavingPermissions(true)
            const grantedPermissions = updated
                .filter((p) => p.isGranted)
                .map((p) => p.id)

            adminApi
                .put(`/users/${username}/permissions`, grantedPermissions)
                .then(() => toast.success('Prava uspešno sačuvana!'))
                .catch((err) => handleApiError(err))
                .finally(() => setSavingPermissions(false))
        }
    }

    const savePermissions = () => {
        if (!permissions) return

        setSavingPermissions(true)
        const grantedPermissions = permissions
            .filter((p) => p.isGranted)
            .map((p) => p.id)

        adminApi
            .put(`/users/${username}/permissions`, grantedPermissions)
            .then(() => toast.success('Prava uspešno sačuvana!'))
            .catch((err) => handleApiError(err))
            .finally(() => setSavingPermissions(false))
    }

    const saveGroups = () => {
        setSavingGroups(true)
        adminApi
            .post(`/users-managing-products-groups/${username}`, checkedGroups)
            .then(() => toast.success('Grupe proizvoda uspešno sačuvane!'))
            .catch((err) => handleApiError(err))
            .finally(() => setSavingGroups(false))
    }

    return (
        <Grid container spacing={3}>
            {/* Permissions Section */}
            <Grid item xs={12} lg={6}>
                <Paper variant="outlined" sx={{ p: 2, height: '100%' }}>
                    <Typography variant="h6" gutterBottom>
                        Prava korisnika
                    </Typography>
                    <Divider sx={{ mb: 2 }} />

                    {permissions === null ? (
                        <LinearProgress />
                    ) : (
                        <>
                            <TextField
                                fullWidth
                                size="small"
                                placeholder="Pretraži prava..."
                                value={permissionSearch}
                                onChange={(e) => setPermissionSearch(e.target.value)}
                                sx={{ mb: 2 }}
                            />
                            <Box sx={{ maxHeight: 300, overflow: 'auto', opacity: savingPermissions ? 0.5 : 1 }}>
                                <Grid container spacing={0}>
                                    {permissions
                                        .filter((p) =>
                                            p.description
                                                .toLowerCase()
                                                .includes(permissionSearch.toLowerCase())
                                        )
                                        .sort((a, b) => a.description.localeCompare(b.description))
                                        .map((permission) => (
                                            <Grid item xs={12} key={permission.name}>
                                                <FormControlLabel
                                                    disabled={disabled || savingPermissions}
                                                    label={
                                                        <Typography variant="body2">
                                                            {permission.description}
                                                        </Typography>
                                                    }
                                                    control={
                                                        <Checkbox
                                                            size="small"
                                                            checked={permission.isGranted}
                                                            onChange={(e) =>
                                                                handlePermissionChange(
                                                                    permission.name,
                                                                    e.target.checked
                                                                )
                                                            }
                                                        />
                                                    }
                                                />
                                            </Grid>
                                        ))}
                                </Grid>
                            </Box>
                            <Box sx={{ mt: 2 }}>
                                <Button
                                    variant="contained"
                                    disabled={savingPermissions || disabled}
                                    onClick={savePermissions}
                                >
                                    Sačuvaj prava
                                </Button>
                            </Box>
                        </>
                    )}
                </Paper>
            </Grid>

            {/* Product Groups Section */}
            <Grid item xs={12} lg={6}>
                <Paper variant="outlined" sx={{ p: 2, height: '100%' }}>
                    <Typography variant="h6" gutterBottom>
                        Grupe proizvoda
                    </Typography>
                    <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                        Grupe proizvoda kojima korisnik može upravljati
                    </Typography>
                    <Divider sx={{ mb: 2 }} />

                    {permissions === null || groups === null || checkedGroups === null ? (
                        <LinearProgress />
                    ) : (
                        <>
                            {(() => {
                                const editAllPermission = permissions.find(
                                    (p) => p.name === 'Admin_Products_EditAll'
                                )
                                const canEditAll = editAllPermission?.isGranted ?? false

                                return (
                                    <>
                                        <FormControlLabel
                                            disabled={disabled || savingPermissions}
                                            label={
                                                <Typography variant="body2" fontWeight="medium">
                                                    {editAllPermission?.description ?? 'Može da menja sve grupe'}
                                                </Typography>
                                            }
                                            control={
                                                <Checkbox
                                                    size="small"
                                                    checked={canEditAll}
                                                    onChange={(e) =>
                                                        handlePermissionChange(
                                                            'Admin_Products_EditAll',
                                                            e.target.checked,
                                                            true
                                                        )
                                                    }
                                                />
                                            }
                                        />
                                        <Divider sx={{ my: 2 }} />
                                        <Box
                                            sx={{
                                                maxHeight: 350,
                                                overflow: 'auto',
                                                opacity: canEditAll || savingGroups ? 0.5 : 1,
                                            }}
                                        >
                                            <ProductGroupTree
                                                groups={groups}
                                                checkedGroups={checkedGroups}
                                                setCheckedGroups={setCheckedGroups}
                                                parentId={null}
                                                disabled={disabled || canEditAll || savingGroups}
                                            />
                                        </Box>
                                    </>
                                )
                            })()}
                            <Box sx={{ mt: 2 }}>
                                <Button
                                    variant="contained"
                                    disabled={savingGroups || disabled}
                                    onClick={saveGroups}
                                >
                                    Sačuvaj grupe
                                </Button>
                            </Box>
                        </>
                    )}
                </Paper>
            </Grid>
        </Grid>
    )
}

// Helper functions for product groups tree
const isChecked = (groups: any[], checkedGroups: number[], id: number): boolean => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)
    const thisChecked = checkedGroups.includes(id)
    if (thisChecked || subGroups.length === 0) return thisChecked

    return subGroups.some((sg) => isChecked(groups, checkedGroups, sg.id))
}

const uncheckRecursive = (
    groups: any[],
    setCheckedGroups: React.Dispatch<React.SetStateAction<number[] | null>>,
    id: number
) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)
    setCheckedGroups((prev) => prev?.filter((item) => item !== id) ?? null)
    subGroups.forEach((sg) => uncheckRecursive(groups, setCheckedGroups, sg.id))
}

const getAllChildIds = (groups: any[], parentId: number): number[] => {
    const children = groups.filter((item) => item.parentGroupId === parentId)
    const childIds = children.map((c) => c.id)
    const grandchildIds = children.flatMap((c) => getAllChildIds(groups, c.id))
    return [...childIds, ...grandchildIds]
}

interface ProductGroupTreeProps {
    groups: any[]
    checkedGroups: number[]
    setCheckedGroups: React.Dispatch<React.SetStateAction<number[] | null>>
    parentId: number | null
    disabled?: boolean
    level?: number
}

const ProductGroupTree = ({
    groups,
    checkedGroups,
    setCheckedGroups,
    parentId,
    disabled,
    level = 0,
}: ProductGroupTreeProps) => {
    const filteredGroups = groups.filter((item) => item.parentGroupId === parentId)

    return (
        <>
            {filteredGroups.map((group) => {
                const hasChildren = groups.some((g) => g.parentGroupId === group.id)
                return (
                    <Box key={group.id} sx={{ ml: level * 2 }}>
                        <FormControlLabel
                            disabled={disabled}
                            label={
                                <Typography variant="body2">
                                    {group.name}
                                </Typography>
                            }
                            control={
                                <Checkbox
                                    size="small"
                                    checked={isChecked(groups, checkedGroups, group.id)}
                                    onChange={(e) => {
                                        if (e.target.checked) {
                                            const allChildIds = getAllChildIds(groups, group.id)
                                            setCheckedGroups((prev) => [
                                                ...(prev ?? []),
                                                group.id,
                                                ...allChildIds,
                                                ...(parentId ? [parentId] : []),
                                            ])
                                        } else {
                                            uncheckRecursive(groups, setCheckedGroups, group.id)
                                        }
                                    }}
                                />
                            }
                        />
                        {hasChildren && (
                            <ProductGroupTree
                                groups={groups}
                                checkedGroups={checkedGroups}
                                setCheckedGroups={setCheckedGroups}
                                parentId={group.id}
                                disabled={disabled}
                                level={level + 1}
                            />
                        )}
                    </Box>
                )
            })}
        </>
    )
}
