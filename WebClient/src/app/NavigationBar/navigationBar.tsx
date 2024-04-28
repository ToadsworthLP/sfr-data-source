"use client"
import "../../css/header.scss";

import Link from "next/link";
import { usePathname, useRouter } from "next/navigation"
import { MenuItem, menuItems } from "@/app/NavigationBar/MenuItems";

export default function NavigationBar() {
    const pathname = usePathname();
    const activePage = pathname === "/" ? menuItems[0].name : pathname.substring(1);

    return (
        <header>
            {menuItems.map((menuItem) =>
                <NavigationEntry
                    key={menuItem.id}
                    menuItem={menuItem}
                    activePage={activePage}
                />
            )}
        </header>
    );
}

function NavigationEntry(
    { menuItem, activePage }: { menuItem: MenuItem, activePage: string }
) {
    const router = useRouter();
    const className = menuItem.name === activePage ? "active" : "";

    return (
        <Link key={menuItem.id} href={`./${menuItem.name}`}>
            <div
                className={className}
                onClick={() => router.push(`/${menuItem.name}`)}>
                {menuItem.name}
            </div>
        </Link>
    );
}