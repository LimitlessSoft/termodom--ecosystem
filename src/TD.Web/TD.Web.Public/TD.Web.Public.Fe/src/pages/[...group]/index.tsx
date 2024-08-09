import { webApi } from '@/api/webApi'
import { IServerSideProps } from '@/interfaces/IServerSideProps'
import { IProductGroupDto } from '@/dtos'
import { buildServerSideProps } from '@/helpers/serverSideHelpers'
import ProizvodiPage from '@/widgets/Proizvodi/ProizvodiPage/ui/ProizvodiPage'

export async function getServerSideProps(context: any) {
    const group = context.params.group.pop()

    const props: IServerSideProps<IProductGroupDto> = {
        data: null,
        statusCode: null,
    }

    await webApi
        .get(`/products-groups/${group}`)
        .then((responseData) => {
            props.data = responseData.data
        })
        .catch((err) => {
            props.statusCode = err.response.status
        })

    return buildServerSideProps(props)
}

const Group = (props: any) => {
    return <ProizvodiPage currentGroup={props.data} />
}

export default Group
