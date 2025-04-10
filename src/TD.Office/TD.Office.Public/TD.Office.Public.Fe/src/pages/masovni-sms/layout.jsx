import SubModuleLayout from '../../widgets/SubModuleLayout/ui/SubModuleLayout'
import { useMassSMSModules } from '../../subModules/useMassSMSModules'

export default function OtpremniceLayout({ children }) {
    const subModules = useMassSMSModules()

    return <SubModuleLayout subModules={subModules}>{children}</SubModuleLayout>
}
