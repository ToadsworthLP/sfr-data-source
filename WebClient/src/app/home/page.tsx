"use client"
import styles from "../../css/page.module.scss";
import Description from "@/app/home/description";
import {useRouter} from "next/navigation";
import Menu from "@/app/menu";
import React from "react";

export default function Home() {
    const router = useRouter();

    return (
        <main className={styles.main}>
            <Menu onChange={(s) => router.push(`/${s.toLowerCase()}`)}/>
            <Description />
        </main>
    );
}
