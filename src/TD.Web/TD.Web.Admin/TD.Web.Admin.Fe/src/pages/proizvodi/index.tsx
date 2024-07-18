import { ProizvodiProductsList } from '@/widgets/Proizvodi/ProizvodiProductsList'
import { ProizvodiActionMenu } from '@/widgets/Proizvodi/ProizvodiActionMenu'

const Proizvodi = () => {
    return (
        <div className={`p-2`}>
            <ProizvodiActionMenu />
            <ProizvodiProductsList />
        </div>
    )
}

export default Proizvodi
