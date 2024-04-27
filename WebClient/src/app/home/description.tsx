import "../../css/description.scss"
import Image from "next/image";
import temperatureImage from "../../../public/Thermometer.png";
import React from "react";


const creators = [
    "Manuel Jurkovic",
    "Alexis Schaffer",
    "Tobias Röck"
]

const technologies = [
    "C#",
    "Java",
    "HTML",
    "Syntactically Awesome Style Sheets",
    "TypeScript",
    "PostgresSQL",
    "Apache Kafka",
    "Apache Kafka Streams",
    "React & Next.js"
]

export default function Description() {
    return (
        <div>
            <h1 className="center headline">
                <Image className="temperature-logo" src={temperatureImage} alt={""}/>
                Temperature for SFR
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
            {
                items.map((item, index) =>
                    <li key={index}>{item}</li>
                )
            }
        </ul>
    )
}