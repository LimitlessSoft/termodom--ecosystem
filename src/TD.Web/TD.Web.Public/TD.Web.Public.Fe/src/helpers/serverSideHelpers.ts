import { IServerSideProps } from '@/interfaces/IServerSideProps'
import { PAGE_CONSTANTS } from '@/constants'

export const buildServerSideProps = (props: IServerSideProps<any>) => {
    if (!props.statusCode || props.statusCode == 200)
        return {
            props,
        }

    if (props.statusCode == 404) {
        return {
            notFound: true,
        }
    }

    return {
        redirect: {
            destination: PAGE_CONSTANTS.ERROR(props.statusCode),
            permanent: false,
        },
    }
}
