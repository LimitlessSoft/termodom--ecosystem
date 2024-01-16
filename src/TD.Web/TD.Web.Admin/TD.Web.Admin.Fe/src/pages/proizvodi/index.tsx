import { ProizvodiActionMenu } from '@/widgets/Proizvodi/ProizvodiActionMenu'
import { ProizvodiProductsList } from '@/widgets/Proizvodi/ProizvodiProductsList'
import { useAppDispatch, useAppSelector, useUser } from '@/app/hooks'

const Proizvodi = (): JSX.Element => {
    
    return (
        <div className={`p-2`}>
            <ProizvodiActionMenu />
            <ProizvodiProductsList />
        </div>
    )
}

export default Proizvodi