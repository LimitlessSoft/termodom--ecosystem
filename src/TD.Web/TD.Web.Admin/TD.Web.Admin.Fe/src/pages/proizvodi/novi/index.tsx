import { ApiBase, ContentType, fetchApi } from "../../../app/api"
import { Box, Button, Card, CardMedia, Checkbox, CircularProgress, FormControlLabel, LinearProgress, MenuItem, Stack, TextField, Typography } from "@mui/material"
import { debug } from "console"
import React, { useRef } from "react"
import { useEffect, useState } from "react"
import { toast } from "react-toastify"

const textFieldVariant = 'standard'

const ProizvodiNovi = (): JSX.Element => {

    const [units, setUnits] = useState<any | undefined>(null)
    const [groups, setGroups] = useState<any | undefined>(null)
    const [priceGroups, setPriceGroups] = useState<any | undefined>(null)
    const imagePreviewRef = useRef<any>(null)
    const [checkedGroups, setCheckedGroups] = useState<number[]>([])
    const [imageToUpload, setImageToUpload] = useState<any | undefined>(null)
    const [isCreating, setIsCreating] = useState<Boolean>(false)

    const [requestBody, setRequestBody] = useState<any>({
        name: '',
        src: '',
        image: '',
        unitId: null,
        catalogId: '',
        classification: 0,
        vat: 20,
        productPriceGroupId: null
    })

    useEffect(() => {
        fetchApi(ApiBase.Main, "/units").then((payload) => {
            setRequestBody((prev: any) => { return { ...prev, unitId: payload[0].id } })
            setUnits(payload)
        })

        fetchApi(ApiBase.Main, "/products-groups").then((payload) => {
            setGroups(payload)
        })

        fetchApi(ApiBase.Main, "/products-prices-groups").then((payload) => {
            setRequestBody((prev: any) => { return { ...prev, productPriceGroupId: payload[0].id } })
            setPriceGroups(payload)
        })
    }, [])


    return (
        <Stack
        direction={'column'}
        alignItems={'center'}
        sx={{ m: 2, '& .MuiTextField-root': { m: 1, width: '50ch' }, }}>
            
            <Typography
                sx={{ m: 2 }}
                variant='h4'>
                Kreiraj novi proizvod
            </Typography>


            <Card sx={{ width: 600 }}>
                <CardMedia
                    sx={{ objectFit: 'contain'}}
                    component={'img'}
                    alt={'upload-image'}
                    height={'536'}
                    ref={imagePreviewRef}
                    src="https://termodom.rs/img/gallery/source/cdddv.jpg" />
                <Button
                    sx={{ width: '100%' }}
                    variant="contained"
                    component="label">
                    Izaberi sliku
                    <input
                        type="file"
                        onChange={(evt) => {
                            let tgt = evt.target
                            let files = tgt.files;

                            if(files == null)
                            {
                                toast('Error upload image. Image is null.', { type: 'error' })
                                return
                            }
                            setImageToUpload(files[0])
                        
                            // FileReader support
                            if (FileReader && files && files.length) {
                                var fr = new FileReader();
                                fr.onload = function () {
                                    imagePreviewRef.current.src = fr.result;
                                }
                                fr.readAsDataURL(files[0]);
                            }
                            
                            // Not supported
                            else {
                                // fallback -- perhaps submit the input to an iframe and temporarily store
                                // them on the server until the user's session ends.
                            }
                        }}
                        hidden
                    />
                    </Button>
            </Card>

            <TextField
            required
            id='name'
            label='Ime proizvoda'
            onChange={(e) => {
                setRequestBody((prev: any) => { return { ...prev, name: e.target.value } })
            }}
            variant={textFieldVariant} />

            <TextField
                required
                id='src'
                onChange={(e) => {
                    setRequestBody((prev: any) => { return { ...prev, src: e.target.value } })
                }}
                label='link (putanja nakon termodom.rs/grupa/[ovde])'
                variant={textFieldVariant} />

            <TextField
                required
                id='catalogId'
                label='Kataloški broj'
                onChange={(e) => {
                    setRequestBody((prev: any) => { return { ...prev, catalogId: e.target.value } })
                }}
                variant={textFieldVariant} />

            {
                units == null ?
                    <CircularProgress /> :
                    <TextField
                        id='unit'
                        select
                        required
                        label='Jedinica mere'
                        onChange={(e) => {
                            setRequestBody((prev: any) => { return { ...prev, unitId: e.target.value } })
                        }}
                        helperText='Izaberite jedinicu mere proizvoda'>
                            {units.map((unit:any, index:any) => {
                                return (
                                    <MenuItem key={`jm-option-${index}`} value={unit.id}>
                                        {unit.name}
                                    </MenuItem>
                                )
                            })}
                    </TextField>
            }

            <TextField
                id='classification'
                select
                required
                label='Klasifikacija'
                onChange={(e) => {
                    setRequestBody((prev: any) => { return { ...prev, classification: e.target.value } })
                }}
                helperText='Izaberite klasu proizvoda'>
                    <MenuItem value={0}>
                        Hobi
                    </MenuItem>
                    <MenuItem value={1}>
                        Standard
                    </MenuItem>
                    <MenuItem value={2}>
                        Profi
                    </MenuItem>
            </TextField>

            <TextField
                required
                id='vat'
                label='PDV'
                defaultValue={20}
                onChange={(e) => {
                    setRequestBody((prev: any) => { return { ...prev, vat: e.target.value } })
                }}
                variant={textFieldVariant} />
            
            {
                priceGroups == null ?
                    <CircularProgress /> :
                    <TextField
                        id='priceGroup'
                        select
                        required
                        onChange={(e) => {
                            setRequestBody((prev: any) => { return { ...prev, productPriceGroupId: e.target.value } })
                        }}
                        label='Cenovna grupa proizvoda'
                        helperText='Izaberite cenovnu grupu proizvoda'>
                            {priceGroups.map((cenovnaGrupa:any, index:any) => {
                                return (
                                    <MenuItem key={`price-group-option-${index}`} value={cenovnaGrupa.id}>
                                        {cenovnaGrupa.name}
                                    </MenuItem>
                                )
                            })}
                    </TextField>
            }
            
            <Box sx={{ m: 1 }}>
                <Typography
                    variant='caption'>
                    Čekiraj grupe/podgrupe
                </Typography>
                <Box>
                    {
                        groups == null ?
                            <LinearProgress /> :
                            <Group setCheckedGroups={setCheckedGroups} checkedGroups={checkedGroups} groups={groups} parentId={null} />
                    }
                </Box>
            </Box>

            <Button
                endIcon={ isCreating ? <CircularProgress color='inherit' /> : null }
                size='large'
                sx={{ m: 2, px: 5, py: 1 }}
                variant='contained'
                onClick={() => {
                    
                    setRequestBody((prev: any) => { return { ...prev, groups: checkedGroups } })

                    var formData = new FormData()
                    formData.append("Image", imageToUpload)

                    fetchApi(ApiBase.Main, "/images", {
                        method: 'POST',
                        body: formData,
                        contentType: ContentType.FormData
                    }).then((payload) => {
                        toast('Slika uspešno uploadovan-a!', { type: 'success' })
                        setRequestBody((prev: any) => { return { ...prev, image: payload } })
                        toast('Kreiram proizvod...')

                        fetchApi(ApiBase.Main, "/products", {
                            method: 'PUT',
                            body: { ...requestBody, image: payload, groups: checkedGroups },
                            contentType: ContentType.ApplicationJson
                        }).then(() => {
                            toast('Proizvod uspešno kreiran!', { type: 'success' })
                        })
                    })
                }}>
                Kreiraj
            </Button>
        </Stack>
    )
}

