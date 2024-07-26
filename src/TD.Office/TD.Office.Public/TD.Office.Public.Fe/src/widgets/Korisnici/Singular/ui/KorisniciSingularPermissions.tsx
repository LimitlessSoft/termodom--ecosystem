import { officeApi } from '@/apis/officeApi'
import { ExpandMore } from '@mui/icons-material'
import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Checkbox,
    CircularProgress,
    FormControlLabel,
    Grid,
    LinearProgress,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { IKorisniciSingularPermissionsProps } from '../interfaces/IKorisniciSingularPermissionsProps'
import { IPermissionDto } from '@/dtos/permissions/IPermissionDto'
import { AxiosResponse } from 'axios'
import { toast } from 'react-toastify'

export const KorisniciSingularPermissions = (
    props: IKorisniciSingularPermissionsProps
) => {
    const [userPermissions, setUserPermissions] = useState<
        IPermissionDto[] | undefined
    >([])
    const [currentlyUpdatingPermissions, setCurrentlyUpdatingPermissions] =
        useState<number[]>([])

    const [searchFilter, setSearchFilter] = useState<string>('')

    useEffect(() => {
        officeApi
            .get(`/users/${props.userId}/permissions`)
            .then((response: AxiosResponse) => {
                setUserPermissions(response.data)
            })
    }, [])

    return (
        <Grid item sm={12}>
            <Accordion>
                <AccordionSummary expandIcon={<ExpandMore />}>
                    <Typography>Prava korisnika</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    {userPermissions === undefined && <LinearProgress />}
                    {userPermissions !== undefined && (
                        <Stack spacing={1}>
                            <TextField
                                label={`Pretraži prava`}
                                value={searchFilter}
                                onChange={(e) =>
                                    setSearchFilter(e.target.value)
                                }
                            />
                            {userPermissions
                                .filter((x: IPermissionDto) => {
                                    return (
                                        x.name
                                            .toLowerCase()
                                            .includes(
                                                searchFilter.toLowerCase()
                                            ) ||
                                        x.description
                                            .toLowerCase()
                                            .includes(
                                                searchFilter.toLowerCase()
                                            ) ||
                                        x.id
                                            .toString()
                                            .includes(
                                                searchFilter.toLowerCase()
                                            )
                                    )
                                })
                                .map((permission) => (
                                    <FormControlLabel
                                        key={permission.id}
                                        label={permission.description}
                                        control={
                                            currentlyUpdatingPermissions.includes(
                                                permission.id
                                            ) ? (
                                                <CircularProgress
                                                    size={24}
                                                    sx={{
                                                        p: 1,
                                                    }}
                                                />
                                            ) : (
                                                <Checkbox
                                                    onChange={() => {
                                                        setCurrentlyUpdatingPermissions(
                                                            [
                                                                ...currentlyUpdatingPermissions,
                                                                permission.id,
                                                            ]
                                                        )
                                                        officeApi
                                                            .put(
                                                                `/users/${props.userId}/permissions/${permission.id}`,
                                                                {
                                                                    isGranted:
                                                                        !permission.isGranted,
                                                                }
                                                            )
                                                            .then(() => {
                                                                setUserPermissions(
                                                                    userPermissions.map(
                                                                        (p) => {
                                                                            if (
                                                                                p.id ===
                                                                                permission.id
                                                                            ) {
                                                                                return {
                                                                                    ...p,
                                                                                    isGranted:
                                                                                        !p.isGranted,
                                                                                }
                                                                            }
                                                                            return p
                                                                        }
                                                                    )
                                                                )
                                                                toast.success(
                                                                    `Pravo ${permission.name} je uspešno promenjeno!`
                                                                )
                                                            })
                                                            .finally(() => {
                                                                setCurrentlyUpdatingPermissions(
                                                                    currentlyUpdatingPermissions.filter(
                                                                        (id) =>
                                                                            id !==
                                                                            permission.id
                                                                    )
                                                                )
                                                            })
                                                    }}
                                                    checked={
                                                        permission.isGranted
                                                    }
                                                />
                                            )
                                        }
                                    />
                                ))}
                        </Stack>
                    )}
                </AccordionDetails>
            </Accordion>
        </Grid>
    )
}
