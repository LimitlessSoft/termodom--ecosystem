import { UIDimensions } from "@/app/constants"
import { PorudzbinaActionBar } from "@/widgets/Porudzbine/PorudzbinaActionbar"
import { PorudzbinaAdminInfo } from "@/widgets/Porudzbine/PorudzbinaAdminInfo"
import { PorudzbinaHeader } from "@/widgets/Porudzbine/PorudzbinaHeader"
import { PorudzbinaItems } from "@/widgets/Porudzbine/PorudzbinaItems"
import { PorudzbinaSummary } from "@/widgets/Porudzbine/PorudzbinaSummary"
import { IPorudzbina } from "@/widgets/Porudzbine/models/IPorudzbina"
import { Grid } from "@mui/material"
import { useRouter } from "next/router"

const Porudzbina = (): JSX.Element => {

    const router = useRouter()
    const oneTimeHash = router.query.id
    
    const mockPorudzbina: IPorudzbina = {
        oneTimeHash: "testhash",
        createdAt: new Date(),
        status: "TestStatus",
        user: "TestUser",
        valueWithVAT: 200,
        discountValue: 400,
        referent: "TestReferent",
        note: "TestNote",
        mobile: "TestMobile",
        name: "TestName",
        items: []
    }

    return (
        <Grid
            sx={{
                maxWidth: UIDimensions.maxWidth,
                margin: `auto`,
            }}>
            <PorudzbinaHeader porudzbina={mockPorudzbina} />
            <PorudzbinaActionBar />
            <PorudzbinaAdminInfo porudzbina={mockPorudzbina} />
            <PorudzbinaItems />
            <PorudzbinaSummary />
        </Grid>
    )
}

export default Porudzbina