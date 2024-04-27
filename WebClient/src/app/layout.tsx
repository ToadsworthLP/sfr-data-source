import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "../css/globals.scss";
import React from "react";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
    title: "Tempetature",
    description: "App for SFR",
};

const RootLayout = ({ children }: Readonly<{  children: React.ReactNode; }>) => (
    <html lang="en">
        <body className={inter.className}>{children}</body>
    </html>
);
export default RootLayout;