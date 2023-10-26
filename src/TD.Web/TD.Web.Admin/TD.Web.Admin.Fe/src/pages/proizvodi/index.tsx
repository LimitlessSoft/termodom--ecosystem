import { ProizvodiActionMenu } from '@/widgets/Proizvodi/ProizvodiActionMenu'
import { ProizvodiProductsList } from '@/widgets/Proizvodi/ProizvodiProductsList'
import { useAppDispatch, useAppSelector } from '@/app/hooks'
import { increment, selectCounter } from '@/features/slices/counterSlice'

const Proizvodi = (): JSX.Element => {

    const counter = useAppSelector(selectCounter)
    const dispatch = useAppDispatch()
    
    return (
        <div className={`p-2`}>
            <ProizvodiActionMenu />
            <ProizvodiProductsList />
{/* 
            <button onClick={() => {
                dispatch(increment())
            }}>Hi: { counter }</button> */}
        </div>
    )
}

export default Proizvodi