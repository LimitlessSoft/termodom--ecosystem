import ContentLoader from 'react-content-loader'
import { ProizvodiListItemStyled } from '@/widgets/Proizvodi/ProizvodiList/ui/ProizvodiListItemStyled'

export const ProizvodiListContentLoader = () => {
    return (
        <ProizvodiListItemStyled>
            <ContentLoader
                width={`100%`}
                height={`300px`}
                speed={1.5}
                backgroundColor={`#ddd`}
            >
                {/* Image Placeholder */}
                <rect x="5%" y="5" rx="5" ry="5" width="90%" height="45%" />

                {/* Title Placeholder */}
                <rect x="5%" y="55%" rx="3" ry="3" width="90%" height="12" />
                <rect x="5%" y="62%" rx="3" ry="3" width="70%" height="12" />

                {/* Price Section Placeholder */}
                <rect x="5%" y="75%" rx="3" ry="3" width="40%" height="10" />
                <rect x="5%" y="85%" rx="3" ry="3" width="30%" height="12" />
                <rect x="40%" y="85%" rx="3" ry="3" width="30%" height="12" />
            </ContentLoader>
        </ProizvodiListItemStyled>
    )
}
