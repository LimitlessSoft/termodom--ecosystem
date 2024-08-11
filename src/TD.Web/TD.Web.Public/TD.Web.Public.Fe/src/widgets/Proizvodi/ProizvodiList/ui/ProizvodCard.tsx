import {
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
import { ClassificationCircleStyled } from '../../styled/ClassificationCircleStyled'
import { CardStyled } from '../styled/CardStyled'

export const ProizvodCard = (props: any) => {
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
            >
                <CardStyled classification={props.proizvod.classification}>
                    <CardActionArea>
                        {props.proizvod == null ? (
                            <Grid
                                container
                                sx={{ p: 2 }}
                                justifyContent={`center`}
                            >
                                <CircularProgress />
                            </Grid>
                        ) : (
                            <CardMedia
                                sx={{ objectFit: 'contain' }}
                                component={'img'}
                                loading={`eager`}
                                image={`data:${props.proizvod.imageContentType};base64,${props.proizvod.imageData}`}
                                alt={`need-to-get-from-image-tags`}
                            />
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
                            {props.user == null ? (
                                <LinearProgress />
                            ) : props.user.isLogged ? (
                                <UserPrice
                                    currentGroup={props.currentGroup}
                                    prices={props.proizvod.userPrice}
                                    unit={props.proizvod.unit}
                                />
                            ) : (
                                <OneTimePrice
                                    currentGroup={props.currentGroup}
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