const isChecked = (groups: any[], checkedGroups: number[], id: number) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)
    const thisChecked = checkedGroups.find((item) => item == id) != null
    if(thisChecked || subGroups.length === 0)
        return thisChecked

    const results: any = subGroups.map((sg) => {
        return isChecked(groups, checkedGroups, sg.id)
    })

    return results.find((i: any) => i == true) != null
}

const decheck = (groups: any[], setCheckedGroups: any, id: number) => {
    const subGroups = groups.filter((item) => item.parentGroupId === id)

    setCheckedGroups((prev: any) => [ ...prev.filter((item: any) => item !== id)])
    subGroups.map((sg) => {
        decheck(groups, setCheckedGroups, sg.id)
    })
}

const Group = (props: any): JSX.Element => {
    return props.groups.filter((item: any) => item.parentGroupId === props.parentId).map((group: any) => {
        const mxVal = props.parentId == null ? 0 : 4
        return (
            <Box
                sx={{ mx: mxVal }}
                key={`group-cb-${group.id}`}>
                <FormControlLabel
                    label={group.name}
                    control={<Checkbox checked={isChecked(props.groups, props.checkedGroups, group.id)} onChange={(e) => {
                        if(e.target.checked) {
                            props.setCheckedGroups((prev: any) => [ ...prev, group.id])
                        } else {
                            decheck(props.groups, props.setCheckedGroups, group.id)
                        }
                    }} />}
                    />
                {
                    props.groups.filter((item: any) => item.parentGroupId == group.id).length > 0 ?
                    <Group setCheckedGroups={props.setCheckedGroups} checkedGroups={props.checkedGroups} groups={props.groups} parentId={group.id} /> :
                    null
                }
            </Box>
        )
    })
}

export default ProizvodiNovi