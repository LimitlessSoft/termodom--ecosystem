import { useOtpremniceSubModules } from '../../subModules/useOtpremniceSubModules'
import SubModuleLayout from '../../widgets/SubModuleLayout/ui/SubModuleLayout'

export default function OtpremniceLayout({ children }) {
    const subModules = useOtpremniceSubModules()

    return <SubModuleLayout subModules={subModules}>{children}</SubModuleLayout>
}
