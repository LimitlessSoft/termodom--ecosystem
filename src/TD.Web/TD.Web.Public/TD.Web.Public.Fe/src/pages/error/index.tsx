import { useRouter } from 'next/router'

const Error = () => {
    const router = useRouter()
    const { status } = router.query

    return (
        <div>
            <h1>Error Page : {status}</h1>
        </div>
    )
}
export default Error
