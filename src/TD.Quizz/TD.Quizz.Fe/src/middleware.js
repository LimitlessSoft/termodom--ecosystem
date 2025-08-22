import { auth } from '@/auth'
import { NextResponse } from 'next/server'

export default auth((req) => {
    const pathname = req.nextUrl.pathname

    if (pathname !== `/login` && !req.auth)
        return NextResponse.redirect(new URL('/login', req.url))

    if (pathname.startsWith('/admin') && !req.auth?.user?.isAdmin === true) {
        return NextResponse.redirect(new URL('/', req.url))
    }

    return NextResponse.next()
})

export const config = {
    matcher: ['/((?!api|_next/static|_next/image|favicon.ico).*)'],
}
