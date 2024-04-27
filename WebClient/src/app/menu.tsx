"use client"
import "../css/header.scss";
import {useState} from "react";
import Link from "next/link";
import { usePathname } from "next/navigation"

const menus = [
    {
        name: "Home",
        path: ""
    },
    {
        name: "API",
        path: ""
    }
]

export default function Menu({onChange}: {onChange: (name: string) => void}) {
    const name = usePathname();
    const current = name === "/" ? menus[0].name.toLowerCase() : name.substring(1);
    const [active, setActive] = useState(current)

    return (
        <header>
        {
            menus.map((m, i) => {
                const className = active === m.name.toLowerCase() ? "active" : "";
                return <Link key={i} href={"./" + m.name.toLowerCase()}>
                    <div
                    className={className}
                    onClick={() => {
                        setActive(m.name)
                        onChange(m.name)
                    }}>
                        {m.name}
                    </div>
                </Link>
            })
        }
        </header>
    );
}