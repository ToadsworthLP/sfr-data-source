import type { Metadata } from "next";

import "../css/globals.scss";
import { Inter } from "next/font/google";
import React from "react";
import NavigationBar from "@/app/NavigationBar/navigationBar";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
    title: "Temperature",
    description: "App for SFR",
};

const RootLayout = ({ children }: Readonly<{  children: React.ReactNode; }>) => (
    <html lang="en">
        <body className={inter.className}>
            <main>
                <NavigationBar />
                {children}
            </main>
        </body>
    </html>
);
export default RootLayout;