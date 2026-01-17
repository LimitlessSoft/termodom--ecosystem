import SubModuleLayout from '../../widgets/SubModuleLayout/ui/SubModuleLayout'
import { useKorisniciSubModules } from '@/subModules/useKorisniciSubModules'

export default function KorisniciLayout({ children }) {
    const subModules = useKorisniciSubModules()

    return <SubModuleLayout subModules={subModules}>{children}</SubModuleLayout>
}
