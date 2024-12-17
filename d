[1mdiff --git a/src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/src/widgets/Izvestaji/IzvestajUkupneKolicineRobeUDokumentima/ui/IzvestajUkupneKolicineRobeUDokumentima.js b/src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/src/widgets/Izvestaji/IzvestajUkupneKolicineRobeUDokumentima/ui/IzvestajUkupneKolicineRobeUDokumentima.js[m
[1mindex 2cfbdb78..c78682c7 100644[m
[1m--- a/src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/src/widgets/Izvestaji/IzvestajUkupneKolicineRobeUDokumentima/ui/IzvestajUkupneKolicineRobeUDokumentima.js[m
[1m+++ b/src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/src/widgets/Izvestaji/IzvestajUkupneKolicineRobeUDokumentima/ui/IzvestajUkupneKolicineRobeUDokumentima.js[m
[36m@@ -97,6 +97,31 @@[m [mexport const IzvestajUkupneKolicineRobeUDokumentima = () => {[m
         }))[m
     }, [magacini])[m
 [m
[32m+[m[32m    console.log(data)[m
[32m+[m
[32m+[m[32m    const handlePromeniRoditeljskimDokumentaminaNacinUplate = () => {[m
[32m+[m[32m        setPromeniNaNacinUplateInProgres(true)[m
[32m+[m
[32m+[m[32m        officeApi[m
[32m+[m[32m            .post([m
[32m+[m[32m                `izvestaji-ukupnih-kolicina-po-robi-u-filtriranim-dokumentima-promeni-nacin-uplate`,[m
[32m+[m[32m                {[m
[32m+[m[32m                    ...promeniNaNacinUplateRequest,[m
[32m+[m[32m                    ...request,[m
[32m+[m[32m                }[m
[32m+[m[32m            )[m
[32m+[m[32m            .then(() => {[m
[32m+[m[32m                setData(undefined)[m
[32m+[m
[32m+[m[32m                toast.success('Uspešno promenjeni nacini uplate')[m
[32m+[m[32m            })[m
[32m+[m[32m            .catch(handleApiError)[m
[32m+[m[32m            .finally(() => {[m
[32m+[m[32m                setIsPromeniNaNacinUplateDialogOpen(false)[m
[32m+[m[32m                setPromeniNaNacinUplateInProgres(false)[m
[32m+[m[32m            })[m
[32m+[m[32m    }[m
[32m+[m
     if ([m
         !hasPermission([m
             permissions,[m
[36m@@ -438,33 +463,9 @@[m [mexport const IzvestajUkupneKolicineRobeUDokumentima = () => {[m
                             <DialogActions>[m
                                 {!izveziInProgres && ([m
                                     <Button[m
[31m-                                        onClick={() => {[m
[31m-                                            setPromeniNaNacinUplateInProgres([m
[31m-                                                true[m
[31m-                                            )[m
[31m-                                            officeApi[m
[31m-                                                .post([m
[31m-                                                    `izvestaji-ukupnih-kolicina-po-robi-u-filtriranim-dokumentima-promeni-nacin-uplate`,[m
[31m-                                                    {[m
[31m-                                                        ...promeniNaNacinUplateRequest,[m
[31m-                                                        ...request,[m
[31m-                                                    }[m
[31m-                                                )[m
[31m-                                                .then(() => {[m
[31m-                                                    toast.success([m
[31m-                                                        'Uspešno promenjeni nacini uplate'[m
[31m-                                                    )[m
[31m-                                                })[m
[31m-                                                .catch(handleApiError)[m
[31m-                                                .finally(() => {[m
[31m-                                                    setIsPromeniNaNacinUplateDialogOpen([m
[31m-                                                        false[m
[31m-                                                    )[m
[31m-                                                    setPromeniNaNacinUplateInProgres([m
[31m-                                                        false[m
[31m-                                                    )[m
[31m-                                                })[m
[31m-                                        }}[m
[32m+[m[32m                                        onClick={[m
[32m+[m[32m                                            handlePromeniRoditeljskimDokumentaminaNacinUplate[m
[32m+[m[32m                                        }[m
                                         variant={`contained`}[m
                                     >[m
                                         Promeni nacine uplate[m
[1mdiff --git a/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe/src/pages/porudzbine/[hash].tsx b/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe/src/pages/porudzbine/[hash].tsx[m
[1mdeleted file mode 100644[m
[1mindex ddbb5551..00000000[m
[1m--- a/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe/src/pages/porudzbine/[hash].tsx[m
[1m+++ /dev/null[m
[36m@@ -1,121 +0,0 @@[m
[31m-import { PorudzbinaActionBar } from '@/widgets/Porudzbine/PorudzbinaActionbar'[m
[31m-import { PorudzbinaAdminInfo } from '@/widgets/Porudzbine/PorudzbinaAdminInfo'[m
[31m-import { PorudzbinaSummary } from '@/widgets/Porudzbine/PorudzbinaSummary'[m
[31m-import { PorudzbinaHeader } from '@/widgets/Porudzbine/PorudzbinaHeader'[m
[31m-import { PorudzbinaItems } from '@/widgets/Porudzbine/PorudzbinaItems'[m
[31m-import { IPorudzbina } from '@/widgets/Porudzbine/models/IPorudzbina'[m
[31m-import { CircularProgress, Grid } from '@mui/material'[m
[31m-import { STYLES_CONSTANTS } from '@/constants'[m
[31m-import { useEffect, useState } from 'react'[m
[31m-import { useRouter } from 'next/router'[m
[31m-import { LSBackButton } from 'ls-core-next'[m
[31m-import { adminApi, handleApiError } from '@/apis/adminApi'[m
[31m-[m
[31m-const Porudzbina = (): JSX.Element => {[m
[31m-    const router = useRouter()[m
[31m-    const oneTimeHash = router.query.hash[m
[31m-[m
[31m-    const [isPretvorUpdating, setIsPretvorUpdating] = useState<boolean>(false)[m
[31m-    const [isDisabled, setIsDisabled] = useState<boolean>(false)[m
[31m-[m
[31m-    const [porudzbina, setPorudzbina] = useState<IPorudzbina | undefined>([m
[31m-        undefined[m
[31m-    )[m
[31m-[m
[31m-    const reloadPorudzbina = (callback?: () => void) => {[m
[31m-        adminApi[m
[31m-            .get(`/orders/${oneTimeHash}`)[m
[31m-            .then((response) => {[m
[31m-                setPorudzbina(response.data)[m
[31m-            })[m
[31m-            .catch((err) => handleApiError(err))[m
[31m-            .finally(() => {[m
[31m-                if (callback) callback()[m
[31m-            })[m
[31m-    }[m
[31m-[m
[31m-    useEffect(() => {[m
[31m-        if (!oneTimeHash) {[m
[31m-            return setPorudzbina(undefined)[m
[31m-        }[m
[31m-[m
[31m-        reloadPorudzbina()[m
[31m-    }, [oneTimeHash])[m
[31m-[m
[31m-    return !porudzbina ? ([m
[31m-        <CircularProgress />[m
[31m-    ) : ([m
[31m-        <Grid[m
[31m-            sx={{[m
[31m-                maxWidth: STYLES_CONSTANTS.UI_DIMENSIONS.MAX_WIDTH,[m
[31m-                margin: `auto`,[m
[31m-            }}[m
[31m-        >[m
[31m-            <LSBackButton[m
[31m-                href={`/korisnici/${porudzbina.username}/porudzbine?userId=${porudzbina.userInformation.id}`}[m
[31m-            />[m
[31m-            <PorudzbinaHeader[m
[31m-                isDisabled={isDisabled}[m
[31m-                porudzbina={porudzbina}[m
[31m-                isTDNumberUpdating={isPretvorUpdating}[m
[31m-                onMestoPreuzimanjaChange={(storeId: number) => {[m
[31m-                    setPorudzbina((prevPorudzbina): any => ({[m
[31m-                        ...prevPorudzbina,[m
[31m-                        storeId: storeId,[m
[31m-                    }))[m
[31m-                }}[m
[31m-            />[m
[31m-            <PorudzbinaActionBar[m
[31m-                isDisabled={isDisabled}[m
[31m-                porudzbina={porudzbina}[m
[31m-                onPreuzmiNaObraduStart={() => {[m
[31m-                    setIsDisabled(true)[m
[31m-                }}[m
[31m-                onPreuzmiNaObraduEnd={() => {[m
[31m-                    reloadPorudzbina(() => {[m
[31m-                        setIsDisabled(false)[m
[31m-                    })[m
[31m-                }}[m
[31m-                onPretvoriUProracunStart={() => {[m
[31m-                    setIsDisabled(true)[m
[31m-                    setIsPretvorUpdating(true)[m
[31m-                }}[m
[31m-                onPretvoriUPonuduStart={() => {}}[m
[31m-                onRazveziOdProracunaStart={() => {[m
[31m-                    setIsDisabled(true)[m
[31m-                    setIsPretvorUpdating(true)[m
[31m-                }}[m
[31m-                onPretvoriUProracunSuccess={() => {[m
[31m-                    reloadPorudzbina(() => {[m
[31m-                        setIsDisabled(false)[m
[31m-                        setIsPretvorUpdating(false)[m
[31m-                    })[m
[31m-                }}[m
[31m-                onPretvoriUProracunFail={() => {}}[m
[31m-                onPretvoriUPonuduEnd={() => {}}[m
[31m-                onRazveziOdProracunaEnd={() => {[m
[31m-                    reloadPorudzbina(() => {[m
[31m-                        setIsDisabled(false)[m
[31m-                        setIsPretvorUpdating(false)[m
[31m-                    })[m
[31m-                }}[m
[31m-                onStornirajStart={() => {[m
[31m-                    setIsDisabled(true)[m
[31m-                }}[m
[31m-                onStornirajSuccess={() => {[m
[31m-                    reloadPorudzbina(() => {[m
[31m-                        setIsDisabled(false)[m
[31m-                    })[m
[31m-                }}[m
[31m-                onStornirajFail={() => {[m
[31m-                    setIsDisabled(false)[m
[31m-                }}[m
[31m-            />[m
[31m-            <PorudzbinaAdminInfo porudzbina={porudzbina} />[m
[31m-            <PorudzbinaItems porudzbina={porudzbina} />[m
[31m-            <PorudzbinaSummary porudzbina={porudzbina} />[m
[31m-        </Grid>[m
[31m-    )[m
[31m-}[m
[31m-[m
[31m-export default Porudzbina[m
[1mdiff --git a/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe/src/widgets/Porudzbine/PorudzbinaSummary/ui/PorudzbinaSummary.tsx b/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe/src/widgets/Porudzbine/PorudzbinaSummary/ui/PorudzbinaSummary.tsx[m
[1mindex 9778cf8c..d5441665 100644[m
[1m--- a/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe/src/widgets/Porudzbine/PorudzbinaSummary/ui/PorudzbinaSummary.tsx[m
[1m+++ b/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe/src/widgets/Porudzbine/PorudzbinaSummary/ui/PorudzbinaSummary.tsx[m
[36m@@ -1,7 +1,7 @@[m
 import { IPorudzbinaSummaryProps } from '../models/IPorudzbinaSummaryProps'[m
 import { formatNumber } from '@/helpers/numberHelpers'[m
 import { mainTheme } from '@/theme'[m
[31m-import { Grid, Typography, styled } from '@mui/material'[m
[32m+[m[32mimport { Grid, Stack, TextField, Typography, styled } from '@mui/material'[m
 [m
 export const PorudzbinaSummary = ([m
     props: IPorudzbinaSummaryProps[m
[36m@@ -14,15 +14,8 @@[m [mexport const PorudzbinaSummary = ([m
     )[m
 [m
     return ([m
[31m-        <Grid[m
[31m-            container[m
[31m-            direction={`column`}[m
[31m-            alignItems={`flex-end`}[m
[31m-            sx={{[m
[31m-                px: 2,[m
[31m-            }}[m
[31m-        >[m
[31m-            <Grid item>[m
[32m+[m[32m        <Grid item>[m
[32m+[m[32m            <Stack>[m
                 <BasicTStyled>[m
                     Osnovica:{' '}[m
                     {formatNumber(props.porudzbina.summary.valueWithoutVAT)}[m
[36m@@ -43,7 +36,7 @@[m [mexport const PorudzbinaSummary = ([m
                     Ušteda:{' '}[m
                     {formatNumber(props.porudzbina.summary.discountValue)}[m
                 </BasicTStyled>[m
[31m-            </Grid>[m
[32m+[m[32m            </Stack>[m
         </Grid>[m
     )[m
 }[m
[1mdiff --git a/src/TD.Web/TD.Web.Public/TD.Web.Public.Fe/package.json b/src/TD.Web/TD.Web.Public/TD.Web.Public.Fe/package.json[m
[1mindex f2d6833e..55f6c781 100644[m
[1m--- a/src/TD.Web/TD.Web.Public/TD.Web.Public.Fe/package.json[m
[1m+++ b/src/TD.Web/TD.Web.Public/TD.Web.Public.Fe/package.json[m
[36m@@ -3,7 +3,7 @@[m
     "version": "0.1.0",[m
     "private": true,[m
     "scripts": {[m
[31m-        "dev": "next dev -p 3001",[m
[32m+[m[32m        "dev": "next dev -p 3000",[m
         "build": "next build",[m
         "start": "next start",[m
         "lint": "next lint"[m
