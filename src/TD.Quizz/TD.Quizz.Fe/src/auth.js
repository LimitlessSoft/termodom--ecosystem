import NextAuth from "next-auth"
import CredentialsProvider from "next-auth/providers/credentials"
import { userRepository } from '@/superbase/userRepository'
import { NextResponse } from 'next/server'

export const authOptions = {
    providers: [
        CredentialsProvider({
            credentials: {
                username: { label: "Username", type: "text" },
                password: { label: "Password", type: "password" }
            },
            
            async authorize(credentials) {
                try {
                    if (!credentials || !credentials.username || !credentials.password)
                        return null
                    return await userRepository.login(credentials.username, credentials.password)
                } catch (error) {
                    return null
                }
            }
        })
    ],
    callbacks: {
        async jwt({ token, user }) {
            if (user) {
                token.id = user.id
                token.username = user.username
                token.isAdmin = user.isAdmin
            }
            return token
        },
        async session({ session, token }) {
            session.user.id = token.id
            session.user.username = token.username
            session.user.isAdmin = token.isAdmin
            return session
        }
    },
    pages: {
        signIn: "/login"
    },
    session: {
        strategy: "jwt",
    }
};

export const { handlers, signIn, signOut, auth } = NextAuth(authOptions);
