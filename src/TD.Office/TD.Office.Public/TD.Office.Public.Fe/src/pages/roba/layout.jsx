import SubModuleLayout from '../../widgets/SubModuleLayout/ui/SubModuleLayout'
import { useRobaSubModules } from '../../subModules/useRobaSubModules'

export default function RobaLayout({ children }) {
    const subModules = useRobaSubModules()

    return <SubModuleLayout subModules={subModules}>{children}</SubModuleLayout>
}
