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
import { adminApi } from '@/apis/adminApi'
import { ArrowDropDownIcon } from '@mui/x-date-pickers'
import { toast } from 'react-toastify'

export const KorisnikAdminSettings = (props: any) => {
    const [groups, setGroups] = React.useState<any>(null)
    const [checkedGroups, setCheckedGroups] = useState<any | undefined>(null)

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
    }, [props.username])

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
                            }}
                        >
                            Sačuvaj admin podesavanja
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
                                onChange={(e) => {
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
