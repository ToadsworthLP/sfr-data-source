"use client"
import styles from "@/css/page.module.scss";
import Menu from "@/app/menu";
import React from "react";
import {useRouter} from "next/navigation";
import API from "@/app/api/api";

export default function Home() {
    const router = useRouter();

    return (
        <main className={styles.main}>
            <Menu onChange={(s) => router.push(`/${s.toLowerCase()}`)}/>
            <API />
        </main>
    );
}
