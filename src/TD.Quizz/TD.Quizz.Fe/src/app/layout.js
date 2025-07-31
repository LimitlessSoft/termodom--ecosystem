import { Geist, Geist_Mono } from 'next/font/google'
import './globals.css'
import { ClientLayout } from '@/app/clientLayout'
import { SessionProvider } from 'next-auth/react'

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata = {
  title: "Termodom Kviz",
  description: "Termodom Kviz - Testiraj svoje znanje",
};

export default function RootLayout({ children }) {
    return (
        <html lang="en">
            <body className={`${geistSans.variable} ${geistMono.variable}`}>
                <SessionProvider>
                    <ClientLayout>
                        {children}
                    </ClientLayout>
                </SessionProvider>
            </body>
        </html>
    );
}
