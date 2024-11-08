import SubModuleLayout from '../../widgets/SubModuleLayout/ui/SubModuleLayout'
import { usePartneriSubModules } from '@/subModules/usePartneriSubModules'

export default function PartneriLayout({ children }) {
    const subModules = usePartneriSubModules()

    return <SubModuleLayout subModules={subModules}>{children}</SubModuleLayout>
}
