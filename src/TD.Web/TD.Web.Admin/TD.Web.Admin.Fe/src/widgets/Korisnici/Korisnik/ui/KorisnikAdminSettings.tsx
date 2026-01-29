import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    Button,
    Checkbox,
    FormControlLabel,
    Grid,
    LinearProgress,
    Paper,
    Typography,
} from '@mui/material'
import React, { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { ArrowDropDownIcon } from '@mui/x-date-pickers'
import { toast } from 'react-toastify'

interface Permission {
    id: number
    name: string
    description: string
    isGranted: boolean
}

export const KorisnikAdminSettings = (props: any) => {
    const [groups, setGroups] = React.useState<any>(null)
    const [checkedGroups, setCheckedGroups] = useState<any | undefined>(null)
    const [permissions, setPermissions] = useState<Permission[] | null>(null)
    const [savingPermissions, setSavingPermissions] = useState(false)

    useEffect(() => {
        if (props.username === undefined) return

        adminApi.get(`/products-groups`).then((response) => {
            setGroups(response.data)
        })

        adminApi
            .get(`/users-managing-products-groups/${props.username}`)
            .then((response) => {
                setCheckedGroups(response.data)
            })
            .catch((err) => handleApiError(err))

        adminApi
            .get(`/users/${props.username}/permissions`)
            .then((response) => {
                setPermissions(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [props.username])

    const handlePermissionChange = (permissionName: string, checked: boolean) => {
        setPermissions((prev) =>
            prev?.map((p) =>
                p.name === permissionName ? { ...p, isGranted: checked } : p
            ) ?? null
        )
    }

    const savePermissions = () => {
        if (!permissions) return

        setSavingPermissions(true)
        const grantedPermissions = permissions
            .filter((p) => p.isGranted)
            .map((p) => p.id)

        adminApi
            .put(`/users/${props.username}/permissions`, grantedPermissions)
            .then(() => {
                toast.success('Prava sačuvana!')
            })
            .catch((err) => handleApiError(err))
            .finally(() => setSavingPermissions(false))
    }

    return (
        <Accordion
            sx={{
                my: 2,
            }}
        >
            <AccordionSummary expandIcon={<ArrowDropDownIcon />}>
                <Typography variant={`h6`}>Admin Podešavanja</Typography>
            </AccordionSummary>
            <AccordionDetails>
                <Grid container spacing={2} p={4}>
                    <Grid item xs={12}>
                        <Typography variant={`h4`}>Admin Settings</Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <Paper
                            elevation={12}
                            sx={{
                                p: 2,
                            }}
                        >
                            <Typography variant={`h6`}>
                                Grupe proizvoda kojima korisnik upravlja
                            </Typography>
                            <Box>
                                {groups == null || checkedGroups == null ? (
                                    <LinearProgress />
                                ) : (
                                    <Group
                                        setCheckedGroups={setCheckedGroups}
                                        checkedGroups={checkedGroups}
                                        groups={groups}
                                        parentId={null}
                                    />
                                )}
                            </Box>
                        </Paper>
                    </Grid>
                    <Grid item xs={12}>
                        <Button
                            variant={`contained`}
                            onClick={() => {
                                adminApi
                                    .post(
                                        `/users-managing-products-groups/${props.username}`,
                                        checkedGroups
                                    )
                                    .then(() => {
                                        toast.success(
                                            `Admin podesavanja sacuvana!`
                                        )
                                    })
                                    .catch((err) => handleApiError(err))
                            }}
                        >
                            Sačuvaj grupe proizvoda
                        </Button>
                    </Grid>
                    <Grid item xs={12}>
                        <Paper
                            elevation={12}
                            sx={{
                                p: 2,
                                mt: 2,
                            }}
                        >
                            <Typography variant={`h6`}>
                                Prava korisnika
                            </Typography>
                            <Box>
                                {permissions == null ? (
                                    <LinearProgress />
                                ) : (
                                    <Grid container>
                                        {permissions.map((permission) => (
                                            <Grid
                                                item
                                                xs={12}
                                                sm={6}
                                                md={4}
                                                key={permission.name}
                                            >
                                                <FormControlLabel
                                                    label={permission.description}
                                                    control={
                                                        <Checkbox
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
                                )}
                            </Box>
                        </Paper>
                    </Grid>
                    <Grid item xs={12}>
                        <Button
                            variant={`contained`}
                            disabled={savingPermissions || permissions == null}
                            onClick={savePermissions}
                        >
                            Sačuvaj prava
                        </Button>
                    </Grid>
                </Grid>
            </AccordionDetails>
        </Accordion>
    )
}

const isChecked = (groups: any[], checkedGroups: number[], id: number) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)
    const thisChecked = checkedGroups.find((item) => item == id) != null
    if (thisChecked || subGroups.length === 0) return thisChecked

    const results: any = subGroups.map((sg) => {
        return isChecked(groups, checkedGroups, sg.id)
    })

    return results.find((i: any) => i == true) != null
}

const decheck = (groups: any[], setCheckedGroups: any, id: number) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)

    setCheckedGroups((prev: any) => [
        ...prev.filter((item: any) => item !== id),
    ])
    subGroups.map((sg) => {
        decheck(groups, setCheckedGroups, sg.id)
    })
}

const Group = (props: any) => {
    return props.groups
        .filter((item: any) => item.parentGroupId === props.parentId)
        .map((group: any) => {
            const mxVal = props.parentId == null ? 0 : 4
            return (
                <Box sx={{ mx: mxVal }} key={`group-cb-${group.id}`}>
                    <FormControlLabel
                        label={group.name}
                        control={
                            <Checkbox
                                disabled={props.disabled}
                                checked={isChecked(
                                    props.groups,
                                    props.checkedGroups,
                                    group.id
                                )}
                                onChange={(e: any) => {
                                    if (e.target.checked) {
                                        props.setCheckedGroups((prev: any) => [
                                            ...prev,
                                            group.id,
                                        ])

                                        if (props.parentId) {
                                            props.setCheckedGroups(
                                                (prev: any) => [
                                                    ...prev,
                                                    props.parentId,
                                                ]
                                            )
                                        }
                                    } else {
                                        decheck(
                                            props.groups,
                                            props.setCheckedGroups,
                                            group.id
                                        )
                                    }
                                }}
                            />
                        }
                    />
                    {props.groups.filter(
                        (item: any) => item.parentGroupId == group.id
                    ).length > 0 ? (
                        <Group
                            disabled={props.disabled}
                            setCheckedGroups={props.setCheckedGroups}
                            checkedGroups={props.checkedGroups}
                            groups={props.groups}
                            parentId={group.id}
                        />
                    ) : null}
                </Box>
            )
        })
}
