import { IHeaderLinkProps } from '../models/IHeaderLinkProps';
import { HeaderLinkStyled } from './HeaderLinkStyled';
import { Link, Typography } from '@mui/material';
import NextLink from 'next/link'

export const HeaderLink = (props: IHeaderLinkProps): JSX.Element => {
    return (
        <HeaderLinkStyled
            href={props.href}
            component={NextLink}
            onClick={(e) => {
                if(props.onClick != null)
                    props.onClick(e)
            }}>
            <Typography
                style={{
                    fontFamily: 'GothamProMedium'
                }}>
                {props.text}
            </Typography>
        </HeaderLinkStyled>
    )};