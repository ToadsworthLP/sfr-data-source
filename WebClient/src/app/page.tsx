"use client"
import styles from "../css/page.module.scss";
import Menu from "@/app/menu";
import {useRouter} from "next/navigation";
import React from "react";
import Description from "@/app/home/description";

export default function Home() {
    const router = useRouter();


    return (
        <main className={styles.main}>
            <Menu onChange={(s) => router.push(`/${s.toLowerCase()}`)}/>
            <Description/>
        </main>
    );
}
