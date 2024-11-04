import SubModuleLayout from '../../widgets/SubModuleLayout/ui/SubModuleLayout'
import { useIzvestajiSubModules } from '@/subModules/useIzvestajiSubModules'

export default function IzvestajiLayout({ children }) {
    const subModules = useIzvestajiSubModules()

    return <SubModuleLayout subModules={subModules}>{children}</SubModuleLayout>
}
