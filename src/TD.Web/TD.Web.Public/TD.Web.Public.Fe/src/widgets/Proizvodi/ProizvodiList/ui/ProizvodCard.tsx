import {
    Alert,
    CardActionArea,
    CardContent,
    CardMedia,
    CircularProgress,
    Grid,
    LinearProgress,
    Typography,
} from '@mui/material'
import { ProizvodiListItemStyled } from './ProizvodiListItemStyled'
import { OneTimePrice } from './OneTimePrice'
import { UserPrice } from './UserPrice'
import { ProizvodiListItemTitleStyled } from './ProizvodiListItemTitleStyled'
import NextLink from 'next/link'
import { useRouter } from 'next/router'
import { ClassificationCircleStyled } from '../../styled/ClassificationCircleStyled'
import { CardStyled } from '../styled/CardStyled'
import { useZOverlay } from '@/zStore'

const SESSION_STORAGE_KEY = 'td-web-product-list-url'

const ProizvodCard = (props: any) => {
    const zOverlay = useZOverlay()
    const router = useRouter()

    const handleClick = () => {
        // Save current URL with filters to sessionStorage for back navigation
        if (typeof window !== 'undefined') {
            sessionStorage.setItem(SESSION_STORAGE_KEY, router.asPath)
        }
        zOverlay.show()
    }

    return (
        <ProizvodiListItemStyled item>
            <ClassificationCircleStyled
                classification={props.proizvod.classification}
            />
            <Grid
                component={NextLink}
                href={`/proizvodi/${props.proizvod.src}`}
                sx={{
                    textDecoration: 'none',
                }}
                onClick={handleClick}
            >
                <CardStyled classification={props.proizvod.classification}>
                    <CardActionArea>
                        {props.proizvod ? (
                            <CardMedia
                                sx={{ objectFit: 'contain' }}
                                component={'img'}
                                loading={`eager`}
                                image={props.proizvod.imageData}
                                alt={`need-to-get-from-image-tags`}
                            />
                        ) : (
                            <Grid
                                container
                                sx={{ p: 2 }}
                                justifyContent={`center`}
                            >
                                <CircularProgress />
                            </Grid>
                        )}
                        <CardContent
                            sx={{
                                p: 1,
                                '&:last-child': {
                                    paddingBottom: 1,
                                },
                            }}
                        >
                            <Grid>
                                <ProizvodiListItemTitleStyled>
                                    {props.proizvod.title}
                                </ProizvodiListItemTitleStyled>
                            </Grid>
                            {!props.user ? (
                                <LinearProgress />
                            ) : props.user.isLogged ? (
                                <UserPrice
                                    isWholesale={props.proizvod.isWholesale}
                                    currentGroup={props.currentGroup}
                                    prices={props.proizvod.userPrice}
                                    unit={props.proizvod.unit}
                                />
                            ) : (
                                <OneTimePrice
                                    isWholesale={props.proizvod.isWholesale}
                                    prices={props.proizvod.oneTimePrice}
                                    unit={props.proizvod.unit}
                                    vat={props.proizvod.vat}
                                />
                            )}
                            {props.user?.data?.isAdmin == true && (
                                <Grid
                                    my={2}
                                    fontSize={`0.9em`}
                                    fontStyle={`italic`}
                                >
                                    <Typography>
                                        Prioritetni index:{' '}
                                        {props.proizvod.priorityIndex}
                                    </Typography>
                                </Grid>
                            )}
                        </CardContent>
                    </CardActionArea>
                </CardStyled>
            </Grid>
        </ProizvodiListItemStyled>
    )
}

export default ProizvodCard
