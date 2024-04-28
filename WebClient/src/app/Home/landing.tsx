import "../../css/landing.scss"
import Image from "next/image";
import temperatureImage from "../../../public/Thermometer.png";
import React from "react";
import {creators, technologies} from "@/app/Home/landingData";

export default function Landing() {
    return (
        <div className={"landing-container"}>
            <h1 className="center headline">
                <Image className="temperature-logo" src={temperatureImage} alt={""}/>
                Tem<wbr/>per<wbr/>a<wbr/>ture for SFR
            </h1>
            <h4 className="center created-by">by {creators.join(", ")}</h4>

            <h2 className="center description">Description</h2>
            <div className="center text">
                This is our project in Software Frameworks, where we use several technologies
                to get used to some of the modern Frameworks out there.<br/>

                <h4>Technologies used:</h4>
                <UnorderedList items={technologies} />
            </div>
        </div>
    )
}

function UnorderedList({items}: {items: string[]}) {
    return (
        <ul>
            {items.map((item, index) =>
                <li key={index}>{item}</li>
            )}
        </ul>);
}