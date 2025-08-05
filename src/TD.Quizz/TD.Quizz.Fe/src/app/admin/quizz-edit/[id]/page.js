'use client'
import { QuizzEdit } from '@/widgets'
import { useParams } from 'next/navigation'

const AdminQuizzPage = () => {
    const { id } = useParams()
    return <QuizzEdit id={id} />
}
export default AdminQuizzPage
