import { IServerSideProps } from '@/interfaces/IServerSideProps'
import { PAGES } from '@/constants'

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
            destination: PAGES.ERROR(props.statusCode),
            permanent: false,
        },
    }
}
