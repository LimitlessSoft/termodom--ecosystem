import { UseNovacSubModules } from '../../subModules/useNovacSubModules'
import SubModuleLayout from '../../widgets/SubModuleLayout/ui/SubModuleLayout'

export default function NovacLayout({ children }) {
    const subModules = UseNovacSubModules()

    return <SubModuleLayout subModules={subModules}>{children}</SubModuleLayout>
}
